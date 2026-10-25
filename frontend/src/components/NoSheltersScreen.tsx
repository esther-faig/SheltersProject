import React from 'react';

function NoSheltersScreen() {
    return (
        <div style={{ textAlign: 'center', marginTop: '30px' }}>
            <h2>לא נמצא מקלט מתאים</h2>
            <p>לצערנו, לא מצאנו מקלט בקרבת מקום שעונה על ההגדרות שבחרת.</p>
            
            <div style={{ backgroundColor: '#ffe6e6', border: '1px solid red', padding: '15px', marginTop: '20px', textAlign: 'right' }}>
                <h3>הנחיות חירום רשמיות:</h3>
                <ul>
                    <li>היכנסו למרחב המוגן ביותר הקיים בקרבתכם (כגון חדר מדרגות פנימי).</li>
                    <li>שבו על הרצפה מתחת לקו החלונות, בצמוד לקיר פנימי.</li>
                    <li>המתינו 10 דקות לפחות מרגע הישמע האזעקה.</li>
                </ul>
            </div>
            
            <br />
            <button>חזרה לחיפוש</button>
        </div>
    );
}

export default NoSheltersScreen;