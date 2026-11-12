
import { useState, type FormEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { login } from '../store/auth/authThunks';
import Icon from './Icon';

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
    <section className="screen active">
      <div className="wrap inner" style={{ maxWidth: 920 }}>
        <div className="phead">
          <button
            type="button"
            className="iconbtn"
            onClick={() => navigate(-1)}
            aria-label="חזרה"
          >
            <Icon name="arrow-r" />
          </button>

          <div>
            <h2>התחברות</h2>
            <div className="sub">שמחים לראות אותך שוב ב-SAFEZONE</div>
          </div>
        </div>

        <div className="cols" id="authCols">
          <div className="card" id="authCard">
            {/* לשוניות התחברות והרשמה */}
            <div className="tabs" role="tablist">
              <Link to="/register" className="tab">
                הרשמה
              </Link>

              <button type="button" className="tab on">
                התחברות
              </button>
            </div>

            <form className="stack" onSubmit={handleSubmit}>
              <div>
                <label className="lbl" htmlFor="login-email">
                  מייל
                </label>
                <div className="field">
                  <Icon name="mail" />
                  <input
                    id="login-email"
                    type="email"
                    dir="ltr"
                    style={{ textAlign: 'right' }}
                    autoComplete="email"
                    placeholder="name@example.com"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                  />
                </div>
              </div>

              <div>
                <label className="lbl" htmlFor="login-password">
                  סיסמה
                </label>
                <div className="field">
                  <Icon name="lock" />
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
                <div className="err" role="alert">
                  {error}
                </div>
              )}

              <button type="submit" className="btn primary block" disabled={loading}>
                <span className="sh"><Icon name="user" /></span>
                {loading ? 'מתחבר...' : 'התחברות'}
              </button>

              <button
                type="button"
                className="btn block"
                onClick={() =>
                  setGoogleMessage('התחברות עם Google תתווסף בהמשך')
                }
              >
                <span className="sh"><Icon name="mail" /></span>
                התחברות עם Google
              </button>

              {googleMessage && (
                <div className="hint" style={{ textAlign: 'center' }}>
                  {googleMessage}
                </div>
              )}
            </form>

            <div className="hint" style={{ textAlign: 'center', marginTop: 18 }}>
              עדיין אין לך חשבון?{' '}
              <Link to="/register">להרשמה לחצי כאן</Link>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

export default LoginScreen;
