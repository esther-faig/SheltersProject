
import { useEffect } from 'react';
import { Link } from 'react-router-dom';

function Icon({ name }: { name: string }) {
  const paths: Record<string, React.ReactNode> = {
    shield: <>
      <path d="M12 2.8 L19.6 5.8 V11.4 C19.6 16 16.6 19.6 12 21.2 C7.4 19.6 4.4 16 4.4 11.4 V5.8 Z" />
      <path d="M8.6 12.2 l2.5 2.6 l4.4 -5" />
    </>,
    pin: <>
      <path d="M12 21.2 C8 16.6 5.4 13.4 5.4 10 a6.6 6.6 0 0 1 13.2 0 c0 3.4 -2.6 6.6 -6.6 11.2 Z" />
      <circle cx="12" cy="10" r="2.4" />
    </>,
    user: <>
      <circle cx="12" cy="8" r="3.6" />
      <path d="M5 20.4 c.5 -4 3.5 -6.2 7 -6.2 s6.5 2.2 7 6.2" />
    </>,
    home: <>
      <path d="M3.4 11.2 L12 4 l8.6 7.2 M6 9.6 V20 h12 V9.6 M10 20 v-5.6 h4 V20" />
    </>,
    sliders: <>
      <path d="M4 7 h8.4 M17.6 7 H20 M4 17 h2.4 M11.6 17 H20" />
      <circle cx="15" cy="7" r="2.4" />
      <circle cx="9" cy="17" r="2.4" />
    </>,
    heart: <>
      <path d="M12 20.2 C5 14.6 3.4 11.2 3.4 8.6 a4.3 4.3 0 0 1 8.6 -1.2 a4.3 4.3 0 0 1 8.6 1.2 C20.6 14.6 19 14.6 12 20.2 Z" />
    </>,
    locate: <>
      <circle cx="12" cy="12" r="6" />
      <circle cx="12" cy="12" r="1.6" />
      <path d="M12 2.4 v3.2 M12 18.4 v3.2 M2.4 12 h3.2 M18.4 12 h3.2" />
    </>,
    walk: <>
      <circle cx="13.2" cy="4.4" r="1.9" />
      <path d="M12 8.6 l-2.6 4 l2.6 2.2 v5.6 M12 8.6 l3 3 l2.6 .4 M9.4 12.6 l-2 3.2 M12 14.8 l3 2 l.6 3.6" />
    </>,
    wheelchair: <>
      <circle cx="10" cy="4.4" r="1.8" />
      <path d="M10 7.6 v6 h5.2 l2.6 5.6 M10 11 h4.6 M7.4 11.6 a5 5 0 1 0 7.8 6.6" />
    </>,
    wind: <>
      <path d="M3 9 h10.4 a2.6 2.6 0 1 0 -2.6 -2.6 M3 13 h15 a2.6 2.6 0 1 1 -2.6 2.6 M3 17 h6.6" />
    </>,
    check: <path d="M5 12.6 l4.6 4.6 l9.6 -10.4" />
  };

  return (
    <svg className="ic" viewBox="0 0 24 24" aria-hidden="true">
      {paths[name]}
    </svg>
  );
}

function CityDecoration() {
  return (
    <div className="home-city" aria-hidden="true">
      <div className="home-sky home-sky-back">
        {Array.from({ length: 34 }, (_, i) => <i key={i} />)}
      </div>
      <div className="home-sky home-sky-front">
        {Array.from({ length: 34 }, (_, i) => <i key={i} />)}
      </div>
      <div className="home-haze" />
    </div>
  );
}

function HomePage() {
  useEffect(() => {
    document.title = 'SAFEZONE — מוצאים מרחב בטוח';
  }, []);

  return (
    <div className="home-page">
      <CityDecoration />

      {/* אזור פתיחה */}
      <section className="home-hero">
        <div className="home-wrap home-hero-inner">
          <div className="home-copy">
            <div className="home-eyebrow">
              <Icon name="shield" />
              מוכנים יותר. בטוחים יותר.
            </div>

            <h1 className="home-main-title">
              בזמן אמת,
              <br />
              <em>מוצאים מקום בטוח.</em>
            </h1>

            <p className="home-description">
              מקלטים, ממ״דים ומרחבים מוגנים ברחבי הארץ — מוצאים את הקרוב
              והמתאים לך, לפי המיקום, דרך ההגעה והדברים שחשובים לך.
            </p>

            <div className="home-actions">
              <Link to="/register" className="home-action">
                <span className="home-action-icon">
                  <Icon name="user" />
                </span>
                התחברות / הרשמה
              </Link>

              <Link to="/share" className="home-action home-action-soft">
                <span className="home-action-icon">
                  <Icon name="home" />
                </span>
                שיתוף מקלט פרטי
              </Link>
            </div>

            <div className="home-trust">
              <Icon name="check" />
              חיפוש פשוט, ברור ונגיש לכולם — גם בלי להירשם
            </div>
          </div>

          {/* הכפתור השלישי הקיים בפרויקט: חיפוש מקלט */}
          <div className="home-beacon-area">
            <Link
              to="/search"
              className="home-beacon"
              aria-label="מציאת מקלט עכשיו"
            >
              <span className="home-beacon-icon">
                <Icon name="pin" />
              </span>
              <span>מציאת מקלט</span>
              <span>עכשיו</span>
            </Link>
          </div>
        </div>
      </section>

      {/* שלושת כרטיסי היתרונות */}
      <div className="home-wrap">
        <div className="home-strip">
          <article className="home-strip-card">
            <span className="home-bubble">
              <Icon name="pin" />
            </span>
            <div>
              <strong>קרוב אליך</strong>
              <span>לפי המיקום שלך</span>
            </div>
          </article>

          <article className="home-strip-card">
            <span className="home-bubble">
              <Icon name="sliders" />
            </span>
            <div>
              <strong>מתאים לך</strong>
              <span>לפי ההעדפות שלך</span>
            </div>
          </article>

          <article className="home-strip-card">
            <span className="home-bubble">
              <Icon name="heart" />
            </span>
            <div>
              <strong>ביחד</strong>
              <span>קהילה שמשתפת ועוזרת</span>
            </div>
          </article>
        </div>
      </div>

      {/* שלושת שלבי השימוש */}
      <section className="home-section home-wrap">
        <div className="home-heading">
          <span className="home-kicker">
            <Icon name="pin" />
            מתחילים מכאן
          </span>
          <h2>שלושה צעדים ויש לך מקום בטוח</h2>
          <p>בלי הרשמה, בלי סיבוכים.</p>
        </div>

        <div className="home-steps">
          <article className="home-card home-step">
            <span className="home-step-number">1</span>
            <span className="home-bubble home-bubble-large">
              <Icon name="locate" />
            </span>
            <h3>משתפים מיקום</h3>
            <p>לוחצים פעם אחת, או מקלידים כתובת.</p>
          </article>

          <article className="home-card home-step">
            <span className="home-step-number">2</span>
            <span className="home-bubble home-bubble-large">
              <Icon name="walk" />
            </span>
            <h3>בוחרים איך מגיעים</h3>
            <p>בהליכה או ברכב — הזמנים מחושבים בהתאם.</p>
          </article>

          <article className="home-card home-step">
            <span className="home-step-number">3</span>
            <span className="home-bubble home-bubble-large">
              <Icon name="shield" />
            </span>
            <h3>מקבלים מקלט מומלץ</h3>
            <p>המקלט הטוב ביותר עבורך, ועוד אפשרויות.</p>
          </article>
        </div>
      </section>

      {/* יתרונות החיפוש */}
      <section className="home-section home-wrap home-features-section">
        <div className="home-heading">
          <span className="home-kicker">
            <Icon name="sliders" />
            החיפוש שמתחשב בך
          </span>
          <h2>לא רק קרוב. גם מתאים.</h2>
          <p>
            המרחב הנכון הוא זה שאפשר להגיע אליו בזמן ושמתאים לצרכים שלך.
          </p>
        </div>

        <div className="home-features">
          <article className="home-card home-feature">
            <span className="home-bubble">
              <Icon name="wheelchair" />
            </span>
            <h3>נגישות חשובה</h3>
            <p>
              אפשר להתחשב בכניסה ללא מדרגות ובגישה נוחה לכיסא גלגלים.
            </p>
          </article>

          <article className="home-card home-feature">
            <span className="home-bubble">
              <Icon name="wind" />
            </span>
            <h3>מרחב ואוורור</h3>
            <p>
              אפשר להעדיף מקום מרווח, מאוורר ונעים יותר לשהייה.
            </p>
          </article>

          <article className="home-card home-feature">
            <span className="home-bubble">
              <Icon name="heart" />
            </span>
            <h3>עזרה קהילתית</h3>
            <p>
              אנשים יכולים לשתף מרחבים פרטיים כדי לעזור לאחרים בסביבה.
            </p>
          </article>
        </div>
      </section>

      {/* אזור שיתוף תחתון — גרפיקה בלבד, לא כפתור */}
      <section className="home-wrap home-share-section">
        <div className="home-card home-share-band">
          <span className="home-bubble home-share-bubble">
            <Icon name="home" />
          </span>

          <div className="home-share-copy">
            <span className="home-kicker">
              הופכים את הקהילה לחזקה יותר
            </span>
            <h2>יש לך ממ״ד או מקלט פרטי?</h2>
            <p>
              אפשר להציע אותו לשיתוף ולעזור למי שנמצא בסביבה שלך.
            </p>
          </div>

          {/* אלמנט גרפי בלבד — בכוונה ללא button וללא Link */}
          <div className="home-share-decoration" aria-hidden="true">
            <span className="home-action-icon">
              <Icon name="heart" />
            </span>
            שיתוף מקלט פרטי
          </div>
        </div>
      </section>
    </div>
  );
}

export default HomePage;
