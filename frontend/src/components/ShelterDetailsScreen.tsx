import React from 'react';
// מסך שמתאר את המקלט שהמשתמש בחר ואת הניווט להגיע אליו,
//  כל זה אחרי מסך התוצאות

function ShelterDetailsScreen() {
    // נתונים קבועים של המקלט והוראות ההגעה
    const shelter = {
        name: 'מקלט ציבורי 15 - רחוב הרצל',
        distance: '200 מטר',
        time: '2 דקות',
        features: ['ללא מדרגות', 'מרחב גדול', 'אוורור'],
        instructions: 'צאו מהבניין, פנו ימינה והמשיכו ישר עד לגינה הציבורית. המקלט נמצא מצד שמאל.'
    };

    return (
        <div>
            <h2>פרטי מקלט והוראות הגעה</h2>
            
            <div style={{ border: '1px solid #ccc', padding: '15px', marginBottom: '20px' }}>
                <h3>{shelter.name}</h3>
                <p><strong>מרחק:</strong> {shelter.distance}</p>
                <p><strong>זמן הגעה משוער:</strong> {shelter.time}</p>
                
                <h4>מאפיינים:</h4>
                <ul>
                    {shelter.features.map((feature, index) => (
                        <li key={index}>✓ {feature}</li>
                    ))}
                </ul>
            </div>

            <div style={{ backgroundColor: '#f9f9f9', padding: '15px', border: '1px solid #ddd' }}>
                <h4>הוראות הגעה:</h4>
                <p>{shelter.instructions}</p>
                
                {/* מקום שמור (Placeholder) למפה */}
                <div style={{ width: '100%', height: '150px', backgroundColor: '#e0e0e0', marginTop: '10px', border: '1px solid #ccc' }}>
                    <p style={{ textAlign: 'center', paddingTop: '65px' }}>כאן תוצג מפת הניווט</p>
                </div>
            </div>
        </div>
    );
}

export default ShelterDetailsScreen;