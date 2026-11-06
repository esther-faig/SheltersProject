import { useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import FeatureTag from './FeatureTag';
import { shelters } from '../shelters';

function ShelterDetailsScreen() {
  const { id } = useParams();
  const navigate = useNavigate();

  // הערך מהכתובת חוזר כמחרוזת, לכן ממירים למספר
  const shelter = shelters.filter((s) => s.id === Number(id))[0];

  useEffect(() => {
    document.title = shelter ? shelter.name : 'מקלט לא נמצא';
  }, [id]);

  if (!shelter) {
    return (
      <div className="screen alert-box">
        <h2>המקלט לא נמצא</h2>
        <button className="btn" onClick={() => navigate(-1)}>חזרה</button>
      </div>
    );
  }

  return (
    <div className="screen">
      <button className="btn small" onClick={() => navigate(-1)}>→ חזרה לתוצאות</button>

      <h2 className="title">{shelter.name}</h2>
      <div className="stats big-stats">
        <span>📏 {shelter.distance} מטר</span>
        <span>🚶 {shelter.walkMin} דקות</span>
        <span>🚗 {shelter.carMin} דקות</span>
      </div>

      <div className="tags">
        {shelter.features.map((f) => (
          <FeatureTag key={f} text={f} />
        ))}
      </div>

      <div className="route-box">
        <div className="route-end">🛡️</div>
        <div className="route-line">
          <div className="walker">🚶</div>
        </div>
        <div className="route-start">📍</div>
      </div>

      <div className="alert-box soft">
        <h3>הוראות הגעה</h3>
        <ol>
          <li>צאו מהבניין ופנו ימינה.</li>
          <li>המשיכו ישר עד לגינה הציבורית.</li>
          <li>המקלט נמצא מצד שמאל.</li>
        </ol>
      </div>
    </div>
  );
}

export default ShelterDetailsScreen;