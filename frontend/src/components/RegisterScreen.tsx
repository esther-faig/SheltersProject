import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import PreferenceRow from './PreferenceRow';

const preferencesList = [
  'ללא מדרגות',
  'נגיש לכיסא גלגלים',
  'מרחב מינימלי מסוים',
  'מרחב גדול',
  'אוורור טוב',
  'צפיפות נמוכה',
  'מקומות ישיבה',
];

function RegisterScreen() {
  const navigate = useNavigate();
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  useEffect(() => {
    document.title = 'הרשמה';
  }, []);

  function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    // בהמשך כאן תישלח הבקשה לשרת
    navigate('/profile');
  }

  return (
    <div className="screen">
      <h2 className="title">הרשמה למערכת</h2>

      <form className="form" onSubmit={handleSubmit}>
        <label className="field">
          שם
          <input value={name} onChange={(e) => setName(e.target.value)} />
        </label>
        <label className="field">
          מייל
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
        </label>
        <label className="field">
          סיסמה
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
        </label>

        <h3>מה חשוב לך במקלט?</h3>
        <div className="prefs">
          {preferencesList.map((pref) => (
            <PreferenceRow key={pref} name={pref} />
          ))}
        </div>

        <button
          type="submit"
          className="btn signal"
          disabled={name.trim() === '' || email.trim() === '' || password.length < 6}
        >
          הרשמה ושמירה
        </button>
      </form>
    </div>
  );
}

export default RegisterScreen;