import { useState, useEffect, type FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import PreferenceRow from './PreferenceRow';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchPreferences } from '../store/preferences/preferencesThunks';
import { register, login } from '../store/auth/authThunks';
import { saveMyPreferences } from '../store/profile/profileThunks';

function RegisterScreen() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { items: prefs, loading, error } = useAppSelector((state) => state.preferences);

  // ערכי טופס: state רגיל
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [chosen, setChosen] = useState<Record<number, boolean>>({});   // id -> האם חובה
  const [submitError, setSubmitError] = useState('');

  useEffect(() => {
    document.title = 'הרשמה';
    dispatch(fetchPreferences());
  }, [dispatch]);

  function handlePrefChange(id: number, checked: boolean, required: boolean) {
    const next = { ...chosen };
    if (checked) next[id] = required;
    else delete next[id];
    setChosen(next);
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setSubmitError('');
    try {
      await dispatch(register({ fullName: name, email, password })).unwrap();
      await dispatch(login({ email, password })).unwrap();
      const list = Object.entries(chosen).map(([id, isRequired], i) => ({
        preferenceId: Number(id),
        priority: i + 1,
        isRequired,
      }));
      await dispatch(saveMyPreferences(list)).unwrap();
      navigate('/profile');
    } catch (err) {
      setSubmitError((err as { message: string }).message);
    }
  }

  if (loading) return <p>טוען...</p>;
  if (error) return <p>שגיאה: {error}</p>;

  return (
    <div className="screen">
      <h2 className="title">הרשמה למערכת</h2>
      <form className="form" onSubmit={handleSubmit}>
        <label className="field">שם
          <input value={name} onChange={(e) => setName(e.target.value)} />
        </label>
        <label className="field">מייל
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
        </label>
        <label className="field">סיסמה
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
        </label>

        <h3>מה חשוב לך במקלט?</h3>
        <div className="prefs">
          {prefs.map((p) => (
            <PreferenceRow
              key={p.preferenceId}
              name={p.name}
              checked={p.preferenceId in chosen}
              required={chosen[p.preferenceId] ?? false}
              onChange={(c, r) => handlePrefChange(p.preferenceId, c, r)}
            />
          ))}
        </div>

        {submitError && <p>{submitError}</p>}
        <button
          type="submit"
          className="btn signal"
          disabled={name.trim() === '' || email.trim() === '' || password.length < 6}
        >
          הרשמה ושמירה
        </button>
      </form>
    </div>
  );
}
export default RegisterScreen;