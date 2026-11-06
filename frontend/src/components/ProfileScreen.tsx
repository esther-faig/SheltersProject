import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

type Pref = { id: number; name: string };

function ProfileScreen() {
  const user = { name: 'ישראל ישראלי', email: 'israel@example.com' };

  const [prefs, setPrefs] = useState<Pref[]>([
    { id: 1, name: 'ללא מדרגות' },
    { id: 2, name: 'אוורור טוב' },
    { id: 3, name: 'מקומות ישיבה' },
  ]);

  const mySharedShelters = [
    { id: 1, name: 'ממ"ד משפחת ישראלי', address: 'רחוב השלום 10' },
  ];

  useEffect(() => {
    document.title = 'הפרופיל שלי';
  }, []);

  function removePref(id: number) {
    setPrefs(prefs.filter((p) => p.id !== id));
  }

  return (
    <div className="screen">
      <h2 className="title">הפרופיל שלי</h2>

      <div className="id-card">
        <div className="avatar">{user.name.slice(0, 1)}</div>
        <div>
          <strong>{user.name}</strong>
          <div>{user.email}</div>
        </div>
      </div>

      <h3>ההעדפות שלי ({prefs.length})</h3>
      <div className="tags">
        {prefs.map((p) => (
          <span key={p.id} className="chip">
            {p.name}
            <button onClick={() => removePref(p.id)}>×</button>
          </span>
        ))}
      </div>
      {prefs.length === 0 && <p>לא נבחרו העדפות.</p>}
      <Link to="/register" className="btn small">עריכת העדפות</Link>

      <h3>המקלטים ששיתפתי</h3>
      {mySharedShelters.map((s) => (
        <div key={s.id} className="shelter-card">
          <h3>{s.name}</h3>
          <p>{s.address}</p>
          <Link to="/share" className="btn small">עריכת מקלט</Link>
        </div>
      ))}
    </div>
  );
}

export default ProfileScreen;