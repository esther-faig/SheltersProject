
import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { shelters } from '../shelters';
import Icon from './Icon';
import ShelterMap, { pinAt } from './ShelterMap';
import ShelterIllustration from './ShelterIllustration';

function ResultsScreen() {
  const { mode } = useParams();
  const navigate = useNavigate();

  const [showEmptyDemo, setShowEmptyDemo] = useState(false);

  useEffect(() => {
    document.title = 'המקלטים המתאימים לך';
  }, []);

  const byCar = mode === 'car';

  const results = showEmptyDemo
    ? []
    : [...shelters].sort((a, b) => a.distance - b.distance);

  function getMinutes(shelter: (typeof shelters)[number]) {
    return byCar ? shelter.carMin : shelter.walkMin;
  }

  function getShelterType(name: string) {
    if (name.includes('פרטי')) return 'מקלט פרטי';
    if (name.includes('בית ספר')) return 'מקלט מוסדי';
    return 'מקלט ציבורי';
  }

  return (
    <section className="screen active">
      <div className="wrap inner" style={{ maxWidth: 1080 }}>
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
            <h2>המקלטים המתאימים לך</h2>
            <div className="sub">
              {byCar
                ? 'לפי זמן נסיעה מהמיקום שלך'
                : 'לפי מרחק הליכה מהמיקום שלך'}
            </div>
          </div>
        </div>

        <div className="cols" style={{ gridTemplateColumns: '1.25fr 1fr' }} id="resCols">
          <div className="list" id="resList">
            {results.length === 0 ? (
              <div className="card empty">
                <span className="bubble"><Icon name="alert" /></span>
                <h3 style={{ fontSize: 24 }}>לא נמצא מקלט מתאים</h3>
                <p>
                  לא מצאנו מקלט מתאים באזור שלך. אפשר לנסות חיפוש מחדש או
                  לשנות את אופן ההגעה.
                </p>

                <div className="info" style={{ textAlign: 'right', marginTop: 14 }}>
                  <Icon name="info" />
                  <span>
                    <b>הנחיות חשובות</b>
                    <br />
                    היכנסו למבנה סגור, התרחקו מחלונות ופעלו לפי הנחיות פיקוד
                    העורף.
                  </span>
                </div>

                <div className="emerg">
                  <div><b>100</b>משטרה</div>
                  <div><b>101</b>מד״א</div>
                  <div><b>102</b>כבאות</div>
                  <div><b>104</b>פיקוד העורף</div>
                </div>

                <div className="actions" style={{ justifyContent: 'center', marginTop: 20 }}>
                  <Link className="btn" to="/search">
                    <span className="sh"><Icon name="sliders" /></span>
                    חזרה לחיפוש
                  </Link>
                  <button
                    className="btn primary"
                    type="button"
                    onClick={() => setShowEmptyDemo(false)}
                  >
                    <span className="sh"><Icon name="pin" /></span>
                    חיפוש מחדש
                  </button>
                </div>
              </div>
            ) : (
              <>
                <ShelterResultCard
                  shelter={results[0]}
                  minutes={getMinutes(results[0])}
                  byCar={byCar}
                  recommended
                  shelterType={getShelterType(results[0].name)}
                />

                {results.length > 1 && (
                  <>
                    <h3 style={{ fontSize: 20, margin: '6px 4px -6px' }}>
                      אפשרויות נוספות
                    </h3>

                    {results.slice(1).map((shelter) => (
                      <ShelterResultCard
                        key={shelter.id}
                        shelter={shelter}
                        minutes={getMinutes(shelter)}
                        byCar={byCar}
                        shelterType={getShelterType(shelter.name)}
                      />
                    ))}
                  </>
                )}
              </>
            )}
          </div>

          <div className="sticky">
            <ShelterMap
              id="resMap"
              user
              pins={results.map((s, i) => ({ id: s.id, ...pinAt(i) }))}
              hl={results.length ? results[0].id : null}
              caption={
                results.length === 0
                  ? 'אין מקלטים להצגה'
                  : `${results.length} מקלטים מתאימים בסביבה`
              }
            />

            <div className="demo-note">
              <button
                className="nl"
                style={{ fontSize: 12 }}
                type="button"
                onClick={() => setShowEmptyDemo((current) => !current)}
              >
                {showEmptyDemo
                  ? 'חזרה להצגת המקלטים'
                  : 'הדגמה: מצב ״לא נמצא מקלט מתאים״'}
              </button>
              <div>הדגמה עיצובית — המפה והמקלטים הם נתוני דוגמה.</div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

type ShelterResultCardProps = {
  shelter: (typeof shelters)[number];
  minutes: number;
  byCar: boolean;
  recommended?: boolean;
  shelterType: string;
};

function ShelterResultCard({
  shelter,
  minutes,
  byCar,
  recommended = false,
  shelterType,
}: ShelterResultCardProps) {
  return (
    <article className={recommended ? 'card reco' : 'card'}>
      {recommended && (
        <div className="ribbon">
          <Icon name="star" />
          המקלט המומלץ עבורך
        </div>
      )}

      <div className="sh-card">
        <div className="thumb"><ShelterIllustration /></div>

        <div className="sh-body">
          <h3>{shelter.name}</h3>

          <div className="meta">
            <span><Icon name="pin" />{shelter.distance} מטר</span>
            <span>
              <Icon name="clock" />
              {byCar ? 'נסיעה' : 'הליכה'} {minutes} דק׳ { /* זמן הגעה לפי אופן ההגעה שנבחר */ }
            </span>
            <span><Icon name="shield" />{shelterType}</span>
          </div>

          <div className="chips">
            {shelter.features.map((feature) => (
              <span className="chip ok" key={feature}>
                <Icon name="check" />
                {feature}
              </span>
            ))}
          </div>

          <div className="sh-actions">
            <Link
              className={recommended ? 'btn primary small' : 'btn soft small'}
              to={`/shelter/${shelter.id}`}
            >
              <span className="sh"><Icon name="nav" /></span>
              ניווט ופרטים
            </Link>
          </div>
        </div>
      </div>
    </article>
  );
}

export default ResultsScreen;
