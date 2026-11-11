
import { useEffect, useState } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { shelters } from '../shelters';

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
      <main className="screen details-page" dir="rtl">
        <div className="details-inner">
          <div className="details-not-found">
            <h2>המקלט לא נמצא</h2>
            <p>לא הצלחנו למצוא את פרטי המקלט המבוקש.</p>
            <button
              className="details-primary-button"
              onClick={() => navigate(-1)}
              type="button"
            >
              חזרה
            </button>
          </div>
        </div>
      </main>
    );
  }

  const minutes = mode === 'car' ? shelter.carMin : shelter.walkMin;

  const directions = [
    'צאו מהמיקום שלכם ופנו לכיוון המקלט.',
    `המשיכו בדרך כ־${Math.round(shelter.distance * 0.6)} מטר.`,
    `המשיכו עוד כ־${Math.round(shelter.distance * 0.4)} מטר.`,
    'המקלט נמצא ביעד המסומן במפה.',
  ];

  return (
    <main className="screen details-page" dir="rtl">
      <div className="details-inner">
        <header className="details-page-head">
          <button
            className="details-back-button"
            type="button"
            onClick={() => navigate(-1)}
            aria-label="חזרה"
          >
            <svg viewBox="0 0 24 24" aria-hidden="true">
              <path d="m15 18-6-6 6-6" />
            </svg>
          </button>

          <div>
            <h2>{shelter.name}</h2>
            <p>פרטי המקלט והוראות הגעה</p>
          </div>
        </header>

        <div className="details-columns">
          <div className="details-content">
            <section className="details-card">
              <div className="details-stats">
                <div className="details-stat">
                  <span className="details-stat-icon">
                    <svg viewBox="0 0 24 24" aria-hidden="true">
                      <path d="M12 21s7-4.5 7-11a7 7 0 1 0-14 0c0 6.5 7 11 7 11Z" />
                      <circle cx="12" cy="10" r="2.5" />
                    </svg>
                  </span>
                  <strong>{shelter.distance} מטר</strong>
                  <small>מרחק</small>
                </div>

                <div className="details-stat">
                  <span className="details-stat-icon">
                    <svg viewBox="0 0 24 24" aria-hidden="true">
                      <circle cx="12" cy="12" r="9" />
                      <path d="M12 7v5l3 2" />
                    </svg>
                  </span>
                  <strong>{shelter.walkMin} דקות</strong>
                  <small>הליכה</small>
                </div>

                <div className="details-stat">
                  <span className="details-stat-icon">
                    <svg viewBox="0 0 24 24" aria-hidden="true">
                      <path d="M5 11l1.5-5h11L19 11l2 2v6h-3v-2H6v2H3v-6l2-2Z" />
                      <path d="M6 11h12M7 14h.01M17 14h.01" />
                    </svg>
                  </span>
                  <strong>{shelter.carMin} דקות</strong>
                  <small>רכב</small>
                </div>
              </div>

              <div className="details-divider" />

              <span className="details-label">מאפיינים</span>

              <div className="details-features">
                {shelter.features.map((feature) => (
                  <span className="details-feature" key={feature}>
                    <svg viewBox="0 0 24 24" aria-hidden="true">
                      <path d="m5 12 4 4L19 6" />
                    </svg>
                    {feature}
                  </span>
                ))}
              </div>
            </section>

            <section className="details-card">
              <span className="details-label">הוראות הגעה</span>

              <ol className="details-step-list">
                {directions.map((direction, index) => (
                  <li key={index}>
                    <span className="details-step-number">{index + 1}</span>
                    <span>{direction}</span>
                  </li>
                ))}
              </ol>

              <div className="details-hint">
                <svg viewBox="0 0 24 24" aria-hidden="true">
                  <circle cx="12" cy="12" r="9" />
                  <path d="M12 11v5M12 7h.01" />
                </svg>
                הוראות לדוגמה בלבד — בהמשך יתחבר שירות ניווט אמיתי.
              </div>
            </section>

            <button
              className="details-navigation-button"
              type="button"
              onClick={() => setNavigationMessage(true)}
            >
              <svg viewBox="0 0 24 24" aria-hidden="true">
                <path d="m3 11 18-8-8 18-2-8-8-2Z" />
                <path d="m11 13 5-5" />
              </svg>
              התחל ניווט
            </button>

            {navigationMessage && (
              <p className="details-navigation-message" role="status">
                הניווט יתחבר בהמשך — זוהי הדגמה בלבד.
              </p>
            )}

            <div className="details-mode-note">
              זמן ההגעה המוצג לפי הבחירה הקודמת:{' '}
              <strong>{mode === 'car' ? 'רכב' : 'הליכה'}</strong>
              {' · '}
              <strong>{minutes} דקות</strong>
            </div>
          </div>

          <aside className="details-map-column">
            <div className="details-map">
              <svg
                viewBox="0 0 600 420"
                className="details-map-drawing"
                role="img"
                aria-label="מפה סכמטית עם מסלול למקלט"
              >
                <rect width="600" height="420" fill="#eef3f4" />

                {[
                  [20, 20, 90, 55],
                  [150, 20, 130, 55],
                  [320, 20, 120, 55],
                  [480, 20, 100, 55],
                  [20, 115, 90, 65],
                  [150, 115, 130, 65],
                  [320, 115, 120, 65],
                  [480, 115, 100, 65],
                  [20, 220, 90, 65],
                  [150, 220, 130, 65],
                  [320, 220, 120, 65],
                  [480, 220, 100, 65],
                  [20, 330, 90, 65],
                  [150, 330, 130, 65],
                  [320, 330, 120, 65],
                  [480, 330, 100, 65],
                ].map(([x, y, width, height], index) => (
                  <rect
                    key={index}
                    x={x}
                    y={y}
                    width={width}
                    height={height}
                    rx="9"
                    fill="#dfe9eb"
                  />
                ))}

                {[
                  'M0 90 H600',
                  'M0 195 H600',
                  'M0 305 H600',
                  'M130 0 V420',
                  'M300 0 V420',
                  'M460 0 V420',
                ].map((street, index) => (
                  <g key={index}>
                    <path
                      d={street}
                      fill="none"
                      stroke="#c3d2d6"
                      strokeWidth="20"
                    />
                    <path
                      d={street}
                      fill="none"
                      stroke="#fff"
                      strokeWidth="14"
                    />
                  </g>
                ))}

                <path
                  d="M300 195 L300 120 L420 120 L420 245"
                  fill="none"
                  stroke="#b75c63"
                  strokeWidth="6"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeDasharray="9 8"
                />

                <g transform="translate(300 195)">
                  <circle r="19" fill="#fff" stroke="#263d49" strokeWidth="2.5" />
                  <circle r="8" fill="#3d6b7a" />
                </g>

                <g transform="translate(420 245)">
                  <path
                    d="M0 20C-4 12-15 4-15-5a15 15 0 1 1 30 0c0 9-11 17-15 25Z"
                    fill="#b75c63"
                    stroke="#263d49"
                    strokeWidth="2"
                  />
                  <circle cy="-5" r="5" fill="#fff" />
                </g>
              </svg>

              <div className="details-map-legend">
                <span>
                  <i className="details-legend-user" />
                  המיקום שלך
                </span>
                <span>
                  <i className="details-legend-shelter" />
                  המקלט
                </span>
              </div>
            </div>

            <div className="details-map-caption">
              מסלול הגעה להמחשה בלבד
            </div>
          </aside>
        </div>
      </div>
    </main>
  );
}

export default ShelterDetailsScreen;
