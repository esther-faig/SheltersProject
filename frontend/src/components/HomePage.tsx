import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

const fullText = 'המקלט הקרוב אליך כבר מחכה';

function HomePage() {
  const [count, setCount] = useState(0);

  useEffect(() => {
    document.title = 'SafeZone | בית';
  }, []);

  useEffect(() => {
    if (count >= fullText.length) return;
    const id = setTimeout(() => setCount(count + 1), 70);
    return () => clearTimeout(id);
  }, [count]);

  return (
    <div className="screen hero">
      <div className="hero-text">
        <h1 className="big-title">
          {fullText.slice(0, count)}
          <span className="cursor">|</span>
        </h1>
        <p className="lead">
          מקלטים, ממ"דים ומרחבים מוגנים ברחבי הארץ, עם התאמה אישית לצרכים שלך.
        </p>
        <div className="btn-stack">
          <Link to="/search" className="btn signal big">מציאת מקלט עכשיו 📍</Link>
          <Link to="/register" className="btn">התחברות / הרשמה 👤</Link>
          <Link to="/share" className="btn teal">שיתוף מקלט פרטי 🏠</Link>
        </div>
      </div>
    </div>
  );
}

export default HomePage;