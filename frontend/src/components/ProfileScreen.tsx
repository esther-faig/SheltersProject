
import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import PreferenceRow from './PreferenceRow';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchMe, saveMyPreferences } from '../store/profile/profileThunks';
import { fetchPreferences } from '../store/preferences/preferencesThunks';
import { fetchMyShelters, setShelterActive } from '../store/shelters/sheltersThunks';

const IMAGES_URL = import.meta.env.VITE_IMAGES_URL as string;

const STATUS: Record<string, { label: string; className: string }> = {
  Pending: { label: 'ממתין לאישור', className: 'warn' },
  Approved: { label: 'מאושר', className: 'ok' },
  Rejected: { label: 'נדחה', className: 'bad' },
};

function ProfileScreen() {
  const dispatch = useAppDispatch();

  const { user, loading, error } = useAppSelector((state) => state.profile);
  const shelters = useAppSelector((state) => state.shelters.items);
  const allPrefs = useAppSelector((state) => state.preferences.items);

  const [editing, setEditing] = useState(false);
  const [chosen, setChosen] = useState<Record<number, boolean>>({});
  const [saveError, setSaveError] = useState('');

  useEffect(() => {
    document.title = 'הפרופיל שלי';
    dispatch(fetchMe());
    dispatch(fetchMyShelters());
    dispatch(fetchPreferences());
  }, [dispatch]);

  function startEdit() {
    if (!user) return;

    const current: Record<number, boolean> = {};

    user.preferences.forEach((p) => {
      current[p.preferenceId] = p.isRequired;
    });

    setChosen(current);
    setSaveError('');
    setEditing(true);
  }

  function handlePrefChange(
    id: number,
    checked: boolean,
    required: boolean
  ) {
    const next = { ...chosen };

    if (checked) next[id] = required;
    else delete next[id];

    setChosen(next);
  }

  async function handleSave() {
    if (!user) return;

    setSaveError('');

    const oldPriority = new Map(
      user.preferences.map((p) => [p.preferenceId, p.priority])
    );

    let nextPriority =
      Math.max(0, ...user.preferences.map((p) => p.priority)) + 1;

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
      await dispatch(fetchMe()).unwrap();
      setEditing(false);
    } catch (err) {
      setSaveError((err as { message: string }).message);
    }
  }

  function removePref(id: number) {
    if (!user) return;

    dispatch(
      saveMyPreferences(
        user.preferences.filter((p) => p.preferenceId !== id)
      )
    );
  }

  if (loading || !user) {
    return <p className="profile-loading">טוען...</p>;
  }

  if (error) {
    return <p className="profile-error">שגיאה: {error}</p>;
  }

  const requiredPrefs = user.preferences.filter((p) => p.isRequired);
  const preferredPrefs = user.preferences.filter((p) => !p.isRequired);

  return (
    <div className="screen profile-page">
      <div className="profile-inner">

        {/* כותרת */}
        <div className="profile-heading">
          <h2>הפרופיל שלי</h2>
        </div>

        <div className="profile-stack">

          {/* פרטי המשתמש */}
          <div className="profile-card profile-top">
            <div className="profile-avatar">
              {user.fullName.slice(0, 1)}
            </div>

            <div className="profile-user-details">
              <h3>{user.fullName}</h3>
              <div className="profile-email">{user.email}</div>
            </div>
          </div>

          {/* העדפות */}
          <div className="profile-card">

            <div className="profile-row-between">
              <h3>ההעדפות שלי</h3>

              {!editing && (
                <button
                  className="btn small"
                  onClick={startEdit}
                >
                  <span className="profile-button-icon">✎</span>
                  עריכת העדפות
                </button>
              )}
            </div>

            {editing ? (
              <>
                <div className="profile-pref-list">
                  {allPrefs.map((p) => (
                    <PreferenceRow
                      key={p.preferenceId}
                      name={p.name}
                      checked={p.preferenceId in chosen}
                      required={chosen[p.preferenceId] ?? false}
                      onChange={(checked, required) =>
                        handlePrefChange(
                          p.preferenceId,
                          checked,
                          required
                        )
                      }
                    />
                  ))}
                </div>

                {saveError && (
                  <p className="profile-error">{saveError}</p>
                )}

                <div className="profile-actions">
                  <button
                    className="btn primary small"
                    onClick={handleSave}
                  >
                    שמירה
                  </button>

                  <button
                    className="btn small"
                    onClick={() => setEditing(false)}
                  >
                    ביטול
                  </button>
                </div>
              </>
            ) : user.preferences.length > 0 ? (
              <div className="profile-pref-columns">

                <div>
                  <span className="chip warn profile-pref-label">
                    🔒 חובה
                  </span>

                  <div className="profile-pref-items">
                    {requiredPrefs.length > 0 ? (
                      requiredPrefs.map((p) => (
                        <div
                          className="profile-pref-item"
                          key={p.preferenceId}
                        >
                          <span>{p.name}</span>

                          <button
                            type="button"
                            aria-label={`הסרת ${p.name}`}
                            onClick={() => removePref(p.preferenceId)}
                          >
                            ×
                          </button>
                        </div>
                      ))
                    ) : (
                      <div className="profile-empty-value">—</div>
                    )}
                  </div>
                </div>

                <div>
                  <span className="chip ok profile-pref-label">
                    ♥ מועדף
                  </span>

                  <div className="profile-pref-items">
                    {preferredPrefs.length > 0 ? (
                      preferredPrefs.map((p) => (
                        <div
                          className="profile-pref-item"
                          key={p.preferenceId}
                        >
                          <span>{p.name}</span>

                          <button
                            type="button"
                            aria-label={`הסרת ${p.name}`}
                            onClick={() => removePref(p.preferenceId)}
                          >
                            ×
                          </button>
                        </div>
                      ))
                    ) : (
                      <div className="profile-empty-value">—</div>
                    )}
                  </div>
                </div>

              </div>
            ) : (
              <div className="profile-empty">
                עוד לא הגדרת העדפות.
              </div>
            )}
          </div>

          {/* המקלטים ששיתפתי */}
          <div className="profile-card">

            <div className="profile-row-between">
              <h3>המקלטים ששיתפתי</h3>

              <Link
                to="/share"
                className="btn primary small"
              >
                <span className="profile-button-icon">+</span>
                שיתוף מקלט חדש
              </Link>
            </div>

            {shelters.length > 0 ? (
              <div className="profile-shelters">

                {shelters.map((s) => {
                  const status = STATUS[s.status] ?? {
                    label: s.status,
                    className: 'bad',
                  };

                  return (
                    <div
                      className="profile-shelter"
                      key={s.shelterId}
                    >

                      <div className="profile-shelter-thumb">
                        {s.files && s.files.length > 0 ? (
                          <img
                            src={`${IMAGES_URL}/${s.files[0]}`}
                            alt={s.name}
                          />
                        ) : (
                          <span>⌂</span>
                        )}
                      </div>

                      <div className="profile-shelter-info">
                        <strong>{s.name}</strong>

                        <span className={`chip ${status.className}`}>
                          {status.label}
                        </span>

                        <span className="profile-shelter-address">
                          {s.address}
                        </span>
                      </div>

                      <button
                        className="btn small profile-active-button"
                        onClick={() =>
                          dispatch(
                            setShelterActive({
                              id: s.shelterId,
                              isActive: !s.isActive,
                            })
                          )
                        }
                      >
                        {s.isActive ? 'השבתה זמנית' : 'הפעלה'}
                      </button>

                    </div>
                  );
                })}

              </div>
            ) : (
              <div className="profile-empty">
                עוד לא שיתפת מקלט.
                יש לכם ממ״ד? זה הזמן לעזור לשכנים.
              </div>
            )}

          </div>
        </div>
      </div>
    </div>
  );
}

export default ProfileScreen;
