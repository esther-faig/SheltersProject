import { useEffect } from 'react';
import { Link } from 'react-router-dom';
import Icon from './Icon';

function HomePage() {
  useEffect(() => {
    document.title = 'SAFEZONE — מוצאים מרחב בטוח';
  }, []);

  return (
    <section className="screen active">
      {/* אזור פתיחה */}
      <div className="hero">
        <div className="wrap hero-in">
          <div className="hero-copy">
            <div className="eyebrow">
              <Icon name="shieldcheck" /> מוכנים יותר. בטוחים יותר.
            </div>

            <h1>
              בזמן אמת,
              <br />
              <em>מוצאים מקום בטוח.</em>
            </h1>

            <p>
              מקלטים, ממ״דים ומרחבים מוגנים ברחבי הארץ — מוצאים את הקרוב
              והמתאים לך, לפי המיקום, דרך ההגעה והדברים שחשובים לך.
            </p>

            <div className="actions">
              <Link to="/register" className="btn">
                <span className="sh"><Icon name="user" /></span>
                התחברות / הרשמה
              </Link>

              <Link to="/share" className="btn soft">
                <span className="sh"><Icon name="home" /></span>
                שיתוף מקלט פרטי
              </Link>
            </div>

            <div className="trust">
              <Icon name="check" /> חיפוש פשוט, ברור ונגיש לכולם — גם בלי להירשם
            </div>
          </div>

          {/* הכפתור השלישי הקיים בפרויקט: חיפוש מקלט */}
          <div className="hero-beacon">
            <Link to="/search" className="beacon" aria-label="מציאת מקלט עכשיו">
              <span className="sh"><Icon name="pin" /></span>
              מציאת מקלט<br />עכשיו
            </Link>
          </div>
        </div>
      </div>

      {/* שלושת כרטיסי היתרונות */}
      <div className="wrap">
        <div className="strip">
          <div className="card">
            <span className="bubble"><Icon name="pin" /></span>
            <div><strong>קרוב אליך</strong><span className="t">לפי המיקום שלך</span></div>
          </div>
          <div className="card">
            <span className="bubble"><Icon name="sliders" /></span>
            <div><strong>מתאים לך</strong><span className="t">לפי ההעדפות שלך</span></div>
          </div>
          <div className="card">
            <span className="bubble"><Icon name="heart" /></span>
            <div><strong>ביחד</strong><span className="t">קהילה שמשתפת ועוזרת</span></div>
          </div>
        </div>
      </div>

      {/* שלושת שלבי השימוש */}
      <section className="block wrap" id="how">
        <div className="heading">
          <span className="kicker"><Icon name="pin" /> מתחילים מכאן</span>
          <h2>שלושה צעדים ויש לך מקום בטוח</h2>
          <p>בלי הרשמה, בלי סיבוכים.</p>
        </div>
        <div className="steps">
          <div className="card step">
            <span className="num">1</span>
            <span className="bubble"><Icon name="locate" /></span>
            <h3>משתפים מיקום</h3>
            <p>לוחצים פעם אחת, או מקלידים כתובת.</p>
          </div>
          <div className="card step">
            <span className="num">2</span>
            <span className="bubble"><Icon name="walk" /></span>
            <h3>בוחרים איך מגיעים</h3>
            <p>בהליכה או ברכב — הזמנים מחושבים בהתאם.</p>
          </div>
          <div className="card step">
            <span className="num">3</span>
            <span className="bubble"><Icon name="shieldcheck" /></span>
            <h3>מקבלים מקלט מומלץ</h3>
            <p>המקלט הטוב ביותר עבורך, ועוד אפשרויות.</p>
          </div>
        </div>
      </section>

      {/* יתרונות החיפוש */}
      <section className="block wrap" id="features" style={{ paddingTop: 70 }}>
        <div className="heading">
          <span className="kicker"><Icon name="sliders" /> החיפוש שמתחשב בך</span>
          <h2>לא רק קרוב. גם מתאים.</h2>
          <p>המרחב הנכון הוא זה שאפשר להגיע אליו בזמן ושמתאים לצרכים שלך.</p>
        </div>
        <div className="grid3">
          <article className="card feat">
            <span className="bubble"><Icon name="wheelchair" /></span>
            <h3>נגישות חשובה</h3>
            <p>אפשר להתחשב בכניסה ללא מדרגות ובגישה נוחה לכיסא גלגלים.</p>
          </article>
          <article className="card feat">
            <span className="bubble"><Icon name="wind" /></span>
            <h3>מרחב ואוורור</h3>
            <p>אפשר להעדיף מקום מרווח, מאוורר ונעים יותר לשהייה.</p>
          </article>
          <article className="card feat">
            <span className="bubble"><Icon name="heart" /></span>
            <h3>עזרה קהילתית</h3>
            <p>אנשים יכולים לשתף מרחבים פרטיים כדי לעזור לאחרים בסביבה.</p>
          </article>
        </div>
      </section>

      {/* אזור שיתוף תחתון — גרפיקה בלבד, לא כפתור */}
      <section className="wrap" id="share">
        <div className="card share-band">
          <span className="bubble" style={{ width: 64, height: 64, background: '#fff' }}>
            <Icon name="home" style={{ width: 32, height: 32 }} />
          </span>
          <div>
            <span className="kicker">הופכים את הקהילה לחזקה יותר</span>
            <h2>יש לך ממ״ד או מקלט פרטי?</h2>
            <p>אפשר להציע אותו לשיתוף ולעזור למי שנמצא בסביבה שלך.</p>
          </div>

          {/* אלמנט גרפי בלבד — בכוונה ללא button וללא Link */}
          <div className="btn primary static" aria-hidden="true">
            <span className="sh"><Icon name="heart" /></span>
            שיתוף מקלט פרטי
          </div>
        </div>
      </section>
    </section>
  );
}

export default HomePage;
