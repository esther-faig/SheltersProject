import React, { useState } from 'react';
import PreferenceRow from './PreferenceRow';

function RegisterScreen() {
    // שימוש ב-useState לשמירת נתוני ההרשמה של המשתמש
    const [name, setName] = useState<string>('');
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');

    // מערך קבוע (Hardcoded) של ההעדפות לפי האפיון
    const preferencesList = [
        "ללא מדרגות",
        "נגיש לכיסא גלגלים",
        "מרחב מינימלי מסוים",
        "מרחב גדול",
        "אוורור טוב",
        "צפיפות נמוכה",
        "מקומות ישיבה"
    ];

    return (
        <div>
            <h2>הרשמה למערכת</h2>
            
            {/* חלק ראשון: פרטי משתמש */}
            <div style={{ border: '1px solid #ccc', padding: '15px', marginBottom: '20px' }}>
                <div>
                    <label>שם: </label>
                    <input type="text" onChange={(e) => setName(e.target.value)} />
                </div>
                <br />
                <div>
                    <label>מייל: </label>
                    <input type="email" onChange={(e) => setEmail(e.target.value)} />
                </div>
                <br />
                <div>
                    <label>סיסמה: </label>
                    <input type="password" onChange={(e) => setPassword(e.target.value)} />
                </div>
                <br />
                <button>הרשמה</button>
            </div>

            {/* חלק שני: בחירת העדפות */}
            <div>
                <h3>מה חשוב לך במקלט?</h3>
                {/* שימוש ב-map כדי לעבור על המערך ולהציג את הקומפוננטה שיצרנו */}
                {preferencesList.map((pref, index) => (
                    <PreferenceRow key={index} name={pref} />
                ))}
                <br />
                <button>שמירה</button>
            </div>
        </div>
    );
}

export default RegisterScreen;