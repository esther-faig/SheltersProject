
import { useEffect, useState } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { shelters } from '../shelters';
import Icon from './Icon';
import ShelterMap, { pinAt } from './ShelterMap';

function ShelterDetailsScreen() {
  const { id } = useParams();
  const navigate = useNavigate();
  const location = useLocation();

  const [navigationMessage, setNavigationMessage] = useState(false);

  const shelter = shelters.find((item) => item.id === Number(id));
  const mode =
    (location.state as { mode?: string } | null)?.mode === 'car'
      ? 'car'
      : 'walk';

  useEffect(() => {
    document.title = shelter ? shelter.name : 'מקלט לא נמצא';
  }, [shelter]);

  if (!shelter) {
    return (
      <section className="screen active">
        <div className="wrap inner">
          <div className="card empty">
            <h2 style={{ fontSize: 26 }}>המקלט לא נמצא</h2>
            <p>לא הצלחנו למצוא את פרטי המקלט המבוקש.</p>
            <div className="actions" style={{ justifyContent: 'center', marginTop: 16 }}>
              <button
                className="btn primary"
                onClick={() => navigate(-1)}
                type="button"
              >
                <span className="sh"><Icon name="arrow-r" /></span>
                חזרה
              </button>
            </div>
          </div>
        </div>
      </section>
    );
  }

  const minutes = mode === 'car' ? shelter.carMin : shelter.walkMin;

  const directions = [
    'צאו מהמיקום שלכם ופנו לכיוון המקלט.',
    `המשיכו בדרך כ־${Math.round(shelter.distance * 0.6)} מטר.`,
    `המשיכו עוד כ־${Math.round(shelter.distance * 0.4)} מטר.`,
    'המקלט נמצא ביעד המסומן במפה.',
  ];

  const pin = pinAt(shelters.findIndex((item) => item.id === shelter.id));
  const route = `M300 170 L${pin.x} 170 L${pin.x} ${pin.y}`;

  return (
    <section className="screen active">
      <div className="wrap inner">
        <div className="phead">
          <button
            className="iconbtn"
            type="button"
            onClick={() => navigate(-1)}
            aria-label="חזרה"
          >
            <Icon name="arrow-r" />
          </button>
          <div>
            <h2>{shelter.name}</h2>
            <div className="sub">פרטי המקלט והוראות הגעה</div>
          </div>
        </div>

        <div className="cols">
          <div className="stack">
            <div className="card">
              <div className="meta" style={{ fontSize: 15 }}>
                <span><Icon name="pin" />{shelter.distance} מטר</span>
                <span><Icon name="walk" />הליכה {shelter.walkMin} דקות</span>
                <span><Icon name="car" />רכב {shelter.carMin} דקות</span>
              </div>

              <span className="lbl">מאפיינים</span>
              <div className="chips">
                {shelter.features.map((feature) => (
                  <span className="chip ok" key={feature}>
                    <Icon name="check" />
                    {feature}
                  </span>
                ))}
              </div>
            </div>

            <div className="card">
              <span className="lbl">הוראות הגעה</span>

              <ol className="step-list">
                {directions.map((direction, index) => (
                  <li key={index}>{direction}</li>
                ))}
              </ol>

              <div className="hint">
                הוראות לדוגמה בלבד — בהמשך יתחבר שירות ניווט אמיתי.
              </div>
            </div>

            <button
              className="btn primary block"
              type="button"
              onClick={() => setNavigationMessage(true)}
            >
              <span className="sh"><Icon name="nav" /></span>
              התחל ניווט
            </button>

            {navigationMessage && (
              <div className="info green" role="status">
                <Icon name="shieldcheck" />
                הניווט יתחבר בהמשך — זוהי הדגמה בלבד.
              </div>
            )}

            <div className="hint" style={{ textAlign: 'center' }}>
              זמן ההגעה המוצג לפי הבחירה הקודמת:{' '}
              <strong>{mode === 'car' ? 'רכב' : 'הליכה'}</strong>
              {' · '}
              <strong>{minutes} דקות</strong>
            </div>
          </div>

          <ShelterMap
            user
            pins={[{ id: shelter.id, ...pin }]}
            route={route}
            hl={shelter.id}
            caption={
              <>
                <span className="lg"><i style={{ background: '#3d6b7a' }} />המיקום שלך</span>
                <span className="lg"><i style={{ background: '#b75c63' }} />המקלט</span>
                {' · '}מסלול הגעה להמחשה בלבד
              </>
            }
          />
        </div>
      </div>
    </section>
  );
}

export default ShelterDetailsScreen;
