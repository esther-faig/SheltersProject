import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

function SearchShelterPage() {
  const navigate = useNavigate();
  const [mode, setMode] = useState<string>('walking');
  const [locating, setLocating] = useState(false);
  const [located, setLocated] = useState(false);

  useEffect(() => {
    document.title = 'חיפוש מקלט';
  }, []);

  // סימולציה של איתור מיקום (בהמשך יחליף אותה חיבור אמיתי)
  useEffect(() => {
    if (!locating) return;
    const id = setTimeout(() => {
      setLocating(false);
      setLocated(true);
    }, 1500);
    return () => clearTimeout(id);
  }, [locating]);

  return (
    <div className="screen">
      <h2 className="title">חיפוש מקלט</h2>

      <div className={locating ? 'map-box scanning' : 'map-box'}>
        {located && <div className="pin">📍</div>}
        {locating && <div className="ripple"></div>}
        <p className="map-note">
          {located ? 'המיקום שלך נקלט' : locating ? 'מאתר מיקום...' : 'כאן תוצג המפה'}
        </p>
      </div>

      <button className="btn" onClick={() => setLocating(true)} disabled={locating}>
        שתף את המיקום שלי
      </button>

      <h3>איך מגיעים?</h3>
      <div className="modes">
        <button
          className={mode === 'walking' ? 'mode-card on' : 'mode-card'}
          onClick={() => setMode('walking')}
        >
          <span className="mode-icon">🚶</span>
          הליכה
        </button>
        <button
          className={mode === 'car' ? 'mode-card on' : 'mode-card'}
          onClick={() => setMode('car')}
        >
          <span className="mode-icon">🚗</span>
          רכב
        </button>
      </div>

      <button
        className="btn signal big"
        disabled={!located}
        onClick={() => navigate(`/results/${mode}`)}
      >
        מצא לי מקלט
      </button>
    </div>
  );
}

export default SearchShelterPage;