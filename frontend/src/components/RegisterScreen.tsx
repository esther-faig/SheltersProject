import React, { useState } from 'react';
import PreferenceRow from './PreferenceRow';

interface Preference {
  id: number;
  name: string;
}

const preferences: Preference[] = [
  { id: 1, name: 'ללא מדרגות' },
  { id: 2, name: 'נגיש לכיסא גלגלים' },
  { id: 3, name: 'מרחב מינימלי מסוים' },
  { id: 4, name: 'מרחב גדול' },
  { id: 5, name: 'אוורור טוב' },
  { id: 6, name: 'צפיפות נמוכה' },
  { id: 7, name: 'מקומות ישיבה' },
];

function RegisterScreen() {
  const [isRegistered, setIsRegistered] = useState(false);

  return (
    <div>
      <h2>הרשמה</h2>
      <input type="text" placeholder="שם" />
      <input type="email" placeholder="מייל" />
      <input type="password" placeholder="סיסמה" />
      <button onClick={() => setIsRegistered(true)}>הרשמה</button>

      {isRegistered && (
        <div>
          <h2>מה חשוב לך במקלט?</h2>
          {preferences.map(preference => (
            <PreferenceRow key={preference.id} name={preference.name} />
          ))}
          <button>שמירה</button>
        </div>
      )}
    </div>
  );
}

export default RegisterScreen;
