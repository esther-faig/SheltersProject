"""מצב ההתרעות בזיכרון. תפוגה מחושבת בזמן הקריאה - אין תהליכי רקע."""
import time
from dataclasses import dataclass
from datetime import datetime, timedelta
from threading import Lock

from .models import AlertType

# קטגוריות ופורמט כמו ב-API של פיקוד העורף
OREF_CATEGORY = {AlertType.alarm: "1", AlertType.early_warning: "14"}
OREF_HISTORY_CATEGORY = {AlertType.alarm: 1, AlertType.early_warning: 14}
OREF_TITLE = {
    AlertType.alarm: "ירי רקטות וטילים",
    AlertType.early_warning: "בדקות הקרובות צפויות להתקבל התרעות באזורך",
}
OREF_DESC = {
    AlertType.alarm: "היכנסו למרחב המוגן ושהו בו 10 דקות",
    AlertType.early_warning: "על תושבי האזורים הבאים לשפר את המיקום למיגון המיטבי ולהישאר בקרבתו",
}


@dataclass
class Alert:
    id: str
    alert_type: AlertType
    cities: list[str]
    started_at: datetime
    expires_at: datetime | None  # None = עד כיבוי ידני

    def is_active(self, now: datetime) -> bool:
        return self.expires_at is None or now < self.expires_at


class AlertStore:
    def __init__(self) -> None:
        self._lock = Lock()
        self._alerts: list[Alert] = []
        self._history: list[dict] = []

    @staticmethod
    def _now() -> datetime:
        return datetime.now()

    def activate(self, alert_type: AlertType, cities: list[str], duration: int) -> Alert:
        now = self._now()
        alert = Alert(
            id=str(int(time.time() * 10_000_000)),
            alert_type=alert_type,
            cities=list(cities),
            started_at=now,
            expires_at=now + timedelta(seconds=duration) if duration > 0 else None,
        )
        with self._lock:
            self._alerts.append(alert)
            for city in cities:
                self._history.append(
                    {
                        "alertDate": now.strftime("%Y-%m-%d %H:%M:%S"),
                        "title": OREF_TITLE[alert_type],
                        "data": city,
                        "category": OREF_HISTORY_CATEGORY[alert_type],
                    }
                )
        return alert

    def active(self) -> list[Alert]:
        now = self._now()
        with self._lock:
            self._alerts = [a for a in self._alerts if a.is_active(now)]
            return list(self._alerts)

    def remove(self, alert_id: str) -> bool:
        with self._lock:
            before = len(self._alerts)
            self._alerts = [a for a in self._alerts if a.id != alert_id]
            return len(self._alerts) < before

    def clear(self, reset_history: bool = False) -> int:
        with self._lock:
            count = len(self._alerts)
            self._alerts = []
            if reset_history:
                self._history = []
            return count

    def history(self) -> list[dict]:
        with self._lock:
            return list(reversed(self._history))  # החדש ראשון, כמו אצל פיקוד העורף

    def current_oref_payload(self) -> dict | None:
        """האובייקט כפי שמחזיר alerts.json האמיתי. None = אין התרעה פעילה."""
        alerts = self.active()
        if not alerts:
            return None
        newest = alerts[-1]
        cities: list[str] = []
        for a in alerts:
            if a.alert_type == newest.alert_type:
                for c in a.cities:
                    if c not in cities:
                        cities.append(c)
        return {
            "id": newest.id,
            "cat": OREF_CATEGORY[newest.alert_type],
            "title": OREF_TITLE[newest.alert_type],
            "data": cities,
            "desc": OREF_DESC[newest.alert_type],
        }


store = AlertStore()
