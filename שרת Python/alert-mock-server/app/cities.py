"""קטלוג ישובים לדוגמה + תרחישים מוכנים.

זמני ההתגוננות כאן הם ערכי דוגמה לצורך בדיקות בלבד.
את הטבלה האמיתית יש לברר מול פיקוד העורף (כמופיע ברשימת המשימות).
"""
from .models import AlertType, ScenarioName

# (שם בכתיב פיקוד העורף, זמן התגוננות בשניות - לדוגמה)
CITIES: list[tuple[str, int]] = [
    ("תל אביב - מרכז העיר", 90),
    ("תל אביב - דרום העיר ויפו", 90),
    ("רמת גן - מזרח", 90),
    ("בני ברק", 90),
    ("פתח תקווה", 90),
    ("ירושלים - מרכז", 90),
    ("ירושלים - דרום", 90),
    ("חיפה - מערב", 60),
    ("חיפה - מפרץ", 60),
    ("נתניה", 90),
    ("אשדוד - א,ב,ד,ה", 45),
    ("אשקלון - דרום", 30),
    ("אשקלון - צפון", 30),
    ("שדרות, איבים, ניר עם", 15),
    ("באר שבע - מזרח", 60),
    ("באר שבע - מערב", 60),
    ("קריית שמונה", 0),
    ("בית שמש", 90),
    ("מודיעין-מכבים-רעות", 90),
    ("רחובות", 90),
]

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
