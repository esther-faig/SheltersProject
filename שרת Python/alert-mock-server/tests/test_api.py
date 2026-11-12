import json

import pytest
from fastapi.testclient import TestClient

from app.main import app
from app.store import store

client = TestClient(app)
CURRENT = "/WarningMessages/alert/alerts.json"


@pytest.fixture(autouse=True)
def _reset():
    store.clear(reset_history=True)
    yield
    store.clear(reset_history=True)


def _oref():
    text = client.get(CURRENT).content.decode("utf-8-sig")
    return json.loads(text) if text.strip() else None


def test_no_alert_returns_empty_body():
    assert _oref() is None
    assert client.get("/simulate/status").json()["active"] is False


def test_alarm_format_matches_oref():
    r = client.post("/simulate/alert", json={"cities": ["תל אביב - מרכז העיר"], "alert_type": "alarm"})
    assert r.status_code == 200
    data = _oref()
    assert data["cat"] == "1"
    assert data["title"] == "ירי רקטות וטילים"
    assert data["data"] == ["תל אביב - מרכז העיר"]
    assert set(data) == {"id", "cat", "title", "data", "desc"}


def test_early_warning_is_different_category():
    client.post("/simulate/alert", json={"cities": ["בני ברק"], "alert_type": "early_warning"})
    assert _oref()["cat"] == "14"


def test_clear_and_stop():
    a = client.post("/simulate/alert", json={"cities": ["בני ברק"]}).json()
    assert client.delete(f"/simulate/alert/{a['id']}").status_code == 200
    assert _oref() is None
    assert client.delete("/simulate/alert/999").status_code == 404
    client.post("/simulate/alert", json={"cities": ["נתניה"]})
    client.post("/simulate/clear")
    assert _oref() is None


def test_scenario_and_history():
    assert client.post("/simulate/scenario/south_alarm").status_code == 200
    hist = client.get("/warningMessages/alert/History/AlertsHistory.json").json()
    assert len(hist) == 4 and hist[0]["category"] == 1


def test_hebrew_validation_errors():
    r = client.post("/simulate/alert", json={"cities": ["  "]})
    assert r.status_code == 422
    assert "לפחות ישוב אחד" in json.dumps(r.json(), ensure_ascii=False)
    r = client.post("/simulate/alert", json={"cities": ["נתניה"], "duration_seconds": -5})
    assert r.status_code == 422


def test_unknown_city_flagged_but_accepted():
    r = client.post("/simulate/alert", json={"cities": ["עיר לא קיימת"]}).json()
    assert r["unknown_cities"] == ["עיר לא קיימת"]


def test_expiry():
    client.post("/simulate/alert", json={"cities": ["נתניה"], "duration_seconds": 1})
    assert _oref() is not None
    import time
    time.sleep(1.2)
    assert _oref() is None
