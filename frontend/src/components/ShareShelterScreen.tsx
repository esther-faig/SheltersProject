import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

type Extra = { id: number; text: string };

function ShareShelterScreen() {
  const navigate = useNavigate();
  const [shelterName, setShelterName] = useState('');
  const [shelterType, setShelterType] = useState('');
  const [address, setAddress] = useState('');
  const [extras, setExtras] = useState<Extra[]>([]);
  const [extraText, setExtraText] = useState('');

  useEffect(() => {
    document.title = 'שיתוף מקלט';
  }, []);

  function addExtra() {
    if (extraText.trim() === '') return;
    setExtras([...extras, { id: Date.now(), text: extraText }]);
    setExtraText('');
  }

  function removeExtra(id: number) {
    setExtras(extras.filter((x) => x.id !== id));
  }

  function handleKeyDown(e: React.KeyboardEvent<HTMLInputElement>) {
    if (e.key === 'Enter') {
      e.preventDefault();
      addExtra();
    }
  }

  function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    navigate('/profile');
  }

  return (
    <div className="screen">
      <h2 className="title">שיתוף מקלט פרטי</h2>

      <form className="form" onSubmit={handleSubmit}>
        <label className="field">
          שם המקלט
          <input value={shelterName} onChange={(e) => setShelterName(e.target.value)} />
        </label>

        <label className="field">
          סוג המקום
          <select value={shelterType} onChange={(e) => setShelterType(e.target.value)}>
            <option value="">בחר סוג...</option>
            <option value="mamad">ממ"ד פרטי</option>
            <option value="private_shelter">מקלט פרטי</option>
            <option value="other">מרחב מוגן אחר</option>
          </select>
        </label>

        <label className="field">
          מיקום / כתובת
          <input value={address} onChange={(e) => setAddress(e.target.value)} />
        </label>

        <h3>מאפיינים</h3>
        <div className="prefs">
          <label className="pref-row"><input type="checkbox" /> יש מדרגות?</label>
          <label className="pref-row"><input type="checkbox" /> המקום נגיש?</label>
          <label className="pref-row"><input type="checkbox" /> יש אוורור?</label>
          <label className="pref-row"><input type="checkbox" /> המקום מרווח?</label>
        </div>

        <h3>מאפיינים נוספים ({extras.length})</h3>
        <div className="add-row">
          <input
            value={extraText}
            onChange={(e) => setExtraText(e.target.value)}
            onKeyDown={handleKeyDown}
            placeholder="למשל: יש מים"
          />
          <button type="button" className="btn small" onClick={addExtra}>הוסף</button>
        </div>
        <div className="tags">
          {extras.map((x) => (
            <span key={x.id} className="chip">
              {x.text}
              <button type="button" onClick={() => removeExtra(x.id)}>×</button>
            </span>
          ))}
        </div>

        <button
          type="submit"
          className="btn signal"
          disabled={shelterName.trim() === '' || address.trim() === '' || shelterType === ''}
        >
          שיתוף המקלט
        </button>
      </form>
    </div>
  );
}

export default ShareShelterScreen;