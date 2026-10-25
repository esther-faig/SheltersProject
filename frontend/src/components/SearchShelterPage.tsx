import React, { useState } from 'react';

function SearchShelterPage() {
    // הגדרת משתנה state לשמירת אופן ההגעה שנבחר (הליכה או רכב)
    const [arrivalMethod, setArrivalMethod] = useState<string>('');

    return (
        <div>
            <h2>חיפוש מקלט</h2>
            
            <button>שתף את המיקום שלי</button>
            
            {/* תיבה אפורה שתשמש כמקום שמור למפה עתידית */}
            <div style={{ width: '100%', height: '200px', backgroundColor: '#e0e0e0', margin: '20px 0', border: '1px solid #ccc' }}>
                <p style={{ textAlign: 'center', paddingTop: '80px' }}>כאן תוצג מפה</p>
            </div>

            <div>
                <h3>אופן ההגעה:</h3>
                <label>
                    <input 
                        type="radio" 
                        name="arrival" 
                        value="walking" 
                        onChange={(e) => setArrivalMethod(e.target.value)} 
                    />
                    הליכה
                </label>
                <br />
                <label>
                    <input 
                        type="radio" 
                        name="arrival" 
                        value="car" 
                        onChange={(e) => setArrivalMethod(e.target.value)} 
                    />
                    רכב
                </label>
            </div>

            <br />
            <button>מצא לי מקלט</button>
        </div>
    );
}

export default SearchShelterPage;