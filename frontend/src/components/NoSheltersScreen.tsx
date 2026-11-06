import { Link } from 'react-router-dom';

function NoSheltersScreen() {
  return (
    <div className="alert-box">
      <h2>לא נמצא מקלט מתאים</h2>
      <p>לא הספקת להגיע לאף מקלט בזמן שנשאר. הנחיות חירום רשמיות:</p>
      <ul>
        <li>היכנסו למרחב המוגן ביותר בקרבתכם, כמו חדר מדרגות פנימי.</li>
        <li>שבו על הרצפה מתחת לקו החלונות, בצמוד לקיר פנימי.</li>
        <li>המתינו 10 דקות לפחות מרגע הישמע האזעקה.</li>
      </ul>
      <Link to="/search" className="btn">חזרה לחיפוש</Link>
    </div>
  );
}

export default NoSheltersScreen;