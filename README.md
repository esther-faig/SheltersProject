\# SheltersProject

 תיאור הפרויקט



SheltersProject הוא אתר שיתופי למציאת מרחב מוגן מתאים בזמן התרעה או אזעקה.
המערכת מאפשרת למשתמש למצוא מקלט או מרחב מוגן הנמצא בקרבתו, תוך התחשבות במיקום שלו, בזמן ההגעה האפשרי ובהעדפות שהגדיר, כגון נגישות, היעדר מדרגות ומאפיינים נוספים.
בנוסף, משתמשים יכולים לשתף מרחבים מוגנים פרטיים, כך שמשתמשים אחרים באזור יוכלו להיעזר בהם בזמן חירום.

 טכנולוגיות



\- C#

\- ASP.NET Core

\- React

\- SQL Server

\- Entity Framework Core



 הרצת הפרויקט

1\. יש לפתוח את הקובץ `SheltersProject.sln` ב-Visual Studio.

2\. יש לוודא שמסד הנתונים SQL Server / LocalDB זמין.

3\. יש לוודא שמחרוזת החיבור מוגדרת ב-`appsettings.json`.

4\. יש להריץ את הפרויקט באמצעות `F5` או `Ctrl + F5`.



\### Frontend

1.יש להיכנס לתיקיית ה-React ולהתקין את החבילות:

```bash
npm install

2.**הגדרת מחרוזת החיבור למסד הנתונים.**
   מחרוזת החיבור לא נשמרת בקוד, והיא נקראת ממשתנה סביבה בשם:

   ```
   ConnectionStrings__DefaultConnection
   ```

3. **יצירת מסד הנתונים** מתוך המיגרציות. בתיקיית `backend`:

   או ב-Package Manager Console (Default project: `DAL`):

   ```
   Update-Database
   ```

4. הרצת הפרויקט עם `F5`. ה-API עולה בכתובת `https://localhost:7273`, ו-Swagger נפתח אוטומטית.

- לאחר כל שינוי במחלקות ב-DAL יש ליצור מיגרציה חדשה: `Add-Migration <שם>` ואז `Update-Database`.


## מגבלות ואזכורים
- האתר הוא כלי עזר בלבד. הנתונים עלולים להיות לא מדויקים או לא עדכניים, וההנחיות הרשמיות של פיקוד העורף קודמות תמיד.
- זמן ההתראה המקדימה (8 דקות) הוא ערך קבוע של האתר ואינו נתון רשמי (`Source = 'SitePolicy'` בטבלת `ProtectionTime`).
- אזורי התרעה, גבולות וזמני מיגון מבוססים על [amitfin/oref_alert](https://github.com/amitfin/oref_alert), רישיון MIT. ראו `third_party/oref_alert/LICENSE` ו-`THIRD_PARTY_NOTICES.md`.
