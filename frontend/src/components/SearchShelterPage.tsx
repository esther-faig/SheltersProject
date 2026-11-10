import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

type Coords = { lat: number; lng: number };

function SearchShelterPage() {
  const navigate = useNavigate();
  const [mode, setMode] = useState<string>('walking');
  const [locating, setLocating] = useState(false);
  const [coords, setCoords] = useState<Coords | null>(null);
  const [locError, setLocError] = useState('');

  useEffect(() => {
    document.title = 'חיפוש מקלט';
  }, []);

  function handleLocate() {
    if (!navigator.geolocation) {
      setLocError('הדפדפן לא תומך באיתור מיקום');
      return;
    }
    setLocating(true);
    setLocError('');
    navigator.geolocation.getCurrentPosition(
      (pos) => {
        setCoords({ lat: pos.coords.latitude, lng: pos.coords.longitude });
        setLocating(false);
      },
      () => {
        setLocError('לא הצלחנו לאתר את המיקום. אשרי גישה למיקום בדפדפן ונסי שוב');
        setLocating(false);
      },
      { enableHighAccuracy: true, timeout: 10000 }
    );
  }

  const located = coords !== null;

  return (
    <div className="screen">
      <h2 className="title">חיפוש מקלט</h2>

      <div className={locating ? 'map-box scanning' : 'map-box'}>
        {located && <div className="pin">📍</div>}
        {locating && <div className="ripple"></div>}
        <p className="map-note">
          {located
            ? `המיקום שלך נקלט (${coords.lat.toFixed(4)}, ${coords.lng.toFixed(4)})`
            : locating
            ? 'מאתר מיקום...'
            : 'כאן תוצג המפה'}
        </p>
      </div>

      <button className="btn" onClick={handleLocate} disabled={locating}>
        שתף את המיקום שלי
      </button>
      {locError && <p>{locError}</p>}

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
        onClick={() => navigate(`/results/${mode}`, { state: coords })}
      >
        מצא לי מקלט
      </button>
    </div>
  );
}

export default SearchShelterPage;