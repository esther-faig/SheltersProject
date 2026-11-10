import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import PreferenceRow from './PreferenceRow';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchMe, saveMyPreferences } from '../store/profile/profileThunks';
import { fetchPreferences } from '../store/preferences/preferencesThunks';
import { fetchMyShelters, setShelterActive } from '../store/shelters/sheltersThunks';

const IMAGES_URL = import.meta.env.VITE_IMAGES_URL as string;

function ProfileScreen() {
  const dispatch = useAppDispatch();
  const { user, loading, error } = useAppSelector((state) => state.profile);
  const shelters = useAppSelector((state) => state.shelters.items);
  const allPrefs = useAppSelector((state) => state.preferences.items);

  // מצב עריכה: id -> האם חובה (אותו מבנה כמו במסך ההרשמה)
  const [editing, setEditing] = useState(false);
  const [chosen, setChosen] = useState<Record<number, boolean>>({});
  const [saveError, setSaveError] = useState('');

  useEffect(() => {
    document.title = 'הפרופיל שלי';
    dispatch(fetchMe());
    dispatch(fetchMyShelters());
    dispatch(fetchPreferences());   // הרשימה המלאה, צריכה לעריכה
  }, [dispatch]);

  function startEdit() {
    if (!user) return;
    const current: Record<number, boolean> = {};
    user.preferences.forEach((p) => { current[p.preferenceId] = p.isRequired; });
    setChosen(current);
    setSaveError('');
    setEditing(true);
  }

  function handlePrefChange(id: number, checked: boolean, required: boolean) {
    const next = { ...chosen };
    if (checked) next[id] = required;
    else delete next[id];
    setChosen(next);
  }

  async function handleSave() {
    if (!user) return;
    setSaveError('');
    // שומרים את העדיפות הקיימת של מה שכבר היה, ונותנים עדיפות חדשה להעדפות חדשות
    const oldPriority = new Map(user.preferences.map((p) => [p.preferenceId, p.priority]));
    let nextPriority = Math.max(0, ...user.preferences.map((p) => p.priority)) + 1;

    const list = Object.entries(chosen).map(([id, isRequired]) => {
      const preferenceId = Number(id);
      return {
        preferenceId,
        priority: oldPriority.get(preferenceId) ?? nextPriority++,
        isRequired,
      };
    });

    try {
      await dispatch(saveMyPreferences(list)).unwrap();
      await dispatch(fetchMe()).unwrap();   // רענון, כדי שהרשימה בפרופיל תתעדכן
      setEditing(false);
    } catch (err) {
      setSaveError((err as { message: string }).message);
    }
  }

  function removePref(id: number) {
    if (!user) return;
    dispatch(saveMyPreferences(user.preferences.filter((p) => p.preferenceId !== id)));
  }

  if (loading || !user) return <p>טוען...</p>;
  if (error) return <p>שגיאה: {error}</p>;

  return (
    <div className="screen">
      <h2 className="title">הפרופיל שלי</h2>

      <div className="id-card">
        <div className="avatar">{user.fullName.slice(0, 1)}</div>
        <div>
          <strong>{user.fullName}</strong>
          <div>{user.email}</div>
        </div>
      </div>

      <h3>ההעדפות שלי ({user.preferences.length})</h3>

      {editing ? (
        <>
          <div className="prefs">
            {allPrefs.map((p) => (
              <PreferenceRow
                key={p.preferenceId}
                name={p.name}
                checked={p.preferenceId in chosen}
                required={chosen[p.preferenceId] ?? false}
                onChange={(c, r) => handlePrefChange(p.preferenceId, c, r)}
              />
            ))}
          </div>
          {saveError && <p>{saveError}</p>}
          <button className="btn signal small" onClick={handleSave}>שמירה</button>
          <button className="btn small" onClick={() => setEditing(false)}>ביטול</button>
        </>
      ) : (
        <>
          <div className="tags">
            {user.preferences.map((p) => (
              <span key={p.preferenceId} className="chip">
                {p.name} {p.isRequired && '(חובה)'}
                <button onClick={() => removePref(p.preferenceId)}>×</button>
              </span>
            ))}
          </div>
          {user.preferences.length === 0 && <p>עוד לא הגדרת העדפות.</p>}
          <button className="btn small" onClick={startEdit}>
            {user.preferences.length === 0 ? 'הגדרת העדפות' : 'עריכת העדפות'}
          </button>
        </>
      )}

      <h3>המקלטים ששיתפתי</h3>
      {shelters.map((s) => (
        <div key={s.shelterId} className="shelter-card">
          <h3>{s.name}</h3>
          {s.files && s.files.length > 0 && (
            <img
              src={`${IMAGES_URL}/${s.files[0]}`}
              alt={s.name}
              style={{ maxWidth: '100%', maxHeight: 200, objectFit: 'cover' }}
            />
          )}
          <p>{s.address}</p>
          <p>סטטוס: {s.status === 'Pending' ? 'ממתין לאישור' : s.status === 'Approved' ? 'מאושר' : 'נדחה'}</p>
          <button className="btn small"
            onClick={() => dispatch(setShelterActive({ id: s.shelterId, isActive: !s.isActive }))}>
            {s.isActive ? 'השבתה זמנית' : 'הפעלה'}
          </button>
        </div>
      ))}
      <Link to="/share" className="btn small">שיתוף מקלט חדש</Link>
    </div>
  );
}
export default ProfileScreen;