import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Icon from './Icon';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchUsers } from '../store/users/usersThunks';

function UsersScreen() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { items, loading, error } = useAppSelector((state) => state.users);

  useEffect(() => {
    dispatch(fetchUsers());
  }, [dispatch]);

  return (
    <div className="wrap inner" style={{ maxWidth: 900 }}>
      <div className="phead">
        <button
          className="iconbtn"
          type="button"
          onClick={() => navigate('/admin')}
          aria-label="חזרה לאזור הניהול"
        >
          <Icon name="arrow-r" />
        </button>
        <div>
          <h2>משתמשי המערכת</h2>
          <div className="sub">{loading ? 'טוען...' : `${items.length} משתמשים רשומים`}</div>
        </div>
      </div>

      {error ? (
        <div className="card">שגיאה: {error}</div>
      ) : (
        <div className="card">
          {!loading && items.length === 0 && <div className="hint">אין משתמשים להצגה.</div>}
          {items.map((u) => (
            <div className="my-sh" key={u.userId}>
              <div className="grow">
                <h3 style={{ fontSize: 19 }}>{u.fullName}</h3>
                <div className="hint" style={{ direction: 'ltr', textAlign: 'right' }}>{u.email}</div>
              </div>
              {u.sharedShelters && <span className="chip">{u.sharedShelters.length} מקלטים ששותפו</span>}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default UsersScreen;