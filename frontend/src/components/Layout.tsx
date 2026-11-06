import { useState, useEffect } from 'react';
import { Link, NavLink, Outlet } from 'react-router-dom';

function Layout() {
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
          <NavLink to="/share">שיתוף מקלט</NavLink>
          <NavLink to="/profile">הפרופיל שלי</NavLink>
          <NavLink to="/register">הרשמה</NavLink>
          <NavLink to="/admin">ניהול</NavLink>
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