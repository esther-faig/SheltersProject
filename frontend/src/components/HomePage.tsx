import React from 'react';
import '../App.css';

function HomePage() {
    return (
        <div className="home-wrapper">
            {/* תפריט עליון */}
            <header className="home-header">
                <div className="logo-container">
                    <span className="logo-icon">🛡️</span>
                    <span className="logo-text">SafeZone</span>
                </div>
                <div className="nav-links">
                    <span>התחברות | הרשמה</span>
                </div>
            </header>

            {/* תוכן מרכזי */}
            <main className="home-main">
                <h1 className="main-title">
                    מצאי את המרחב המוגן<br />המתאים לך
                </h1>
                <p className="main-subtitle">
                    מקלטים, ממ"דים ומרחבים מוגנים ברחבי הארץ<br />
                    בזמן אמת, עם התאמה אישית לצרכים שלך.
                </p>
                
                <div className="buttons-container">
                    <button className="btn-primary">
                        מציאת מקלט עכשיו <span className="icon">📍</span>
                    </button>
                    <button className="btn-secondary">
                        התחברות / הרשמה <span className="icon">👤</span>
                    </button>
                    <button className="btn-tertiary">
                        שיתוף מקלט פרטי <span className="icon">🏠</span>
                    </button>
                </div>
            </main>

            {/* כותרת תחתונה */}
            <footer className="home-footer">
                יחד מדווחים, יחד בטוחים <span className="heart-icon">💙</span>
            </footer>
        </div>
    );
}

export default HomePage;