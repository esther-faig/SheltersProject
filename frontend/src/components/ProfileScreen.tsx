import React from 'react';

function ProfileScreen() {
    // נתונים קבועים לדוגמה של המשתמש
    const user = {
        name: "ישראל ישראלי",
        email: "israel@example.com"
    };

    // מערך קבוע של ההעדפות שהמשתמש בחר בעבר
    const myPreferences = [
        "ללא מדרגות",
        "אוורור טוב",
        "מקומות ישיבה"
    ];

    // מערך קבוע של מקלטים שהמשתמש שיתף
    const mySharedShelters = [
        { id: 1, name: 'ממ"ד משפחת ישראלי', address: "רחוב השלום 10" }
    ];

    return (
        <div>
            <h2>הפרופיל שלי</h2>
            
            <div style={{ padding: '10px', border: '1px solid #ccc', marginBottom: '20px' }}>
                <p><strong>שם:</strong> {user.name}</p>
                <p><strong>מייל:</strong> {user.email}</p>
            </div>

            <div>
                <h3>ההעדפות שלי</h3>
                <ul>
                    {myPreferences.map((pref, index) => (
                        <li key={index}>{pref}</li>
                    ))}
                </ul>
                <button>עריכת העדפות</button>
            </div>

            <hr />

            <div>
                <h3>המקלטים ששיתפתי</h3>
                <ul>
                    {mySharedShelters.map((shelter) => (
                        <li key={shelter.id} style={{ marginBottom: '15px' }}>
                            <strong>{shelter.name}</strong> - {shelter.address}
                            <br />
                            <button style={{ marginTop: '5px' }}>עריכת מקלט</button>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
}

export default ProfileScreen;