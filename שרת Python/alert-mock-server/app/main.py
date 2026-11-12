"""שרת מדומה של פיקוד העורף - להפעלת אזעקות והתראות דרך Swagger.

הרצה:  uvicorn app.main:app --reload --port 8000
Swagger: http://localhost:8000/docs
"""
import json
from datetime import datetime

from fastapi import FastAPI, HTTPException, Query, Request
from fastapi.exceptions import RequestValidationError
from fastapi.responses import JSONResponse, Response

from .cities import CITIES, CITY_TIMES, SCENARIOS
from .models import (
    ActivateAlertRequest,
    ActiveAlertInfo,
    AlertType,
    CityInfo,
    ScenarioName,
    StatusResponse,
)
from .store import Alert, store

app = FastAPI(
    title="שרת מדומה - פיקוד העורף",
    description=(
        "מדמה את שרת ההתרעות של פיקוד העורף לצורך בדיקות.\n\n"
        "**איך משתמשים:** בקבוצה *הפעלת התרעות* מפעילים אזעקה או התראה, "
        "והאתר (שמבצע polling) יקבל אותה בכתובת "
        "`/WarningMessages/alert/alerts.json` באותו פורמט של השרת האמיתי."
    ),
    version="1.0.0",
)

TAG_SIM = "הפעלת התרעות (Swagger)"
TAG_OREF = "תואם פיקוד העורף (לשימוש האתר)"
TAG_CITIES = "ישובים"


def _to_info(alert: Alert) -> ActiveAlertInfo:
    now = datetime.now()
    return ActiveAlertInfo(
        id=alert.id,
        alert_type=alert.alert_type,
        cities=alert.cities,
        unknown_cities=[c for c in alert.cities if c not in CITY_TIMES],
        started_at=alert.started_at.isoformat(timespec="seconds"),
        expires_at=alert.expires_at.isoformat(timespec="seconds") if alert.expires_at else None,
        seconds_left=(
            max(0, int((alert.expires_at - now).total_seconds())) if alert.expires_at else None
        ),
    )


# ---------- שגיאות בעברית ----------
@app.exception_handler(RequestValidationError)
async def validation_handler(_: Request, exc: RequestValidationError):
    problems = []
    for err in exc.errors():
        field = ".".join(str(p) for p in err["loc"] if p not in ("body", "query", "path"))
        msg = str(err.get("msg", "")).removeprefix("Value error, ")
        problems.append(f"שדה '{field}': {msg}" if field else msg)
    return JSONResponse(status_code=422, content={"detail": "קלט לא תקין", "errors": problems})


# ---------- הפעלת התרעות ----------
@app.post("/simulate/alert", response_model=ActiveAlertInfo, tags=[TAG_SIM],
          summary="הפעלת אזעקה / התראה לישובים לבחירה")
def activate_alert(body: ActivateAlertRequest):
    """בוחרים סוג (`alarm` = אזעקה, `early_warning` = התראה מקדימה), ישובים ומשך."""
    alert = store.activate(body.alert_type, body.cities, body.duration_seconds)
    return _to_info(alert)


@app.post("/simulate/scenario/{name}", response_model=ActiveAlertInfo, tags=[TAG_SIM],
          summary="הפעלת תרחיש מוכן בלחיצה")
def activate_scenario(
    name: ScenarioName,
    duration_seconds: int = Query(60, ge=0, le=3600, description="0 = עד כיבוי ידני"),
):
    alert_type, cities = SCENARIOS[name]
    return _to_info(store.activate(alert_type, cities, duration_seconds))


@app.get("/simulate/status", response_model=StatusResponse, tags=[TAG_SIM],
         summary="אילו התרעות פעילות כרגע")
def status():
    alerts = store.active()
    return StatusResponse(
        active=bool(alerts),
        alerts=[_to_info(a) for a in alerts],
        message="יש התרעות פעילות." if alerts else "אין התרעות פעילות כרגע.",
    )


@app.delete("/simulate/alert/{alert_id}", tags=[TAG_SIM], summary="כיבוי התרעה אחת לפי מזהה")
def stop_alert(alert_id: str):
    if not store.remove(alert_id):
        raise HTTPException(404, f"לא נמצאה התרעה פעילה עם המזהה {alert_id}.")
    return {"message": "ההתרעה כובתה."}


@app.post("/simulate/clear", tags=[TAG_SIM], summary="כיבוי כל ההתרעות")
def clear(reset_history: bool = Query(False, description="לאפס גם את ההיסטוריה")):
    count = store.clear(reset_history)
    return {"message": f"כובו {count} התרעות.", "history_reset": reset_history}


# ---------- פורמט זהה לפיקוד העורף ----------
@app.get("/WarningMessages/alert/alerts.json", tags=[TAG_OREF],
         summary="התרעה נוכחית (כמו alerts.json של פיקוד העורף)")
def oref_current():
    """כשאין התרעה, מוחזר גוף ריק (BOM בלבד) - בדיוק כמו בשרת האמיתי."""
    payload = store.current_oref_payload()
    body = "﻿" if payload is None else "﻿" + json.dumps(payload, ensure_ascii=False)
    return Response(content=body, media_type="application/json; charset=utf-8")


@app.get("/warningMessages/alert/History/AlertsHistory.json", tags=[TAG_OREF],
         summary="היסטוריית התרעות (כמו AlertsHistory.json)")
def oref_history():
    return JSONResponse(store.history())


# ---------- ישובים ----------
@app.get("/cities", response_model=list[CityInfo], tags=[TAG_CITIES],
         summary="קטלוג ישובים לדוגמה (לחיפוש שם מדויק)")
def cities(q: str | None = Query(None, description="חיפוש חלקי בשם")):
    result = [CityInfo(name=n, protection_seconds=s) for n, s in CITIES if not q or q in n]
    return result


@app.get("/", include_in_schema=False)
def root():
    return {"message": "השרת פעיל. ממשק הבדיקות: /docs"}
