import { Link } from 'react-router-dom';
import Icon from './Icon';

function NoSheltersScreen() {
  return (
    <section className="screen active">
      <div className="wrap inner">
        <div className="card empty">
          <span className="bubble"><Icon name="alert" /></span>
          <h3 style={{ fontSize: 24 }}>לא נמצא מקלט מתאים</h3>
          <p>לא הספקת להגיע לאף מקלט בזמן שנשאר. הנחיות חירום רשמיות:</p>
          <ul>
            <li>היכנסו למרחב המוגן ביותר בקרבתכם, כמו חדר מדרגות פנימי.</li>
            <li>שבו על הרצפה מתחת לקו החלונות, בצמוד לקיר פנימי.</li>
            <li>המתינו 10 דקות לפחות מרגע הישמע האזעקה.</li>
          </ul>
          <div className="actions" style={{ justifyContent: 'center', marginTop: 20 }}>
            <Link to="/search" className="btn primary">
              <span className="sh"><Icon name="pin" /></span>
              חזרה לחיפוש
            </Link>
          </div>
        </div>
      </div>
    </section>
  );
}

export default NoSheltersScreen;
