import { useState, type FormEvent } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { login } from '../store/auth/authThunks';

function LoginScreen() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { loading, error } = useAppSelector((state) => state.auth);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    const result = await dispatch(login({ email, password }));
    if (login.fulfilled.match(result)) navigate('/profile');
  }

  return (
    <div className="screen">
      <h2 className="title">התחברות</h2>
      <form className="form" onSubmit={handleSubmit}>
        <label className="field">מייל
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
        </label>
        <label className="field">סיסמה
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
        </label>
        {error && <p>{error}</p>}
        <button type="submit" className="btn signal" disabled={loading}>
          {loading ? 'מתחבר...' : 'התחברות'}
        </button>
      </form>
      <p>עדיין אין חשבון? <Link to="/register">הרשמה</Link></p>
    </div>
  );
}
export default LoginScreen;