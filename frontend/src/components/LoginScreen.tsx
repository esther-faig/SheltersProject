
import { useState, type FormEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { login } from '../store/auth/authThunks';

function LoginScreen() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const { loading, error } = useAppSelector((state) => state.auth);

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [googleMessage, setGoogleMessage] = useState('');

  async function handleSubmit(e: FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const result = await dispatch(login({ email, password }));

    if (login.fulfilled.match(result)) {
      navigate('/profile');
    }
  }

  return (
    <div className="auth-page">
      <div className="auth-inner">

        <div className="auth-page-head">
          <button
            type="button"
            className="auth-back-button"
            onClick={() => navigate(-1)}
            aria-label="חזרה"
          >
            ←
          </button>

          <div>
            <h2>התחברות</h2>
            <p>שמחים לראות אותך שוב ב-SAFEZONE</p>
          </div>
        </div>

        <div className="login-card auth-card">

          {/* לשוניות התחברות והרשמה */}
          <div className="login-tabs">
            <Link
              to="/register"
              className="login-tab"
            >
              הרשמה
            </Link>

            <button
              type="button"
              className="login-tab active"
            >
              התחברות
            </button>
          </div>

          <form className="auth-form" onSubmit={handleSubmit}>

            <div>
              <label className="auth-label" htmlFor="login-email">
                מייל
              </label>

              <div className="auth-field">
                <input
                  id="login-email"
                  type="email"
                  dir="ltr"
                  autoComplete="email"
                  placeholder="name@example.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                />
              </div>
            </div>

            <div>
              <label className="auth-label" htmlFor="login-password">
                סיסמה
              </label>

              <div className="auth-field">
                <input
                  id="login-password"
                  type="password"
                  autoComplete="current-password"
                  placeholder="הסיסמה שלך"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />
              </div>
            </div>

            {error && (
              <p className="auth-error" role="alert">
                {error}
              </p>
            )}

            <button
              type="submit"
              className="auth-submit-button"
              disabled={loading}
            >
              {loading ? 'מתחבר...' : 'התחברות'}
            </button>

            <button
              type="button"
              className="login-google-button"
              onClick={() =>
                setGoogleMessage('התחברות עם Google תתווסף בהמשך')
              }
            >
              התחברות עם Google
            </button>

            {googleMessage && (
              <p className="login-google-message">
                {googleMessage}
              </p>
            )}

          </form>

          <p className="login-register-footer">
            עדיין אין לך חשבון?
            {' '}
            <Link to="/register">להרשמה לחצי כאן</Link>
          </p>

        </div>
      </div>
    </div>
  );
}

export default LoginScreen;
