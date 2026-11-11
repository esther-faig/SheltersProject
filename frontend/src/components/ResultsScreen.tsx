
import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { shelters } from '../shelters';

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
    <main className="screen results-page" dir="rtl">
      <div className="results-inner">
        <header className="results-page-head">
          <button
            className="results-back-button"
            type="button"
            onClick={() => navigate(-1)}
            aria-label="חזרה"
          >
            <svg viewBox="0 0 24 24" aria-hidden="true">
              <path d="M15 18l-6-6 6-6" />
            </svg>
          </button>

          <div>
            <h2>המקלטים המתאימים לך</h2>
            <p className="results-subtitle">
              {byCar
                ? 'לפי זמן נסיעה מהמיקום שלך'
                : 'לפי מרחק הליכה מהמיקום שלך'}
            </p>
          </div>
        </header>

        <div className="results-columns">
          <section className="results-list">
            {results.length === 0 ? (
              <div className="results-empty-card">
                <span className="results-empty-icon">
                  <svg viewBox="0 0 24 24" aria-hidden="true">
                    <path d="M12 9v4M12 17h.01" />
                    <path d="M10.3 3.9 2.5 17.4A2 2 0 0 0 4.2 20h15.6a2 2 0 0 0 1.7-2.6L13.7 3.9a2 2 0 0 0-3.4 0Z" />
                  </svg>
                </span>

                <h3>לא נמצא מקלט מתאים</h3>

                <p>
                  לא מצאנו מקלט מתאים באזור שלך. אפשר לנסות חיפוש מחדש או
                  לשנות את אופן ההגעה.
                </p>

                <div className="results-emergency-info">
                  <div className="results-info-heading">
                    <svg viewBox="0 0 24 24" aria-hidden="true">
                      <circle cx="12" cy="12" r="9" />
                      <path d="M12 11v5M12 7h.01" />
                    </svg>
                    הנחיות חשובות
                  </div>
                  <p>
                    היכנסו למבנה סגור, התרחקו מחלונות ופעלו לפי הנחיות פיקוד
                    העורף.
                  </p>
                </div>

                <div className="results-emergency-numbers">
                  <div>
                    <strong>100</strong>
                    <span>משטרה</span>
                  </div>
                  <div>
                    <strong>101</strong>
                    <span>מד״א</span>
                  </div>
                  <div>
                    <strong>102</strong>
                    <span>כבאות</span>
                  </div>
                  <div>
                    <strong>104</strong>
                    <span>פיקוד העורף</span>
                  </div>
                </div>

                <div className="results-empty-actions">
                  <Link className="results-secondary-button" to="/search">
                    חזרה לחיפוש
                  </Link>
                  <button
                    className="results-primary-button"
                    type="button"
                    onClick={() => setShowEmptyDemo(false)}
                  >
                    חיפוש מחדש
                  </button>
                </div>
              </div>
            ) : (
              <>
                <ShelterResultCard
                  shelter={results[0]}
                  minutes={getMinutes(results[0])}
                  recommended
                  shelterType={getShelterType(results[0].name)}
                />

                {results.length > 1 && (
                  <>
                    <h3 className="results-more-title">אפשרויות נוספות</h3>

                    {results.slice(1).map((shelter) => (
                      <ShelterResultCard
                        key={shelter.id}
                        shelter={shelter}
                        minutes={getMinutes(shelter)}
                        shelterType={getShelterType(shelter.name)}
                      />
                    ))}
                  </>
                )}
              </>
            )}
          </section>

          <aside className="results-map-column">
            <div className="results-map">
              <ResultsMap shelters={results} />
              <div className="results-map-caption">
                {results.length === 0
                  ? 'אין מקלטים להצגה'
                  : `${results.length} מקלטים מתאימים בסביבה`}
              </div>
            </div>

            <div className="results-demo-note">
              <button
                type="button"
                onClick={() => setShowEmptyDemo((current) => !current)}
              >
                {showEmptyDemo
                  ? 'חזרה להצגת המקלטים'
                  : 'הדגמה: מצב ״לא נמצא מקלט מתאים״'}
              </button>
              <p>הדגמה עיצובית — המפה והמקלטים הם נתוני דוגמה.</p>
            </div>
          </aside>
        </div>
      </div>
    </main>
  );
}

type ShelterResultCardProps = {
  shelter: (typeof shelters)[number];
  minutes: number;
  recommended?: boolean;
  shelterType: string;
};

function ShelterResultCard({
  shelter,
  minutes,
  recommended = false,
  shelterType,
}: ShelterResultCardProps) {
  return (
    <article
      className={
        recommended
          ? 'results-shelter-card results-recommended'
          : 'results-shelter-card'
      }
    >
      {recommended && (
        <div className="results-ribbon">
          <svg viewBox="0 0 24 24" aria-hidden="true">
            <path d="m12 3 2.7 5.5 6.1.9-4.4 4.3 1 6.1-5.4-2.9-5.4 2.9 1-6.1-4.4-4.3 6.1-.9L12 3Z" />
          </svg>
          המקלט המומלץ עבורך
        </div>
      )}

      <div className="results-shelter-content">
        <div className="results-shelter-illustration">
          <svg viewBox="0 0 80 80" aria-hidden="true">
            <g
              stroke="#263d49"
              strokeWidth="2.4"
              strokeLinejoin="round"
              strokeLinecap="round"
            >
              <rect x="12" y="22" width="56" height="46" rx="3" fill="#b9d0d6" />
              <rect x="12" y="22" width="56" height="9" fill="#3d6b7a" />
              <rect x="31" y="42" width="18" height="26" rx="2" fill="#eab04b" />
              <rect x="18" y="37" width="9" height="9" rx="2" fill="#fff" />
              <rect x="53" y="37" width="9" height="9" rx="2" fill="#fff" />
              <path d="M40 8 52 17H28L40 8Z" fill="#fff" />
              <path d="M40 10v8" />
            </g>
          </svg>
        </div>

        <div className="results-shelter-body">
          <h3>{shelter.name}</h3>

          <div className="results-shelter-meta">
            <span>
              <svg viewBox="0 0 24 24" aria-hidden="true">
                <path d="M20 10c0 5-8 12-8 12S4 15 4 10a8 8 0 1 1 16 0Z" />
                <circle cx="12" cy="10" r="2.5" />
              </svg>
              {shelter.distance} מטר
            </span>

            <span>
              <svg viewBox="0 0 24 24" aria-hidden="true">
                <circle cx="12" cy="12" r="9" />
                <path d="M12 7v5l3 2" />
              </svg>
              {minutes} דק׳ { /* זמן הגעה לפי אופן ההגעה שנבחר */ }
            </span>

            <span>
              <svg viewBox="0 0 24 24" aria-hidden="true">
                <path d="M12 22s8-4 8-11V5l-8-3-8 3v6c0 7 8 11 8 11Z" />
              </svg>
              {shelterType}
            </span>
          </div>

          <div className="results-feature-chips">
            {shelter.features.map((feature) => (
              <span className="results-feature-chip" key={feature}>
                <svg viewBox="0 0 24 24" aria-hidden="true">
                  <path d="m5 12 4 4L19 6" />
                </svg>
                {feature}
              </span>
            ))}
          </div>

          <div className="results-shelter-actions">
            <Link
              className={
                recommended
                  ? 'results-primary-button'
                  : 'results-secondary-button'
              }
              to={`/shelter/${shelter.id}`}
            >
              <svg viewBox="0 0 24 24" aria-hidden="true">
                <path d="M12 21s7-4.5 7-11a7 7 0 1 0-14 0c0 6.5 7 11 7 11Z" />
                <circle cx="12" cy="10" r="2.5" />
              </svg>
              ניווט ופרטים
            </Link>
          </div>
        </div>
      </div>
    </article>
  );
}

type ResultsMapProps = {
  shelters: typeof shelters;
};

function ResultsMap({ shelters: visibleShelters }: ResultsMapProps) {
  const pinPositions = [
    { x: 155, y: 90 },
    { x: 330, y: 145 },
    { x: 445, y: 75 },
    { x: 245, y: 245 },
    { x: 495, y: 245 },
  ];

  return (
    <svg
      className="results-map-drawing"
      viewBox="0 0 600 340"
      role="img"
      aria-label="מפה סכמטית עם מיקום המקלטים"
    >
      <rect width="600" height="340" fill="#eef3f4" />

      {[
        [20, 20, 80, 40],
        [140, 22, 130, 32],
        [310, 18, 140, 40],
        [490, 22, 90, 38],
        [20, 92, 80, 60],
        [140, 92, 130, 60],
        [312, 90, 140, 64],
        [492, 92, 90, 60],
        [22, 190, 76, 56],
        [140, 198, 130, 52],
        [490, 190, 92, 56],
        [22, 284, 80, 40],
        [142, 286, 126, 40],
        [310, 286, 142, 40],
        [492, 286, 88, 40],
      ].map(([x, y, width, height], index) => (
        <rect
          key={index}
          x={x}
          y={y}
          width={width}
          height={height}
          rx="10"
          fill="#dfe9eb"
        />
      ))}

      {[
        'M0 72 H600',
        'M0 172 H600',
        'M0 266 H600',
        'M120 0 V340',
        'M290 0 V340',
        'M470 0 V340',
        'M0 330 L600 20',
      ].map((street, index) => (
        <g key={index}>
          <path
            d={street}
            stroke="#c3d2d6"
            strokeWidth="18"
            fill="none"
            strokeLinecap="round"
          />
          <path
            d={street}
            stroke="#fff"
            strokeWidth="13"
            fill="none"
            strokeLinecap="round"
          />
        </g>
      ))}

      <g transform="translate(300 170)">
        <circle r="20" fill="#fff" stroke="#263d49" strokeWidth="2.5" />
        <circle r="9" fill="none" stroke="#3d6b7a" strokeWidth="3" />
        <circle r="3" fill="#3d6b7a" />
      </g>

      {visibleShelters.map((shelter, index) => {
        const position = pinPositions[index % pinPositions.length];

        return (
          <g key={shelter.id} transform={`translate(${position.x} ${position.y})`}>
            <path
              d="M0 18C-4 11-14 3-14-5a14 14 0 1 1 28 0c0 8-10 16-14 23Z"
              fill={index === 0 ? '#b75c63' : '#3d6b7a'}
              stroke="#263d49"
              strokeWidth="2"
            />
            <circle cy="-5" r="5" fill="#fff" />
            <text
              x="0"
              y="-2"
              textAnchor="middle"
              fontSize="8"
              fontWeight="bold"
              fill="#263d49"
            >
              {index + 1}
            </text>
          </g>
        );
      })}
    </svg>
  );
}

export default ResultsScreen;
