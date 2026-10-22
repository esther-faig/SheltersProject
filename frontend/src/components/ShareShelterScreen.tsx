import React, { useState } from 'react';

function ShareShelterScreen() {
    const [shelterName, setShelterName] = useState<string>('');
    const [shelterType, setShelterType] = useState<string>('');
    const [address, setAddress] = useState<string>('');

    return (
        <div>
            <h2>שיתוף מקלט פרטי</h2>
            
            <form onSubmit={(e) => e.preventDefault()}>
                <div>
                    <label>שם המקלט: </label>
                    <input type="text" onChange={(e) => setShelterName(e.target.value)} />
                </div>
                <br />
                
                <div>
                    <label>סוג המקום: </label>
                    <select onChange={(e) => setShelterType(e.target.value)}>
                        <option value="">בחר סוג...</option>
                        <option value="mamad">ממ"ד פרטי</option>
                        <option value="private_shelter">מקלט פרטי</option>
                        <option value="other">מרחב מוגן אחר</option>
                    </select>
                </div>
                <br />

                <div>
                    <label>מיקום / כתובת: </label>
                    <input type="text" onChange={(e) => setAddress(e.target.value)} />
                </div>
                <br />

                <div>
                    <h4>מאפיינים:</h4>
                    <label><input type="checkbox" /> יש מדרגות?</label><br />
                    <label><input type="checkbox" /> המקום נגיש?</label><br />
                    <label><input type="checkbox" /> יש אוורור?</label><br />
                    <label><input type="checkbox" /> המקום מרווח?</label>
                </div>
                <br />

                <button type="submit">שיתוף המקלט</button>
            </form>
        </div>
    );
}

export default ShareShelterScreen;