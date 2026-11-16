
import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import PreferencePicker from './PreferencePicker';
import EditShelterDialog from './EditShelterDialog';
import {
  fromUserPreferences,
  toPriorityList,
  type PickedPreference,
} from '../utils/pickedPreferences';
import Icon, { featureIcon } from './Icon';
import ShelterIllustration from './ShelterIllustration';
import { selectIsAdmin } from '../store/auth/authSelectors';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchMe, saveMyPreferences } from '../store/profile/profileThunks';
import { fetchPreferences } from '../store/preferences/preferencesThunks';
import {
  fetchMyShelters,
  setShelterActive,
  deleteShelter,
} from '../store/shelters/sheltersThunks';

const IMAGES_URL = import.meta.env.VITE_IMAGES_URL as string;

const STATUS: Record<string, { label: string; className: string }> = {
  Pending: { label: 'ממתין לאישור', className: 'warn' },
  Approved: { label: 'מאושר', className: 'ok' },
  Rejected: { label: 'נדחה', className: 'bad' },
};

function ProfileScreen() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const isAdmin = useAppSelector(selectIsAdmin);

  const { user, loading, error } = useAppSelector((state) => state.profile);
  const shelters = useAppSelector((state) => state.shelters.items);
  const allPrefs = useAppSelector((state) => state.preferences.items);

  const [editing, setEditing] = useState(false);
  const [picked, setPicked] = useState<PickedPreference[]>([]);
  const [saveError, setSaveError] = useState('');
  const [deleteError, setDeleteError] = useState('');
  const [editingId, setEditingId] = useState<number | null>(null);

  useEffect(() => {
    document.title = 'הפרופיל שלי';
    dispatch(fetchMe());
    dispatch(fetchMyShelters());
    dispatch(fetchPreferences());
  }, [dispatch]);

  function startEdit() {
    if (!user) return;

    setPicked(fromUserPreferences(user.preferences));
    setSaveError('');
    setEditing(true);
  }

  async function handleSave() {
    setSaveError('');

    try {
      await dispatch(saveMyPreferences(toPriorityList(picked))).unwrap();
      await dispatch(fetchMe()).unwrap();
      setEditing(false);
    } catch (err) {
      setSaveError((err as { message: string }).message);
    }
  }

  function removePref(id: number) {
    if (!user) return;

    const remaining = user.preferences
      .filter((p) => p.preferenceId !== id)
      .sort((a, b) => a.priority - b.priority);

    dispatch(saveMyPreferences(toPriorityList(remaining)));
  }

  async function handleDelete(id: number, name: string) {
    if (!window.confirm(`למחוק את המקלט "${name}"? אי אפשר לשחזר אחרי המחיקה.`)) {
      return;
    }

    setDeleteError('');

    try {
      await dispatch(deleteShelter(id)).unwrap();
    } catch (err) {
      setDeleteError((err as { message: string }).message);
    }
  }

  if (loading || !user) {
    return (
      <section className="screen active">
        <div className="wrap inner" style={{ maxWidth: 900 }}>
          <div className="card empty">טוען...</div>
        </div>
      </section>
    );
  }

  if (error) {
    return (
      <section className="screen active">
        <div className="wrap inner" style={{ maxWidth: 900 }}>
          <div className="card empty err">שגיאה: {error}</div>
        </div>
      </section>
    );
  }

  const requiredPrefs = user.preferences.filter((p) => p.isRequired);

  const preferredPrefs = user.preferences
    .filter((p) => !p.isRequired)
    .sort((a, b) => a.priority - b.priority);

  const prefList = (list: typeof user.preferences) =>
    list.length > 0 ? (
      list.map((p) => (
        <div className="pref-line" key={p.preferenceId}>
          <span className="l">
            <Icon name={featureIcon(p.name)} />
            {p.name}
          </span>

          <button
            type="button"
            className="iconx"
            aria-label={`הסרת ${p.name}`}
            onClick={() => removePref(p.preferenceId)}
          >
            <Icon name="x" />
          </button>
        </div>
      ))
    ) : (
      <div className="hint">—</div>
    );

  return (
    <section className="screen active">
      <div
        className="wrap inner"
        style={{ maxWidth: 900 }}
        id="profileBody"
      >
        {/* כותרת */}
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
            <h2>הפרופיל שלי</h2>
          </div>
        </div>

        <div className="stack">
          {/* פרטי המשתמש */}
          <div className="card profile-top">
            <span className="avatar">
              <Icon name="user" />
            </span>

            <div>
              <h3 style={{ fontSize: 24 }}>{user.fullName}</h3>

              <div
                style={{
                  color: 'var(--ink2)',
                  direction: 'ltr',
                  textAlign: 'right',
                }}
              >
                {user.email}
              </div>

              {isAdmin && (
                <span className="chip warn" style={{ marginTop: 6 }}>
                  <Icon name="key" />
                  מנהל מערכת
                </span>
              )}
            </div>
          </div>

          {/* העדפות */}
          <div className="card">
            <div className="row-between" style={{ marginBottom: 14 }}>
              <h3 style={{ fontSize: 22 }}>ההעדפות שלי</h3>

              {!editing && (
                <button className="btn small" onClick={startEdit}>
                  <span className="sh">
                    <Icon name="edit" />
                  </span>
                  עריכת העדפות
                </button>
              )}
            </div>

            {editing ? (
              <>
                <div style={{ marginBottom: 18 }}>
                  <PreferencePicker
                    all={allPrefs}
                    value={picked}
                    onChange={setPicked}
                  />
                </div>

                {saveError && <div className="err">{saveError}</div>}

                <div className="actions">
                  <button
                    className="btn primary small"
                    onClick={handleSave}
                  >
                    <span className="sh">
                      <Icon name="check" />
                    </span>
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
              <div className="cols">
                <div>
                  <span className="chip warn" style={{ marginBottom: 8 }}>
                    <Icon name="lock" />
                    חובה
                  </span>

                  {prefList(requiredPrefs)}
                </div>

                <div>
                  <span className="chip ok" style={{ marginBottom: 8 }}>
                    <Icon name="heart" />
                    מועדף
                  </span>

                  {prefList(preferredPrefs)}
                </div>
              </div>
            ) : (
              <div className="hint">עוד לא הגדרת העדפות.</div>
            )}
          </div>

          {/* המקלטים ששיתפתי */}
          <div className="card">
            <div className="row-between" style={{ marginBottom: 8 }}>
              <h3 style={{ fontSize: 22 }}>המקלטים ששיתפתי</h3>

              <Link to="/share" className="btn small primary">
                <span className="sh">
                  <Icon name="plus" />
                </span>
                שיתוף מקלט חדש
              </Link>
            </div>

            {deleteError && <div className="err">{deleteError}</div>}

            {shelters.length > 0 ? (
              shelters.map((s) => {
                const status = STATUS[s.status] ?? {
                  label: s.status,
                  className: 'bad',
                };

                return (
                  <div className="my-sh" key={s.shelterId}>
                    <div
                      className="thumb"
                      style={{ width: 58, height: 58 }}
                    >
                      {s.files && s.files.length > 0 ? (
                        <img
                          src={`${IMAGES_URL}/${s.files[0]}`}
                          alt={s.name}
                          style={{
                            width: '100%',
                            height: '100%',
                            objectFit: 'cover',
                          }}
                        />
                      ) : (
                        <div style={{ transform: 'scale(.6)' }}>
                          <ShelterIllustration />
                        </div>
                      )}
                    </div>

                    <div className="grow">
                      <b>{s.name}</b>

                      <div style={{ marginTop: 4 }}>
                        <span className={`chip ${status.className}`}>
                          {status.label}
                        </span>
                      </div>

                      <div className="hint">{s.address}</div>

                      {/* סיבת דחייה, אם קיימת */}
                      {s.status === 'Rejected' && s.rejectionReason && (
                        <div className="hint">
                          סיבת הדחייה: {s.rejectionReason}
                        </div>
                      )}
                    </div>

                    {/* כפתורי פעולות למקלט */}
                    <div className="my-sh-actions">
                      {/* עריכת מקלט */}
                      <button
                        className="btn small primary"
                        onClick={() => setEditingId(s.shelterId)}
                      >
                        <span className="sh">
                          <Icon name="edit" />
                        </span>
                        עריכה
                      </button>

                      {/* השבתה זמנית או הפעלה */}
                      <button
                        className="btn small"
                        onClick={() =>
                          dispatch(
                            setShelterActive({
                              id: s.shelterId,
                              isActive: !s.isActive,
                            })
                          )
                        }
                      >
                        <span className="sh">
                          <Icon name={s.isActive ? 'clock' : 'check'} />
                        </span>

                        {s.isActive ? 'השבתה זמנית' : 'הפעלה'}
                      </button>

                      {/* מחיקת מקלט */}
                      <button
                        className="btn small"
                        onClick={() =>
                          handleDelete(s.shelterId, s.name)
                        }
                      >
                        <span className="sh">
                          <Icon name="x" />
                        </span>
                        מחיקה
                      </button>
                    </div>
                  </div>
                );
              })
            ) : (
              <div className="hint" style={{ padding: '10px 0' }}>
                עוד לא שיתפת מקלט. יש לכם ממ״ד? זה הזמן לעזור לשכנים.
              </div>
            )}
          </div>
        </div>
      </div>

      {/* חלונית לעריכת מקלט */}
      {editingId !== null && (
        <EditShelterDialog
          shelterId={editingId}
          onClose={() => setEditingId(null)}
          onSaved={() => dispatch(fetchMyShelters())}
        />
      )}
    </section>
  );
}

export default ProfileScreen;
