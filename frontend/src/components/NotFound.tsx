import { Link } from 'react-router-dom';
import Icon from './Icon';

function NotFound() {
  return (
    <section className="screen active">
      <div className="wrap inner">
        <div className="card empty">
          <span className="bubble"><Icon name="alert" /></span>
          <h2 style={{ fontSize: 28 }}>404 - הדף לא נמצא</h2>
          <p>הכתובת שהקלדת לא קיימת באתר.</p>
          <div className="actions" style={{ justifyContent: 'center', marginTop: 20 }}>
            <Link to="/" className="btn primary">
              <span className="sh"><Icon name="home" /></span>
              חזרה לדף הבית
            </Link>
          </div>
        </div>
      </div>
    </section>
  );
}

export default NotFound;
