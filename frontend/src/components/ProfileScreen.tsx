import { useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchMe, saveMyPreferences } from '../store/profile/profileThunks';
import { fetchMyShelters, setShelterActive } from '../store/shelters/sheltersThunks';

function ProfileScreen() {
  const dispatch = useAppDispatch();
  const { user, loading, error } = useAppSelector((state) => state.profile);
  const shelters = useAppSelector((state) => state.shelters.items);

  useEffect(() => {
    document.title = 'הפרופיל שלי';
    dispatch(fetchMe());
    dispatch(fetchMyShelters());
  }, [dispatch]);

  function removePref(id: number) {
    if (!user) return;
    // שולחים את הרשימה המלאה בלי ההעדפה שהוסרה
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
      <div className="tags">
        {user.preferences.map((p) => (
          <span key={p.preferenceId} className="chip">
            {p.name} {p.isRequired && '(חובה)'}
            <button onClick={() => removePref(p.preferenceId)}>×</button>
          </span>
        ))}
      </div>
      {user.preferences.length === 0 && <p>לא נבחרו העדפות.</p>}

      <h3>המקלטים ששיתפתי</h3>
      {shelters.map((s) => (
        <div key={s.shelterId} className="shelter-card">
          <h3>{s.name}</h3>
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