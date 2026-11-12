
import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Icon from './Icon';
import ShelterMap, { pinAt } from './ShelterMap';
import { shelters } from '../shelters';

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

  const pins = located
    ? shelters.map((s, i) => ({ id: s.id, ...pinAt(i) }))
    : [];

  return (
    <section className="screen active">
      <div className="wrap inner">
        <div className="phead">
          <button
            className="iconbtn"
            type="button"
            onClick={() => navigate(-1)}
            aria-label="חזרה"
          >
            <Icon name="arrow-r" />
          </button>
          <div>
            <h2>מציאת מקלט</h2>
            <div className="sub">
              משתפים מיקום, בוחרים איך מגיעים — ואנחנו מוצאים.
            </div>
          </div>
        </div>

        <div className="cols" id="searchCols">
          <ShelterMap
            id="searchMap"
            user={located}
            pins={pins}
            caption={
              located
                ? 'המקלטים הקרובים אליך'
                : 'שתפו מיקום כדי לראות מקלטים בסביבה'
            }
          />

          <div className="stack">
            <div className="card stack">
              <div>
                <span className="lbl">המיקום שלך</span>
                <button
                  className="btn soft block"
                  type="button"
                  onClick={handleLocate}
                  disabled={locating}
                >
                  <span className="sh"><Icon name="locate" /></span>
                  <span>
                    {locating
                      ? 'מאתר מיקום...'
                      : located
                        ? 'המיקום שלך נקלט ✓'
                        : 'שתף את המיקום שלי'}
                  </span>
                </button>

                {coords && (
                  <div className="hint">
                    {coords.lat.toFixed(4)}, {coords.lng.toFixed(4)}
                  </div>
                )}

                {locError && (
                  <div className="err" role="alert">{locError}</div>
                )}
              </div>

              <div>
                <label className="lbl" htmlFor="search-address">
                  או הזינו כתובת ידנית
                </label>
                <div className="field">
                  <Icon name="pin" />
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
                  <div className="hint">הכתובת נקלטה ✓</div>
                )}
              </div>
            </div>

            <div>
              <span className="lbl">אופן הגעה</span>
              <div className="modes">
                <button
                  className="mode"
                  type="button"
                  aria-pressed={mode === 'walk'}
                  onClick={() => setMode('walk')}
                >
                  <span className="bubble"><Icon name="walk" /></span>
                  <span>
                    <strong>הליכה</strong>
                    <small>מסלול רגלי קרוב</small>
                  </span>
                </button>

                <button
                  className="mode"
                  type="button"
                  aria-pressed={mode === 'car'}
                  onClick={() => setMode('car')}
                >
                  <span className="bubble"><Icon name="car" /></span>
                  <span>
                    <strong>רכב</strong>
                    <small>טווח הגעה רחב יותר</small>
                  </span>
                </button>
              </div>
            </div>

            <div className="info">
              <Icon name="info" />
              <span>
                אפשר לחפש גם בלי להירשם.{' '}
                <Link to="/register">הרשמו</Link> כדי שנתחשב בהעדפות שלכם.
              </span>
            </div>

            <button
              className="btn primary block"
              type="button"
              onClick={handleFindShelter}
            >
              <span className="sh"><Icon name="shieldcheck" /></span>
              מצא לי מקלט
            </button>

            <div className="demo-note">
              הדגמה עיצובית — המפה והמקלטים כאן הם נתוני דוגמה.
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

export default SearchShelterPage;
