import { useEffect, useState } from 'react';
import { Link, Navigate, useLocation, useNavigate, useParams } from 'react-router-dom';
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
import { searchShelters, type ShelterSearchResponse, type ShelterSearchResult } from '../api/shelterSearch';

const IMAGES_URL = import.meta.env.VITE_IMAGES_URL as string;

type Coords = { lat: number; lng: number };
type ResultsState = { coords: Coords; response: ShelterSearchResponse };

function ResultsScreen() {
  const { mode } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const state = location.state as ResultsState | null;

  const [response, setResponse] = useState<ShelterSearchResponse | null>(state?.response ?? null);
  const [byCar, setByCar] = useState(mode === 'car');
  const [searching, setSearching] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    document.title = 'המקלטים המתאימים לך';
  }, []);

  // רענון הדף מאבד את התוצאות: חוזרים לחיפוש
  if (!state || !response) return <Navigate to="/search" replace />;

  const coords = state.coords;
  const results = response.results;

  const markers: MapPoint[] = results.map((s) => ({
    id: s.shelterId,
    lat: s.latitude,
    lng: s.longitude,
    title: s.name,
  }));

  // חיפוש חוזר ברכב: מקלטים רחוקים יותר
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
      setError((err as Error).message || 'החיפוש נכשל. נסו שוב.');
    } finally {
      setSearching(false);
    }
  }

  const driveButton = response.offerDrivingSearch && (
    <button className="btn soft" type="button" onClick={searchByCar} disabled={searching}>
      <span className="sh"><Icon name="car" /></span>
      {searching ? 'מחפש...' : 'אני ברכב — חפש מקלטים רחוקים יותר'}
    </button>
  );

  return (
    <section className="screen active">
      <div className="wrap inner" style={{ maxWidth: 1080 }}>
        <div className="phead">
          <button className="iconbtn" type="button" onClick={() => navigate(-1)} aria-label="חזרה">
            <Icon name="arrow-r" />
          </button>
          <div>
            <h2>המקלטים המתאימים לך</h2>
            <div className="sub">
              {byCar ? 'לפי זמן נסיעה מהמיקום שלך' : 'לפי זמן הליכה מהמיקום שלך'}
            </div>
          </div>
        </div>

        {error && <div className="err" role="alert">{error}</div>}

        <div className="cols" style={{ gridTemplateColumns: '1.25fr 1fr' }} id="resCols">
          <div className="list" id="resList">
            {!response.hasResults || results.length === 0 ? (
              <div className="card empty">
                <span className="bubble"><Icon name="alert" /></span>
                <h3 style={{ fontSize: 24 }}>לא נמצא מקלט מתאים</h3>
                <p>{response.message}</p>

                {response.emergencyGuidance && (
                  <div className="info" style={{ textAlign: 'right', marginTop: 14 }}>
                    <Icon name="info" />
                    <span>
                      <b>{response.emergencyGuidance.title}</b>
                      <br />
                      {response.emergencyGuidance.instructions.split('\n').map((line) => (
                        <span key={line}>
                          {line}
                          <br />
                        </span>
                      ))}
                      <small>מקור: {response.emergencyGuidance.source}</small>
                    </span>
                  </div>
                )}

                <div className="emerg">
                  <div><b>100</b>משטרה</div>
                  <div><b>101</b>מד״א</div>
                  <div><b>102</b>כבאות</div>
                  <div><b>104</b>פיקוד העורף</div>
                </div>

                <div className="actions" style={{ justifyContent: 'center', marginTop: 20 }}>
                  <Link className="btn" to="/search">
                    <span className="sh"><Icon name="sliders" /></span>
                    חזרה לחיפוש
                  </Link>
                  {driveButton}
                </div>
              </div>
            ) : (
              <>
                <ShelterResultCard shelter={results[0]} coords={coords} byCar={byCar} recommended />

                {results.length > 1 && (
                  <>
                    <h3 style={{ fontSize: 20, margin: '6px 4px -6px' }}>אפשרויות נוספות</h3>
                    {results.slice(1).map((shelter) => (
                      <ShelterResultCard
                        key={shelter.shelterId}
                        shelter={shelter}
                        coords={coords}
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
              caption={results.length === 0 ? 'אין מקלטים להצגה' : `${results.length} מקלטים מתאימים בסביבה`}
            />
          </div>
        </div>
      </div>
    </section>
  );
}

type CardProps = {
  shelter: ShelterSearchResult;
  coords: Coords;
  byCar: boolean;
  recommended?: boolean;
};

function ShelterResultCard({ shelter, coords, byCar, recommended = false }: CardProps) {
  const available = shelter.features.filter((f) => f.isAvailable === true);

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
          {shelter.files.length > 0 ? (
            <img
              src={`${IMAGES_URL}/${shelter.files[0]}`}
              alt={shelter.name}
              style={{ width: '100%', height: '100%', objectFit: 'cover' }}
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
            <span><Icon name="pin" />{formatDistance(shelter.distanceMeters)}</span>
            <span>
              <Icon name="clock" />
              {byCar ? 'נסיעה' : 'הליכה'} {formatTravel(shelter.travelTimeSeconds)}
              {shelter.isEstimate ? ' (הערכה)' : ''}
            </span>
          </div>

          {available.length > 0 && (
            <div className="chips">
              {available.map((f) => (
                <span className="chip ok" key={f.preferenceId}>
                  <Icon name="check" />
                  {f.name}
                </span>
              ))}
            </div>
          )}

          {shelter.matchReasons.length > 0 && (
            <ul className="reasons">
              {shelter.matchReasons.map((reason) => (
                <li key={reason}>{reason}</li>
              ))}
            </ul>
          )}

          <ShelterSourceNote isImported={shelter.isImported} />

          <div className="sh-actions">
            <Link
              className={recommended ? 'btn primary small' : 'btn soft small'}
              to={`/shelter/${shelter.shelterId}`}
              state={{ shelter, byCar, coords }}
            >
              <span className="sh"><Icon name="nav" /></span>
              ניווט ופרטים
            </Link>
          </div>
        </div>
      </div>
    </article>
  );
}

export default ResultsScreen;