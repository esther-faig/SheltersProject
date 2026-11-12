"""מודלים (DTO) עם ולידציות - כל הודעות השגיאה בעברית."""
from enum import Enum

from pydantic import BaseModel, Field, field_validator


class AlertType(str, Enum):
    """סוג ההתרעה."""

    alarm = "alarm"  # אזעקה - ירי רקטות וטילים (cat=1)
    early_warning = "early_warning"  # התראה מקדימה (cat=14)


class ScenarioName(str, Enum):
    tel_aviv_alarm = "tel_aviv_alarm"
    jerusalem_alarm = "jerusalem_alarm"
    haifa_alarm = "haifa_alarm"
    south_alarm = "south_alarm"
    center_early_warning = "center_early_warning"
    south_early_warning = "south_early_warning"


class ActivateAlertRequest(BaseModel):
    cities: list[str] = Field(
        ...,
        description="רשימת ישובים/אזורי התרעה (בכתיב של פיקוד העורף, למשל 'תל אביב - מרכז העיר').",
    )
    alert_type: AlertType = Field(
        AlertType.alarm, description="alarm = אזעקה, early_warning = התראה מקדימה."
    )
    duration_seconds: int = Field(
        60,
        ge=0,
        le=3600,
        description="כמה שניות ההתרעה נשארת פעילה לפני שנעלמת מעצמה. 0 = עד כיבוי ידני.",
    )

    @field_validator("cities")
    @classmethod
    def _clean_cities(cls, value: list[str]) -> list[str]:
        cleaned: list[str] = []
        for city in value:
            city = city.strip()
            if city and city not in cleaned:
                cleaned.append(city)
        if not cleaned:
            raise ValueError("יש להזין לפחות ישוב אחד.")
        if len(cleaned) > 300:
            raise ValueError("ניתן להזין עד 300 ישובים בהתרעה אחת.")
        return cleaned

    model_config = {
        "json_schema_extra": {
            "examples": [
                {
                    "cities": ["תל אביב - מרכז העיר"],
                    "alert_type": "alarm",
                    "duration_seconds": 60,
                }
            ]
        }
    }


class ActiveAlertInfo(BaseModel):
    id: str
    alert_type: AlertType
    cities: list[str]
    unknown_cities: list[str] = Field(
        default_factory=list,
        description="ישובים שלא קיימים בקטלוג הדוגמה (עדיין נשלחים, רק אזהרה).",
    )
    started_at: str
    expires_at: str | None
    seconds_left: int | None


class StatusResponse(BaseModel):
    active: bool
    alerts: list[ActiveAlertInfo]
    message: str


class CityInfo(BaseModel):
    name: str
    protection_seconds: int
