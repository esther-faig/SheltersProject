import { useEffect } from 'react';
import { Link } from 'react-router-dom';
import Icon, { type IconName } from './Icon';

interface AdminTile {
  to: string;
  icon: IconName;
  title: string;
  text: string;
}

// סדר הכרטיסים: 1 מימין, 3 משמאל (האתר מימין-לשמאל)
const TILES: AdminTile[] = [
  { to: '/admin/shelters', icon: 'shieldcheck', title: 'ניהול מקלטים', text: 'אישור ודחייה של מקלטים שממתינים לבדיקה.' },
  { to: '/admin/users', icon: 'users', title: 'ניהול משתמשים', text: 'צפייה במשתמשים הרשומים ובמקלטים ששיתפו.' },
  { to: '/admin/preferences', icon: 'sliders', title: 'ניהול העדפות', text: 'הוספה, עריכה ומחיקה של ההעדפות במערכת.' },
];

function AdminHubScreen() {
  useEffect(() => {
    document.title = 'ניהול — SAFEZONE';
  }, []);

  return (
    <section className="screen active">
      <div className="wrap block">
        <div className="heading">
          <span className="kicker"><Icon name="shieldcheck" /> אזור ניהול</span>
          <h2>מה רוצים לנהל?</h2>
          <p>בוחרים אחד מהאזורים כדי להתחיל.</p>
        </div>

        <div className="steps admin-tiles">
          {TILES.map((tile, i) => (
            <Link to={tile.to} className="card step admin-tile" key={tile.to}>
              <span className="num">{i + 1}</span>
              <span className="bubble"><Icon name={tile.icon} /></span>
              <h3>{tile.title}</h3>
              <p>{tile.text}</p>
            </Link>
          ))}
        </div>
      </div>
    </section>
  );
}

export default AdminHubScreen;