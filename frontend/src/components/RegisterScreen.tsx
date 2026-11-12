
import { useEffect, useState, type FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import PreferenceRow from './PreferenceRow';
import Icon from './Icon';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchPreferences } from '../store/preferences/preferencesThunks';
import { register, login } from '../store/auth/authThunks';
import { saveMyPreferences } from '../store/profile/profileThunks';

function RegisterScreen() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const { items: prefs, loading, error } = useAppSelector(
    (state) => state.preferences
  );

  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [chosen, setChosen] = useState<Record<number, boolean>>({});
  const [submitError, setSubmitError] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    document.title = 'הרשמה';
    dispatch(fetchPreferences());
  }, [dispatch]);

  function handlePrefChange(
    id: number,
    checked: boolean,
    required: boolean
  ) {
    setChosen((current) => {
      const next = { ...current };

      if (checked) {
        next[id] = required;
      } else {
        delete next[id];
      }

      return next;
    });
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setSubmitError('');
    setSaving(true);

    try {
      await dispatch(
        register({
          fullName: name.trim(),
          email: email.trim(),
          password,
        })
      ).unwrap();

      await dispatch(login({ email: email.trim(), password })).unwrap();

      const preferenceList = Object.entries(chosen).map(
        ([id, isRequired], index) => ({
          preferenceId: Number(id),
          priority: index + 1,
          isRequired,
        })
      );

      await dispatch(saveMyPreferences(preferenceList)).unwrap();
      navigate('/profile');
    } catch (err) {
      setSubmitError(
        typeof err === 'string'
          ? err
          : (err as { message?: string })?.message ||
              'אירעה שגיאה בהרשמה. נסו שוב.'
      );
    } finally {
      setSaving(false);
    }
  }

  if (loading) {
    return (
      <section className="screen active">
        <div className="wrap inner">
          <div className="card empty">טוען העדפות...</div>
        </div>
      </section>
    );
  }

  if (error) {
    return (
      <section className="screen active">
        <div className="wrap inner">
          <div className="card empty">שגיאה בטעינת ההעדפות: {error}</div>
        </div>
      </section>
    );
  }

  return (
    <section className="screen active">
      <div className="wrap inner" style={{ maxWidth: 920 }}>
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
            <h2>הרשמה והגדרת העדפות</h2>
            <div className="sub">פותחים חשבון ומגדירים מה חשוב לכם במקלט.</div>
          </div>
        </div>

        <div className="cols" id="authCols">
          <div className="card" id="authCard">
            <div className="tabs" role="tablist">
              <button className="tab on" type="button">
                הרשמה
              </button>

              <button
                className="tab"
                type="button"
                onClick={() => navigate('/login')}
              >
                התחברות
              </button>
            </div>

            <form className="stack" onSubmit={handleSubmit}>
              <div>
                <label className="lbl" htmlFor="register-name">
                  שם
                </label>
                <div className="field">
                  <Icon name="user" />
                  <input
                    id="register-name"
                    type="text"
                    autoComplete="name"
                    placeholder="איך לקרוא לך?"
                    value={name}
                    onChange={(event) => setName(event.target.value)}
                    required
                  />
                </div>
              </div>

              <div>
                <label className="lbl" htmlFor="register-email">
                  מייל
                </label>
                <div className="field">
                  <Icon name="mail" />
                  <input
                    id="register-email"
                    type="email"
                    dir="ltr"
                    style={{ textAlign: 'right' }}
                    autoComplete="email"
                    placeholder="name@example.com"
                    value={email}
                    onChange={(event) => setEmail(event.target.value)}
                    required
                  />
                </div>
              </div>

              <div>
                <label className="lbl" htmlFor="register-password">
                  סיסמה
                </label>
                <div className="field">
                  <Icon name="lock" />
                  <input
                    id="register-password"
                    type="password"
                    autoComplete="new-password"
                    placeholder="לפחות 6 תווים"
                    value={password}
                    onChange={(event) => setPassword(event.target.value)}
                    minLength={6}
                    required
                  />
                </div>
              </div>

              {submitError && (
                <div className="err" role="alert">
                  {submitError}
                </div>
              )}

              <button
                className="btn primary block"
                type="submit"
                disabled={
                  saving ||
                  name.trim() === '' ||
                  email.trim() === '' ||
                  password.length < 6
                }
              >
                <span className="sh"><Icon name="shieldcheck" /></span>
                {saving ? 'נרשמים...' : 'הרשמה'}
              </button>
            </form>
          </div>

          <div className="card" id="prefCard">
            <h3 style={{ fontSize: 22, marginBottom: 4 }}>מה חשוב לך במקלט?</h3>

            <div className="hint" style={{ margin: '0 0 12px' }}>
              מסמנים מה מתאים לך, ואפשר לקבוע מה <b>חובה</b> — מקלט שלא עומד
              בזה לא יוצג.
            </div>

            <div id="prefRows">
              <div className="plist">
                {prefs.map((preference) => (
                  <PreferenceRow
                    key={preference.preferenceId}
                    name={preference.name}
                    checked={preference.preferenceId in chosen}
                    required={chosen[preference.preferenceId] ?? false}
                    onChange={(checked, required) =>
                      handlePrefChange(
                        preference.preferenceId,
                        checked,
                        required
                      )
                    }
                  />
                ))}
              </div>
            </div>

            <div className="hint" style={{ marginTop: 14 }}>
              ההעדפות יישמרו יחד עם החשבון לאחר ההרשמה.
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

export default RegisterScreen;
