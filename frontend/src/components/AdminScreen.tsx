import { useState, useEffect } from 'react';

type PendingShelter = { id: number; name: string; type: string; address: string };

function AdminScreen() {
  const [pending, setPending] = useState<PendingShelter[]>([
    { id: 1, name: 'ממ"ד משפחת לוי', type: 'ממ"ד פרטי', address: 'רחוב הרצל 5' },
    { id: 2, name: 'מקלט בניין 4', type: 'מקלט פרטי', address: 'רחוב ויצמן 12' },
  ]);

  useEffect(() => {
    document.title = 'ניהול מקלטים';
  }, []);

  function handleAction(id: number) {
    setPending(pending.filter((s) => s.id !== id));
  }

  return (
    <div className="screen">
      <h2 className="title">ניהול ואישור מקלטים</h2>
      <p className="lead">ממתינים לאישור: {pending.length}</p>

      {pending.length === 0 ? (
        <p>אין מקלטים הממתינים לאישור 🎉</p>
      ) : (
        pending.map((s) => (
          <div key={s.id} className="shelter-card">
            <h3>{s.name}</h3>
            <p>סוג: {s.type}</p>
            <p>כתובת: {s.address}</p>
            <button className="btn teal small" onClick={() => handleAction(s.id)}>אישור</button>
            <button className="btn signal small" onClick={() => handleAction(s.id)}>דחייה</button>
          </div>
        ))
      )}
    </div>
  );
}

export default AdminScreen;