import React, { useState } from 'react';

// הגדרת טיפוס למקלט הממתין לאישור לפי הנחיות המורה
interface PendingShelter {
    id: number;
    name: string;
    type: string;
    address: string;
}

function AdminScreen() {
    // נתונים קבועים (hardcoded) של מקלטים שממתינים לאישור
    const [pendingShelters, setPendingShelters] = useState<PendingShelter[]>([
        { id: 1, name: 'ממ"ד משפחת לוי', type: 'ממ"ד פרטי', address: 'רחוב הרצל 5' },
        { id: 2, name: 'מקלט בניין 4', type: 'מקלט פרטי', address: 'רחוב ויצמן 12' }
    ]);

    // פונקציה שמסירה את המקלט מהרשימה לאחר אישור או דחייה
    const handleAction = (id: number) => {
        const updatedList = pendingShelters.filter(shelter => shelter.id !== id);
        setPendingShelters(updatedList);
    };

    return (
        <div>
            <h2>ניהול ואישור מקלטים (מסך מנהל)</h2>
            
            {/* אם אין מקלטים, נציג הודעה מתאימה. אחרת נציג את הרשימה. */}
            {pendingShelters.length === 0 ? (
                <p>אין מקלטים הממתינים לאישור.</p>
            ) : (
                <div>
                    {pendingShelters.map((shelter) => (
                        <div key={shelter.id} style={{ border: '1px solid #ccc', padding: '10px', marginBottom: '10px' }}>
                            <h3>{shelter.name}</h3>
                            <p><strong>סוג:</strong> {shelter.type}</p>
                            <p><strong>כתובת:</strong> {shelter.address}</p>
                            
                            <button onClick={() => handleAction(shelter.id)} style={{ backgroundColor: '#ccffcc', marginRight: '10px' }}>
                                אישור
                            </button>
                            <button onClick={() => handleAction(shelter.id)} style={{ backgroundColor: '#ffcccc' }}>
                                דחייה
                            </button>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default AdminScreen;