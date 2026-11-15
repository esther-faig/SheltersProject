"""קטלוג ישובים + תרחישים מוכנים.

רשימת הישובים וזמני האזעקה נטענת מ-cities_data.py (נוצר מ-amitfin/oref_alert, רישיון MIT).
הזמנים לא מאומתים, ולכן הם לצורך בדיקות בלבד.
"""
from .cities_data import CITIES
from .models import AlertType, ScenarioName

CITY_TIMES: dict[str, int] = dict(CITIES)

SCENARIOS: dict[ScenarioName, tuple[AlertType, list[str]]] = {
    ScenarioName.tel_aviv_alarm: (AlertType.alarm, ["תל אביב - מרכז העיר"]),
    ScenarioName.jerusalem_alarm: (
        AlertType.alarm,
        ["ירושלים - מרכז", "ירושלים - דרום"],
    ),
    ScenarioName.haifa_alarm: (AlertType.alarm, ["חיפה - מערב", "חיפה - מפרץ"]),
    ScenarioName.south_alarm: (
        AlertType.alarm,
        ["שדרות, איבים, ניר עם", "אשקלון - דרום", "אשקלון - צפון", "באר שבע - מזרח"],
    ),
    ScenarioName.center_early_warning: (
        AlertType.early_warning,
        ["תל אביב - מרכז העיר", "רמת גן - מזרח", "בני ברק", "פתח תקווה"],
    ),
    ScenarioName.south_early_warning: (
        AlertType.early_warning,
        ["שדרות, איבים, ניר עם", "אשקלון - דרום", "באר שבע - מזרח"],
    ),
}

# אם שם בתרחיש לא קיים בנתונים החדשים, מדפיסים אזהרה (במקום להיכשל בשקט)
for _scenario, (_type, _names) in SCENARIOS.items():
    for _name in _names:
        if _name not in CITY_TIMES:
            print(f"אזהרה: התרחיש {_scenario.value} מכיל שם שלא קיים בנתונים: {_name}")