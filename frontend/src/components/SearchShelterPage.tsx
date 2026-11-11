
import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';

type Coords = {
  lat: number;
  lng: number;
};

function SearchShelterPage() {
  const navigate = useNavigate();

  const [mode, setMode] = useState<'walk' | 'car'>('walk');
  const [coords, setCoords] = useState<Coords | null>(null);
  const [address, setAddress] = useState('');
  const [locating, setLocating] = useState(false);
  const [locError, setLocError] = useState('');
  const [addressAccepted, setAddressAccepted] = useState(false);

  useEffect(() => {
    document.title = 'מציאת מקלט';
  }, []);

  const located = coords !== null || addressAccepted;

  function handleLocate() {
    if (!navigator.geolocation) {
      setLocError('הדפדפן לא תומך באיתור מיקום');
      return;
    }

    setLocating(true);
    setLocError('');

    navigator.geolocation.getCurrentPosition(
      (position) => {
        setCoords({
          lat: position.coords.latitude,
          lng: position.coords.longitude,
        });
        setAddressAccepted(false);
        setLocating(false);
      },
      () => {
        setLocError(
          'לא הצלחנו לאתר את המיקום. יש לאשר גישה למיקום בדפדפן ולנסות שוב.'
        );
        setLocating(false);
      },
      { enableHighAccuracy: true, timeout: 10000 }
    );
  }

  function handleAddressKeyDown(
    event: React.KeyboardEvent<HTMLInputElement>
  ) {
    if (event.key === 'Enter') {
      event.preventDefault();

      if (address.trim()) {
        setAddressAccepted(true);
        setCoords(null);
        setLocError('');
      }
    }
  }

  function handleFindShelter() {
    if (!located && !address.trim()) {
      setLocError('קודם משתפים מיקום או מקלידים כתובת');
      return;
    }

    if (address.trim() && !coords) {
      setAddressAccepted(true);
    }

    navigate(`/results/${mode}`, {
      state: {
        coords,
        address: address.trim(),
        mode,
      },
    });
  }

  return (
    <main className="screen search-page" dir="rtl">
      <div className="search-inner">
        <header className="search-page-head">
          <button
            className="search-back-button"
            type="button"
            onClick={() => navigate(-1)}
            aria-label="חזרה"
          >
            <svg viewBox="0 0 24 24" aria-hidden="true">
              <path d="M15 18l-6-6 6-6" />
            </svg>
          </button>

          <div>
            <h2>מציאת מקלט</h2>
            <p className="search-subtitle">
              משתפים מיקום, בוחרים איך מגיעים — ואנחנו מוצאים.
            </p>
          </div>
        </header>

        <div className="search-columns">
          <div className="search-map" aria-label="מפה סכמטית">
            <svg
              className="search-map-drawing"
              viewBox="0 0 600 340"
              role="img"
              aria-label="מפה סכמטית של האזור"
            >
              <rect width="600" height="340" fill="#eef3f4" />

              {[
                [20, 20, 80, 40],
                [140, 22, 130, 32],
                [310, 18, 140, 40],
                [490, 22, 90, 38],
                [20, 92, 80, 60],
                [140, 92, 130, 60],
                [312, 90, 140, 64],
                [492, 92, 90, 60],
                [22, 190, 76, 56],
                [140, 198, 130, 52],
                [490, 190, 92, 56],
                [22, 284, 80, 40],
                [142, 286, 126, 40],
                [310, 286, 142, 40],
                [492, 286, 88, 40],
              ].map(([x, y, width, height], index) => (
                <rect
                  key={index}
                  x={x}
                  y={y}
                  width={width}
                  height={height}
                  rx="10"
                  fill="#dfe9eb"
                />
              ))}

              {[
                'M0 72 H600',
                'M0 172 H600',
                'M0 266 H600',
                'M120 0 V340',
                'M290 0 V340',
                'M470 0 V340',
                'M0 330 L600 20',
              ].map((street, index) => (
                <g key={index}>
                  <path
                    d={street}
                    stroke="#c3d2d6"
                    strokeWidth="18"
                    fill="none"
                    strokeLinecap="round"
                  />
                  <path
                    d={street}
                    stroke="#ffffff"
                    strokeWidth="13"
                    fill="none"
                    strokeLinecap="round"
                  />
                </g>
              ))}

              <rect
                x="318"
                y="184"
                width="130"
                height="66"
                rx="22"
                fill="#cfe0c4"
                stroke="#263d49"
                strokeWidth="1.6"
                strokeDasharray="5 5"
              />

              {[
                [345, 208],
                [380, 222],
                [415, 206],
                [430, 230],
              ].map(([cx, cy], index) => (
                <circle
                  key={index}
                  cx={cx}
                  cy={cy}
                  r="9"
                  fill="#7da57f"
                  stroke="#263d49"
                  strokeWidth="1.8"
                />
              ))}

              {located && (
                <g transform="translate(300 170)">
                  <circle
                    r="19"
                    fill="#ffffff"
                    stroke="#263d49"
                    strokeWidth="2.6"
                  />
                  <circle
                    r="8.5"
                    fill="none"
                    stroke="#3d6b7a"
                    strokeWidth="3"
                  />
                  <circle r="3.2" fill="#3d6b7a" />
                </g>
              )}
            </svg>

            <div className="search-map-caption">
              {located
                ? 'המיקום נקלט — אפשר להמשיך למציאת מקלט'
                : 'שתפו מיקום כדי לראות מקלטים בסביבה'}
            </div>
          </div>

          <div className="search-controls">
            <section className="search-card">
              <div>
                <span className="search-label">המיקום שלך</span>

                <button
                  className="search-location-button"
                  type="button"
                  onClick={handleLocate}
                  disabled={locating}
                >
                  <span className="search-button-icon">
                    <svg viewBox="0 0 24 24" aria-hidden="true">
                      <circle cx="12" cy="12" r="8" />
                      <circle cx="12" cy="12" r="3" />
                      <path d="M12 2v3M12 19v3M2 12h3M19 12h3" />
                    </svg>
                  </span>

                  <span>
                    {locating
                      ? 'מאתר מיקום...'
                      : located
                        ? 'המיקום שלך נקלט ✓'
                        : 'שתף את המיקום שלי'}
                  </span>
                </button>

                {coords && (
                  <p className="search-coordinate-note">
                    {coords.lat.toFixed(4)}, {coords.lng.toFixed(4)}
                  </p>
                )}

                {locError && (
                  <p className="search-error" role="alert">
                    {locError}
                  </p>
                )}
              </div>

              <div>
                <label className="search-label" htmlFor="search-address">
                  או הזינו כתובת ידנית
                </label>

                <div className="search-address-field">
                  <svg viewBox="0 0 24 24" aria-hidden="true">
                    <path d="M20 10c0 5-8 12-8 12S4 15 4 10a8 8 0 1 1 16 0Z" />
                    <circle cx="12" cy="10" r="2.5" />
                  </svg>

                  <input
                    id="search-address"
                    type="text"
                    placeholder="רחוב ומספר בית, עיר"
                    autoComplete="street-address"
                    value={address}
                    onChange={(event) => {
                      setAddress(event.target.value);
                      setAddressAccepted(false);
                    }}
                    onKeyDown={handleAddressKeyDown}
                  />
                </div>

                {addressAccepted && (
                  <p className="search-coordinate-note">
                    הכתובת נקלטה ✓
                  </p>
                )}
              </div>
            </section>

            <section className="search-mode-section">
              <span className="search-label">אופן הגעה</span>

              <div className="search-modes">
                <button
                  className="search-mode"
                  type="button"
                  aria-pressed={mode === 'walk'}
                  onClick={() => setMode('walk')}
                >
                  <span className="search-mode-bubble">
                    <svg viewBox="0 0 24 24" aria-hidden="true">
                      <circle cx="13" cy="4" r="2" />
                      <path d="M11 8l-2 5 4 2 2 6M9 13l-4 5M11 8l5 3 3 1" />
                    </svg>
                  </span>
                  <span>
                    <strong>הליכה</strong>
                    <small>מסלול רגלי קרוב</small>
                  </span>
                </button>

                <button
                  className="search-mode"
                  type="button"
                  aria-pressed={mode === 'car'}
                  onClick={() => setMode('car')}
                >
                  <span className="search-mode-bubble">
                    <svg viewBox="0 0 24 24" aria-hidden="true">
                      <path d="M5 11l1.5-5h11L19 11l2 2v6h-3v-2H6v2H3v-6l2-2Z" />
                      <path d="M6 11h12M7 14h.01M17 14h.01" />
                    </svg>
                  </span>
                  <span>
                    <strong>רכב</strong>
                    <small>טווח הגעה רחב יותר</small>
                  </span>
                </button>
              </div>
            </section>

            <div className="search-info">
              <span className="search-info-icon">
                <svg viewBox="0 0 24 24" aria-hidden="true">
                  <circle cx="12" cy="12" r="9" />
                  <path d="M12 11v5M12 7h.01" />
                </svg>
              </span>

              <span>
                אפשר לחפש גם בלי להירשם.{' '}
                <Link to="/register">הרשמו</Link> כדי שנתחשב בהעדפות שלכם.
              </span>
            </div>

            <button
              className="search-find-button"
              type="button"
              onClick={handleFindShelter}
            >
              <span className="search-button-icon">
                <svg viewBox="0 0 24 24" aria-hidden="true">
                  <path d="M12 22s8-4 8-11V5l-8-3-8 3v6c0 7 8 11 8 11Z" />
                  <path d="m9 12 2 2 4-4" />
                </svg>
              </span>
              מצא לי מקלט
            </button>

            <div className="search-demo-note">
              הדגמה עיצובית — המפה והמקלטים כאן הם נתוני דוגמה.
            </div>
          </div>
        </div>
      </div>
    </main>
  );
}

export default SearchShelterPage;
