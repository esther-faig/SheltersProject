import { Link } from 'react-router-dom';

function NotFound() {
  return (
    <div className="screen alert-box">
      <h2>404 - הדף לא נמצא</h2>
      <p>הכתובת שהקלדת לא קיימת באתר.</p>
      <Link to="/" className="btn">חזרה לדף הבית</Link>
    </div>
  );
}

export default NotFound;