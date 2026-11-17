import { useEffect, useState } from 'react';
import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';
import Icon, { featureIcon } from './Icon';
import RealMap from './RealMap';
import {
  ShelterSourceBadge,
  ShelterSourceNote,
  TypeChip,
  formatDistance,
  formatTravel,
} from './ShelterSource';
import { googleMapsUrl, wazeUrl } from '../navigation';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { selectIsLoggedIn } from '../store/auth/authSelectors';
import { fetchPreferences } from '../store/preferences/preferencesThunks';
import { getShelter, updateShelterFeatures } from '../api/shelters';
import type { ShelterSearchResult } from '../api/shelterSearch';

type Coords = { lat: number; lng: number };
type DetailsState = { shelter?: ShelterSearchResult; byCar?: boolean; coords?: Coords | null };
type Tri = 'yes' | 'no' | 'unknown';

const TRI_LABELS: { value: Tri; label: string }[] = [
  { value: 'yes', label: 'יש' },
  { value: 'no', label: 'אין' },
  { value: 'unknown', label: 'לא ידוע' },
];

function ShelterDetailsScreen() {
  const { id } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const dispatch = useAppDispatch();
  const state = location.state as DetailsState | null;

  const loggedIn = useAppSelector(selectIsLoggedIn);
  const prefs = useAppSelector((s) => s.preferences.items);

  const mode: 'walk' | 'car' = state?.byCar ? 'car' : 'walk';
  const userCoords = state?.coords ?? null;
  const fromSearch = state?.shelter !== undefined;

  const [shelter, setShelter] = useState<ShelterSearchResult | null>(state?.shelter ?? null);
  const [loading, setLoading] = useState(!state?.shelter);
  const [editing, setEditing] = useState(false);
  const [draft, setDraft] = useState<Record<number, Tri>>({});
  const [saving, setSaving] = useState(false);
  const [message, setMessage] = useState('');
  const [saved, setSaved] = useState(false);

  // כניסה ישירה לכתובת (בלי לעבור בחיפוש): טוענים מהשרת
  useEffect(() => {
    if (shelter) return;
    getShelter(Number(id))
      .then(setShelter)
      .catch(() => setShelter(null))
      .finally(() => setLoading(false));
  }, [id, shelter]);

  useEffect(() => {
    document.title = shelter ? shelter.name : 'פרטי מקלט';
  }, [shelter]);

  useEffect(() => {
    if (loggedIn) dispatch(fetchPreferences());
  }, [dispatch, loggedIn]);

  if (loading) {
    return (
      <section className="screen active">
        <div className="wrap inner">
          <div className="card empty">טוען...</div>
        </div>
      </section>
    );
  }

  if (!shelter) {
    return (
      <section className="screen active">
        <div className="wrap inner">
          <div className="card empty">
            <h2 style={{ fontSize: 26 }}>המקלט לא נמצא</h2>
            <p>לא הצלחנו למצוא את פרטי המקלט המבוקש.</p>
            <div className="actions" style={{ justifyContent: 'center', marginTop: 16 }}>
              <button className="btn primary" onClick={() => navigate(-1)} type="button">
                <span className="sh"><Icon name="arrow-r" /></span>
                חזרה
              </button>
            </div>
          </div>
        </div>
      </section>
    );
  }

  const current = shelter;
  const available = current.features.filter((f) => f.isAvailable === true);

  function startEdit() {
    const next: Record<number, Tri> = {};
    prefs.forEach((p) => {
      const f = current.features.find((x) => x.preferenceId === p.preferenceId);
      next[p.preferenceId] = f?.isAvailable === true ? 'yes' : f?.isAvailable === false ? 'no' : 'unknown';
    });
    setDraft(next);
    setMessage('');
    setSaved(false);
    setEditing(true);
  }

  async function handleSave() {
    // "לא ידוע" לא נשלח: אין מה לעדכן
    const features = prefs
      .filter((p) => draft[p.preferenceId] && draft[p.preferenceId] !== 'unknown')
      .map((p) => ({ preferenceId: p.preferenceId, isAvailable: draft[p.preferenceId] === 'yes' }));

    if (features.length === 0) {
      setMessage('סמנו לפחות מאפיין אחד (יש / אין)');
      return;
    }

    setSaving(true);
    setMessage('');
    try {
      const updated = await updateShelterFeatures(current.shelterId, features);
      setShelter({ ...current, features: updated.features });
      setEditing(false);
      setSaved(true);
    } catch (err) {
      setMessage((err as Error).message || 'העדכון נכשל');
    } finally {
      setSaving(false);
    }
  }

  const here = { lat: current.latitude, lng: current.longitude };

  return (
    <section className="screen active">
      <div className="wrap inner">
        <div className="phead">
          <button className="iconbtn" type="button" onClick={() => navigate(-1)} aria-label="חזרה">
            <Icon name="arrow-r" />
          </button>
          <div>
            <h2>{current.name}</h2>
            <div className="sub">פרטי המקלט והוראות הגעה</div>
          </div>
        </div>

        <div className="cols">
          <div className="stack">
            <div className="card">
              <div className="chips" style={{ marginBottom: 12 }}>
                <TypeChip type={current.shelterType} />
                <ShelterSourceBadge isImported={current.isImported} />
              </div>

              <div className="meta" style={{ fontSize: 15 }}>
                {current.address && <span><Icon name="pin" />{current.address}, {current.city}</span>}
                {fromSearch && (
                  <>
                    <span><Icon name="pin" />{formatDistance(current.distanceMeters)}</span>
                    <span>
                      <Icon name={mode === 'car' ? 'car' : 'walk'} />
                      {formatTravel(current.travelTimeSeconds)}
                    </span>
                  </>
                )}
              </div>

              <span className="lbl">מאפיינים</span>
              {!editing && (
                <div className="chips">
                  {available.length > 0 ? (
                    available.map((f) => (
                      <span className="chip ok" key={f.preferenceId}>
                        <Icon name="check" />
                        {f.name}
                      </span>
                    ))
                  ) : (
                    <span className="hint">לא סומנו מאפיינים</span>
                  )}
                </div>
              )}

              {editing && (
                <div className="plist" style={{ marginTop: 6 }}>
                  {prefs.map((p) => (
                    <div className="prow" key={p.preferenceId}>
                      <span className="lt" style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                        <Icon name={featureIcon(p.name)} />
                        {p.name}
                      </span>
                      <span className="tri" role="group" aria-label={p.name}>
                        {TRI_LABELS.map((o) => (
                          <button
                            key={o.value}
                            type="button"
                            aria-pressed={draft[p.preferenceId] === o.value}
                            onClick={() => setDraft({ ...draft, [p.preferenceId]: o.value })}
                          >
                            {o.label}
                          </button>
                        ))}
                      </span>
                    </div>
                  ))}

                  {message && <div className="err" role="alert">{message}</div>}

                  <div className="actions">
                    <button className="btn primary small" type="button" onClick={handleSave} disabled={saving}>
                      <span className="sh"><Icon name="check" /></span>
                      {saving ? 'שומר...' : 'שמירה'}
                    </button>
                    <button className="btn small" type="button" onClick={() => setEditing(false)}>
                      ביטול
                    </button>
                  </div>
                </div>
              )}

              {saved && <div className="info green" style={{ marginTop: 10 }}><Icon name="check" />המאפיינים עודכנו, תודה!</div>}

              <ShelterSourceNote isImported={current.isImported} />

              {/* עריכה: רק במקלט מיובא, ורק מהמסך הזה */}
              {current.isImported && !editing && (
                loggedIn ? (
                  <button className="btn small soft" type="button" style={{ marginTop: 12 }} onClick={startEdit}>
                    <span className="sh"><Icon name="edit" /></span>
                    עדכון מאפייני המקלט
                  </button>
                ) : (
                  <div className="hint" style={{ marginTop: 10 }}>
                    רוצים לתקן מאפיינים? <Link to="/login">התחברו</Link> כדי לעדכן.
                  </div>
                )
              )}

              {!current.isImported && (
                <div className="hint" style={{ marginTop: 10 }}>
                  מקלט ששיתף משתמש. רק מי ששיתף אותו יכול לערוך את פרטיו.
                </div>
              )}
            </div>

            <a
              className="btn primary block"
              href={googleMapsUrl(current.latitude, current.longitude, mode, userCoords)}
              target="_blank"
              rel="noopener noreferrer"
            >
              <span className="sh"><Icon name="nav" /></span>
              ניווט ב-Google Maps
            </a>

            {mode === 'car' && (
              <a
                className="btn block"
                href={wazeUrl(current.latitude, current.longitude)}
                target="_blank"
                rel="noopener noreferrer"
              >
                <span className="sh"><Icon name="car" /></span>
                ניווט ב-Waze
              </a>
            )}
          </div>

          <RealMap
            center={here}
            userPos={userCoords}
            markers={[{ id: current.shelterId, lat: here.lat, lng: here.lng, title: current.name }]}
            height={420}
          />
        </div>
      </div>
    </section>
  );
}

export default ShelterDetailsScreen;