
import { useEffect, useState } from 'react';
import {
  Link,
  Navigate,
  useLocation,
  useNavigate,
  useParams,
} from 'react-router-dom';

import Icon from './Icon';
import RealMap, { type MapPoint } from './RealMap';
import ShelterIllustration from './ShelterIllustration';

import {
  ShelterSourceBadge,
  ShelterSourceNote,
  TypeChip,
  formatDistance,
  formatTravel,
  
} from './ShelterSource';

import {
  searchShelters,
  type ShelterSearchResponse,
  type ShelterSearchResult,
} from '../api/shelterSearch';

const IMAGES_URL = import.meta.env.VITE_IMAGES_URL as string;

type Coords = {
  lat: number;
  lng: number;
};

type ResultsState = {
  coords: Coords;
  response: ShelterSearchResponse;
};

function ResultsScreen() {
  const { mode } = useParams();
  const navigate = useNavigate();
  const location = useLocation();

  const state = location.state as ResultsState | null;

  const [response, setResponse] = useState<ShelterSearchResponse | null>(
    state?.response ?? null
  );

  const [byCar, setByCar] = useState(mode === 'car');
  const [searching, setSearching] = useState(false);
  const [error, setError] = useState('');
  const [startedAt] = useState(() => Date.now());

  useEffect(() => {
    document.title = 'המקלטים המתאימים לך';
  }, []);

  // אם המשתמש רענן את הדף ואין נתוני חיפוש, חוזרים למסך החיפוש
  if (!state || !response) {
    return <Navigate to="/search" replace />;
  }

  const coords = state.coords;
  const results = response.results ?? [];
  const deadline =
  response.maxSecondsToReach > 0 ? startedAt + response.maxSecondsToReach * 1000 : null;

  const markers: MapPoint[] = results.map((shelter) => ({
    id: shelter.shelterId,
    lat: shelter.latitude,
    lng: shelter.longitude,
    title: shelter.name,
  }));

  // חיפוש חוזר ברכב ללא שינוי בהגדרות ההתרעה שבשרת
  async function searchByCar() {
    setSearching(true);
    setError('');

    try {
      const next = await searchShelters({
        latitude: coords.lat,
        longitude: coords.lng,
        travelMode: 'DRIVING',
      });

      setResponse(next);
      setByCar(true);
    } catch (err) {
      setError(
        (err as Error).message || 'החיפוש נכשל. נסו שוב.'
      );
    } finally {
      setSearching(false);
    }
  }

  const driveButton = response.offerDrivingSearch && (
    <button
      className="btn soft"
      type="button"
      onClick={searchByCar}
      disabled={searching}
    >
      <span className="sh">
        <Icon name="car" />
      </span>

      {searching
        ? 'מחפש...'
        : 'אני ברכב — חפש מקלטים רחוקים יותר'}
    </button>
  );

  return (
    <section className="screen active">
      <div className="wrap inner" style={{ maxWidth: 1080 }}>
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
            <h2>המקלטים המתאימים לך</h2>

            <div className="sub">
              {byCar
                ? 'לפי זמן נסיעה מהמיקום שלך'
                : 'לפי זמן הליכה מהמיקום שלך'}
            </div>
          </div>
        </div>

        {(response.areaName || response.maxSecondsToReach > 0) && (
          <div className="info" style={{ marginBottom: 16 }}>
            <Icon name="info" />

            <span>
              {response.areaName && (
                <span>
                  אזור ההתגוננות: <b>{response.areaName}</b>
                  {response.maxSecondsToReach > 0 && <br />}
                </span>
              )}

              {response.maxSecondsToReach > 0 && (
                <span>
                  זמן ההתגוננות באזור: {response.maxSecondsToReach} שניות
                </span>
              )}
            </span>
          </div>
        )}

        {error && (
          <div className="err" role="alert">
            {error}
          </div>
        )}
        {deadline !== null && <Countdown deadline={deadline} />}

        <div
          className="cols"
          style={{ gridTemplateColumns: '1.25fr 1fr' }}
          id="resCols"
        >
          <div className="list" id="resList">
            {!response.hasResults || results.length === 0 ? (
              <div className="card empty">
                <span className="bubble">
                  <Icon name="alert" />
                </span>

                <h3 style={{ fontSize: 24 }}>
                  לא נמצא מקלט מתאים
                </h3>

                <p>{response.message}</p>

                {response.emergencyGuidance && (
                  <div
                    className="info"
                    style={{
                      textAlign: 'right',
                      marginTop: 14,
                    }}
                  >
                    <Icon name="info" />

                    <span>
                      <b>{response.emergencyGuidance.title}</b>
                      <br />

                      {response.emergencyGuidance.instructions
                        .split('\n')
                        .map((line, index) => (
                          <span key={index}>
                            {line}
                            <br />
                          </span>
                        ))}

                      <small>
                        מקור: {response.emergencyGuidance.source}
                      </small>
                    </span>
                  </div>
                )}

                <div className="emerg">
                  <div>
                    <b>100</b>
                    משטרה
                  </div>

                  <div>
                    <b>101</b>
                    מד״א
                  </div>

                  <div>
                    <b>102</b>
                    כבאות
                  </div>

                  <div>
                    <b>104</b>
                    פיקוד העורף
                  </div>
                </div>

                <div
                  className="actions"
                  style={{
                    justifyContent: 'center',
                    marginTop: 20,
                  }}
                >
                  <Link className="btn" to="/search">
                    <span className="sh">
                      <Icon name="sliders" />
                    </span>

                    חזרה לחיפוש
                  </Link>

                  {driveButton}
                </div>
              </div>
            ) : (
              <>
                <ShelterResultCard
                  shelter={results[0]}
                  byCar={byCar}
                  recommended
                />

                {results.length > 1 && (
                  <>
                    <h3
                      style={{
                        fontSize: 20,
                        margin: '6px 4px -6px',
                      }}
                    >
                      אפשרויות נוספות
                    </h3>

                    {results.slice(1).map((shelter) => (
                      <ShelterResultCard
                        key={shelter.shelterId}
                        shelter={shelter}
                        byCar={byCar}
                      />
                    ))}
                  </>
                )}

                {driveButton}
              </>
            )}
          </div>

          <div className="sticky">
            <RealMap
              center={coords}
              userPos={coords}
              markers={markers}
              height={460}
              caption={
                results.length === 0
                  ? 'אין מקלטים להצגה'
                  : `${results.length} מקלטים מתאימים בסביבה`
              }
            />
          </div>
        </div>
      </div>
    </section>
  );
}
const remaining = (deadline: number) => Math.max(0, Math.round((deadline - Date.now()) / 1000));

function Countdown({ deadline }: { deadline: number }) {
  const [left, setLeft] = useState(() => remaining(deadline));

  useEffect(() => {
    setLeft(remaining(deadline));
    const id = window.setInterval(() => setLeft(remaining(deadline)), 1000);
    return () => window.clearInterval(id);
  }, [deadline]);

  const minutes = Math.floor(left / 60);
  const seconds = String(left % 60).padStart(2, '0');

  return (
    <div className={`countdown${left === 0 ? ' over' : left <= 30 ? ' low' : ''}`} role="timer">
      <Icon name="clock" />
      <div>
        <div className="countdown-label">
          {left > 0 ? 'זמן שנותר להגיע למקלט' : 'הזמן הסתיים — היכנסו למרחב המוגן הקרוב ביותר'}
        </div>
        <div className="countdown-time">{minutes}:{seconds}</div>
        <div className="hint">הספירה מתחילה ברגע שהתוצאות מוצגות</div>
      </div>
    </div>
  );
}

type CardProps = {
  shelter: ShelterSearchResult;
  byCar: boolean;
  recommended?: boolean;
};

function ShelterResultCard({
  shelter,
  byCar,
  recommended = false,
}: CardProps) {
  const available = (shelter.features ?? []).filter(
    (feature) => feature.isAvailable === true
  );

  return (
    <article className={recommended ? 'card reco' : 'card'}>
      {recommended && (
        <div className="ribbon">
          <Icon name="star" />
          המקלט המומלץ עבורך
        </div>
      )}

      <div className="sh-card">
        <div className="thumb">
          {shelter.files?.length > 0 ? (
            <img
              src={`${IMAGES_URL}/${shelter.files[0]}`}
              alt={shelter.name}
              style={{
                width: '100%',
                height: '100%',
                objectFit: 'cover',
              }}
            />
          ) : (
            <ShelterIllustration />
          )}
        </div>

        <div className="sh-body">
          <h3>{shelter.name}</h3>

          <div className="chips" style={{ marginBottom: 10 }}>
            <TypeChip type={shelter.shelterType} />

            <ShelterSourceBadge isImported={shelter.isImported} />
          </div>

          <div className="meta">
            <span>
              <Icon name="pin" />
              {formatDistance(shelter.distanceMeters)}
            </span>

            <span>
              <Icon name="clock" />
              {byCar ? 'נסיעה' : 'הליכה'}{' '}
              {formatTravel(shelter.travelTimeSeconds)}
              {shelter.isEstimate ? ' (הערכה)' : ''}
            </span>
          </div>

          {available.length > 0 && (
            <div className="chips">
              {available.map((feature) => (
                <span
                  className="chip ok"
                  key={feature.preferenceId}
                >
                  <Icon name="check" />
                  {feature.name}
                </span>
              ))}
            </div>
          )}

          {shelter.matchReasons?.length > 0 && (
            <ul className="reasons">
              {shelter.matchReasons.map((reason, index) => (
                <li key={`${shelter.shelterId}-${index}`}>
                  {reason}
                </li>
              ))}
            </ul>
          )}

          <ShelterSourceNote isImported={shelter.isImported} />

          <div className="sh-actions">
            <Link
              className={
                recommended
                  ? 'btn primary small'
                  : 'btn soft small'
              }
              to={`/shelter/${shelter.shelterId}`}
              state={{ shelter, byCar }}
            >
              <span className="sh">
                <Icon name="nav" />
              </span>

              ניווט ופרטים
            </Link>
          </div>
        </div>
      </div>
    </article>
  );
}

export default ResultsScreen;
