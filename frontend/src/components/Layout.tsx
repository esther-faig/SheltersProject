import { Link, NavLink, Outlet } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import {
  selectIsLoggedIn,
  selectIsAdmin,
} from '../store/auth/authSelectors';
import { logout } from '../store/auth/authThunks';
import Icon from './Icon';

function Layout() {
  const dispatch = useAppDispatch();
  const loggedIn = useAppSelector(selectIsLoggedIn);
  const isAdmin = useAppSelector(selectIsAdmin);

  const navClass = ({ isActive }: { isActive: boolean }) =>
    `nl${isActive ? ' on' : ''}`;

  return (
    <div className="app">
      <header className="site-header">

        {/* פס הודעות נע */}
        <div className="ticker" aria-label="הודעות">
          <div className="ticker-track">
            <span>
              ⚠ בזמן אזעקה: היכנסו למרחב המוגן הקרוב ביותר
              • התרחקו מחלונות
              • המתינו 10 דקות לפחות
              •
            </span>
            <span>
              ⚠ בזמן אזעקה: היכנסו למרחב המוגן הקרוב ביותר
              • התרחקו מחלונות
              • המתינו 10 דקות לפחות
              •
            </span>
          </div>
        </div>

        {/* סרגל עליון */}
        <div className="topbar">
          <div className="topbar-in">

            {/* לוגו */}
            <Link to="/" className="logo" aria-label="SAFEZONE — לעמוד הבית">
              <svg
                className="logo-mark"
                viewBox="0 0 48 52"
                aria-hidden="true"
              >
                <g
                  stroke="#263d49"
                  strokeLinejoin="round"
                  strokeLinecap="round"
                >
                  <path
                    d="M24 3.5 L42 9.5 V24 C42 36 34 44 24 49 C14 44 6 36 6 24 V9.5 Z"
                    fill="#3d6b7a"
                    strokeWidth="3"
                  />
                  <path
                    d="M24 9 L36.5 13.5 V24 C36.5 32.5 31 39 24 43"
                    fill="none"
                    stroke="#ffffff90"
                    strokeWidth="1.6"
                    strokeDasharray="3 3"
                  />
                  <path
                    d="M24 15.5 L34 24 H30.6 V33.5 H17.4 V24 H14 Z"
                    fill="#ffffff"
                    strokeWidth="2.2"
                  />
                  <path
                    d="M21.6 33.5 V27.5 H26.4 V33.5"
                    fill="#eab04b"
                    strokeWidth="1.8"
                  />
                </g>
              </svg>

              <span className="logo-text">
                SAFE<b>ZONE</b>
                <small>מוצאים מרחב בטוח</small>
              </span>
            </Link>

            {/* כפתורי ניווט */}
            <nav className="navlinks" aria-label="ניווט ראשי">
              <NavLink to="/" end className={navClass}>
                <Icon name="home" />
                <span className="tx">בית</span>
              </NavLink>

              <NavLink to="/search" className={navClass}>
                <Icon name="pin" />
                <span className="tx">חיפוש מקלט</span>
              </NavLink>

              <NavLink to="/share" className={navClass}>
                <Icon name="heart" />
                <span className="tx">שיתוף מקלט</span>
              </NavLink>

              {loggedIn ? (
                <>
                  {isAdmin && (
                    <NavLink to="/admin" className={navClass}>
                      <Icon name="shield" />
                      <span className="tx">ניהול</span>
                    </NavLink>
                  )}

                  <NavLink to="/profile" className={navClass}>
                    <Icon name="user" />
                    <span className="tx">הפרופיל שלי</span>
                  </NavLink>

                  <button
                    className="nl icon-nav"
                    onClick={() => dispatch(logout())}
                    aria-label="התנתקות"
                    title="התנתקות"
                  >
                    <Icon name="logout" />
                  </button>
                </>
              ) : (
                <NavLink
                  to="/login"
                  className="btn small primary nav-auth"
                >
                  <span className="sh">
                    <Icon name="user" />
                  </span>
                  התחברות / הרשמה
                </NavLink>
              )}
            </nav>
          </div>
        </div>
      </header>

      <main className="page">
        <Outlet />
      </main>
    </div>
  );
}

export default Layout;