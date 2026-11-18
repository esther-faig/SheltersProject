
import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Icon from './Icon';
import RealMap from './RealMap';
import { geocodeAddress } from '../api/geocode';
import { searchShelters } from '../api/shelterSearch';

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
  const [searching, setSearching] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    document.title = 'מציאת מקלט';
  }, []);

  const located = coords !== null || address.trim() !== '';

  function handleLocate() {
    if (!navigator.geolocation) {
      setError('הדפדפן לא תומך באיתור מיקום');
      return;
    }

    setLocating(true);
    setError('');

    navigator.geolocation.getCurrentPosition(
      (position) => {
        setCoords({
          lat: position.coords.latitude,
          lng: position.coords.longitude,
        });

        setAddress('');
        setLocating(false);
      },
      () => {
        setError(
          'לא הצלחנו לאתר את המיקום. יש לאשר גישה למיקום בדפדפן ולנסות שוב.'
        );
        setLocating(false);
      },
      {
        enableHighAccuracy: true,
        timeout: 10000,
      }
    );
  }

  async function handleFindShelter() {
    if (!located) {
      setError('קודם משתפים מיקום או מקלידים כתובת');
      return;
    }

    setError('');
    setSearching(true);

    try {
      let origin = coords;

      // אם המשתמש הזין כתובת, ממירים אותה לקואורדינטות
      if (!origin) {
        origin = await geocodeAddress(address.trim());

        if (!origin) {
          setError(
            'לא מצאנו את הכתובת. נסו להוסיף עיר, או שתפו את המיקום שלכם.'
          );
          return;
        }

        setCoords(origin);
      }

      const response = await searchShelters({
        latitude: origin.lat,
        longitude: origin.lng,
        travelMode: mode === 'car' ? 'DRIVING' : 'WALKING',
      });

      navigate(`/results/${mode}`, {
        state: {
          coords: origin,
          response,
        },
      });
    } catch (err) {
      setError((err as Error).message || 'החיפוש נכשל. נסו שוב.');
    } finally {
      setSearching(false);
    }
  }

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
          <RealMap
            center={coords}
            userPos={coords}
            height={460}
            caption={located ? 'המיקום שלך' : 'שתפו מיקום כדי להתחיל'}
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
                  <span className="sh">
                    <Icon name="locate" />
                  </span>

                  <span>
                    {locating
                      ? 'מאתר מיקום...'
                      : coords
                        ? 'המיקום שלך נקלט ✓'
                        : 'שתף את המיקום שלי'}
                  </span>
                </button>

                {coords && (
                  <div className="hint">
                    {coords.lat.toFixed(4)}, {coords.lng.toFixed(4)}
                  </div>
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
                      setCoords(null);
                    }}
                    onKeyDown={(event) => {
                      if (event.key === 'Enter') {
                        event.preventDefault();
                        handleFindShelter();
                      }
                    }}
                  />
                </div>
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
                  <span className="bubble">
                    <Icon name="walk" />
                  </span>

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
                  <span className="bubble">
                    <Icon name="car" />
                  </span>

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
                אפשר לחפש גם בלי להירשם. <Link to="/register">הרשמו</Link> כדי
                שנתחשב בהעדפות שלכם.
              </span>
            </div>

            {error && (
              <div className="err" role="alert">
                {error}
              </div>
            )}

            <button
              className="btn primary block"
              type="button"
              onClick={handleFindShelter}
              disabled={searching}
            >
              <span className="sh">
                <Icon name="shieldcheck" />
              </span>

              {searching ? 'מחפש...' : 'מצא לי מקלט'}
            </button>
          </div>
        </div>
      </div>
    </section>
  );
}

export default SearchShelterPage;
