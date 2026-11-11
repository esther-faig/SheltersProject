
import { useEffect, useState, type FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import PreferenceRow from './PreferenceRow';
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
      <main className="screen auth-page" dir="rtl">
        <div className="auth-loading">טוען העדפות...</div>
      </main>
    );
  }

  if (error) {
    return (
      <main className="screen auth-page" dir="rtl">
        <div className="auth-loading">שגיאה בטעינת ההעדפות: {error}</div>
      </main>
    );
  }

  return (
    <main className="screen auth-page" dir="rtl">
      <div className="auth-inner">
        <header className="auth-page-head">
          <button
            className="auth-back-button"
            type="button"
            onClick={() => navigate(-1)}
            aria-label="חזרה"
          >
            <svg viewBox="0 0 24 24" aria-hidden="true">
              <path d="m15 18-6-6 6-6" />
            </svg>
          </button>

          <div>
            <h2>הרשמה והגדרת העדפות</h2>
            <p>פותחים חשבון ומגדירים מה חשוב לכם במקלט.</p>
          </div>
        </header>

        <div className="auth-columns">
          <section className="auth-card">
            <div className="auth-tabs">
              <button className="auth-tab active" type="button">
                הרשמה
              </button>

              <button
                className="auth-tab"
                type="button"
                onClick={() => navigate('/login')}
              >
                התחברות
              </button>
            </div>

            <form className="auth-form" onSubmit={handleSubmit}>
              <div>
                <label className="auth-label" htmlFor="register-name">
                  שם
                </label>
                <div className="auth-field">
                  <svg viewBox="0 0 24 24" aria-hidden="true">
                    <circle cx="12" cy="8" r="4" />
                    <path d="M4 21c0-4 3-7 8-7s8 3 8 7" />
                  </svg>
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
                <label className="auth-label" htmlFor="register-email">
                  מייל
                </label>
                <div className="auth-field">
                  <svg viewBox="0 0 24 24" aria-hidden="true">
                    <rect x="3" y="5" width="18" height="14" rx="2" />
                    <path d="m3 7 9 6 9-6" />
                  </svg>
                  <input
                    id="register-email"
                    type="email"
                    dir="ltr"
                    autoComplete="email"
                    placeholder="name@example.com"
                    value={email}
                    onChange={(event) => setEmail(event.target.value)}
                    required
                  />
                </div>
              </div>

              <div>
                <label className="auth-label" htmlFor="register-password">
                  סיסמה
                </label>
                <div className="auth-field">
                  <svg viewBox="0 0 24 24" aria-hidden="true">
                    <rect x="4" y="10" width="16" height="11" rx="2" />
                    <path d="M8 10V7a4 4 0 0 1 8 0v3M12 14v3" />
                  </svg>
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
                <div className="auth-error" role="alert">
                  {submitError}
                </div>
              )}

              <button
                className="auth-submit-button"
                type="submit"
                disabled={
                  saving ||
                  name.trim() === '' ||
                  email.trim() === '' ||
                  password.length < 6
                }
              >
                <svg viewBox="0 0 24 24" aria-hidden="true">
                  <path d="M12 22s8-4 8-11V5l-8-3-8 3v6c0 7 8 11 8 11Z" />
                  <path d="m9 12 2 2 4-4" />
                </svg>
                {saving ? 'נרשמים...' : 'הרשמה'}
              </button>
            </form>
          </section>

          <section className="auth-card auth-preferences-card">
            <h3>מה חשוב לך במקלט?</h3>

            <p className="auth-hint">
              מסמנים מה מתאים לך, ואפשר לקבוע מה <strong>חובה</strong> —
              מקלט שלא עומד בזה לא יוצג.
            </p>

            <div className="auth-preferences">
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

            <div className="auth-preferences-note">
              ההעדפות יישמרו יחד עם החשבון לאחר ההרשמה.
            </div>
          </section>
        </div>
      </div>
    </main>
  );
}

export default RegisterScreen;
