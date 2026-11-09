import { useState, useEffect } from 'react';
import { Link, NavLink, Outlet } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { selectIsLoggedIn, selectIsAdmin } from '../store/auth/authSelectors';
import { logout } from '../store/auth/authThunks';

function Layout() {
  const dispatch = useAppDispatch();
  const loggedIn = useAppSelector(selectIsLoggedIn);
  const isAdmin = useAppSelector(selectIsAdmin);
  const [time, setTime] = useState(new Date());

  useEffect(() => {
    const id = setInterval(() => {
      setTime(new Date());
    }, 1000);
    return () => clearInterval(id);
  }, []);

  return (
    <div className="app">
      <div className="ticker">
        <div className="ticker-track">
          <span>⚠ בזמן אזעקה: היכנסו למרחב המוגן הקרוב ביותר • התרחקו מחלונות • המתינו 10 דקות לפחות • ⚠ בזמן אזעקה: היכנסו למרחב המוגן הקרוב ביותר • התרחקו מחלונות • המתינו 10 דקות לפחות • </span>
          <span>⚠ בזמן אזעקה: היכנסו למרחב המוגן הקרוב ביותר • התרחקו מחלונות • המתינו 10 דקות לפחות • ⚠ בזמן אזעקה: היכנסו למרחב המוגן הקרוב ביותר • התרחקו מחלונות • המתינו 10 דקות לפחות • </span>
        </div>
      </div>

      <header className="topbar">
        <Link to="/" className="brand">
          <span className="brand-dot"></span>SafeZone
        </Link>
        <nav className="nav">
          <NavLink to="/search">חיפוש מקלט</NavLink>
          {loggedIn ? (
            <>
              <NavLink to="/share">שיתוף מקלט</NavLink>
              <NavLink to="/profile">הפרופיל שלי</NavLink>
              {isAdmin && <NavLink to="/admin">ניהול</NavLink>}
              <button onClick={() => dispatch(logout())}>התנתקות</button>
            </>
          ) : (
            <>
              <NavLink to="/login">התחברות</NavLink>
              <NavLink to="/register">הרשמה</NavLink>
            </>
          )}
        </nav>
        <div className="clock">{time.toLocaleTimeString()}</div>
      </header>

      <main className="page">
        <Outlet />
      </main>
    </div>
  );
}

export default Layout;