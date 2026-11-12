# שרת מדומה של פיקוד העורף

שרת FastAPI שמדמה אזעקות והתראות, כדי לבדוק את אתר המקלטים בלי להיות תלויה באזעקות אמיתיות.

## הרצה

Windows: לחיצה כפולה על `run.bat`  
Linux/Mac: `./run.sh`

או ידנית:
```
pip install -r requirements.txt
uvicorn app.main:app --reload --port 8000
```

ממשק Swagger: http://localhost:8000/docs

## איך מפעילים אזעקה

1. נכנסים ל-`/docs`.
2. **`POST /simulate/alert`** - בוחרים `alert_type` (`alarm` = אזעקה, `early_warning` = התראה מקדימה), רשימת ישובים ומשך בשניות (0 = עד כיבוי ידני).
3. או **`POST /simulate/scenario/{name}`** - תרחיש מוכן בלחיצה (למשל `tel_aviv_alarm`).
4. **`POST /simulate/clear`** מכבה הכול.

כדי למצוא את השם המדויק של ישוב: `GET /cities?q=תל`.

## מה האתר שלך מתחבר אליו

| | שרת אמיתי | שרת מדומה |
|---|---|---|
| התרעה נוכחית | `https://www.oref.org.il/WarningMessages/alert/alerts.json` | `http://localhost:8000/WarningMessages/alert/alerts.json` |
| היסטוריה | `https://www.oref.org.il/warningMessages/alert/History/AlertsHistory.json` | `http://localhost:8000/warningMessages/alert/History/AlertsHistory.json` |

הפורמט זהה, לכן מספיק להחליף Base URL בקונפיגורציה (`appsettings.json`).

דוגמת תשובה בזמן אזעקה:
```json
{"id":"134...","cat":"1","title":"ירי רקטות וטילים","data":["תל אביב - מרכז העיר"],"desc":"היכנסו למרחב המוגן ושהו בו 10 דקות"}
```
- `cat = "1"` אזעקה, `cat = "14"` התראה מקדימה.
- כשאין התרעה, הגוף ריק (BOM בלבד), בדיוק כמו בשרת האמיתי. הקוד שלך צריך לטפל בזה.
- אם כמה התרעות מאותו סוג פעילות, הישובים מאוחדים להתרעה אחת.

## בדיקות
```
pytest
```

## הערות
- זמני ההתגוננות ב-`app/cities.py` הם ערכי דוגמה בלבד.
- המצב נשמר בזיכרון, ואיתחול השרת מאפס אותו.
- פורמט הנתונים של השרת האמיתי נכתב לפי מה שידוע לי, וייתכן שהשתנה. מומלץ לוודא מול תשובה אמיתית בזמן אזעקה.
