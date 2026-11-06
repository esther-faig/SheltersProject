import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import ShelterCard from './ShelterCard';
import NoSheltersScreen from './NoSheltersScreen';
import { shelters } from '../shelters';

const START_SECONDS = 300;

function ResultsScreen() {
  const { mode } = useParams();
  const [seconds, setSeconds] = useState(START_SECONDS);

  useEffect(() => {
    document.title = 'תוצאות חיפוש';
  }, []);

  // ספירה לאחור: לא מפעילים טיימר חדש כש-seconds מגיע ל-0
  useEffect(() => {
    if (seconds <= 0) return;
    const id = setTimeout(() => setSeconds(seconds - 1), 1000);
    return () => clearTimeout(id);
  }, [seconds]);

  const byCar = mode === 'car';

  // מסננים מקלטים שלא מספיקים להגיע אליהם בזמן שנשאר
  const reachable = shelters.filter(
    (s) => (byCar ? s.carMin : s.walkMin) * 60 <= seconds
  );
  const tooFar = shelters.length - reachable.length;

  const mins = Math.floor(seconds / 60);
  const secs = seconds % 60;
  const hurry = seconds < 60;

  return (
    <div className="screen">
      <div className={hurry ? 'timer hurry' : 'timer'}>
        <div>
          <div className="timer-label">הזמן שנשאר להגיע למרחב מוגן</div>
          <div className="timer-num">{mins}:{secs < 10 ? '0' + secs : secs}</div>
        </div>
        <div className="timer-bar">
          <div className="timer-fill" style={{ width: `${(seconds / START_SECONDS) * 100}%` }}></div>
        </div>
        <div className="timer-note">
          {byCar ? '🚗 חישוב לנסיעה ברכב' : '🚶 חישוב להליכה'}
          {tooFar > 0 && ` • ${tooFar} מקלטים רחוקים מדי ולכן הוסתרו`}
        </div>
      </div>

      {reachable.length === 0 ? (
        <NoSheltersScreen />
      ) : (
        <div>
          <h2 className="title">המקלט המומלץ עבורך</h2>
          <ShelterCard
            id={reachable[0].id}
            name={reachable[0].name}
            distance={reachable[0].distance}
            minutes={byCar ? reachable[0].carMin : reachable[0].walkMin}
            features={reachable[0].features}
            isRecommended={true}
            delay={0}
          />

          {reachable.length > 1 && <h2 className="title">אפשרויות נוספות</h2>}
          {reachable.slice(1).map((s, i) => (
            <ShelterCard
              key={s.id}
              id={s.id}
              name={s.name}
              distance={s.distance}
              minutes={byCar ? s.carMin : s.walkMin}
              features={s.features}
              isRecommended={false}
              delay={i + 1}
            />
          ))}
        </div>
      )}
    </div>
  );
}

export default ResultsScreen;