import React, { useState } from 'react';

function SearchScreen() {
  const [isLocationShared, setIsLocationShared] = useState(false);
  const [isCar, setIsCar] = useState(false);

  return (
    <div>
      <h2>חיפוש מקלט</h2>
      <button onClick={() => setIsLocationShared(true)}>שתף את המיקום שלי</button>
      <div>{isLocationShared ? '📍 המיקום שלך משותף' : 'כאן תוצג המפה'}</div>
      <p>אופן ההגעה: {isCar ? 'רכב' : 'הליכה'}</p>
      <button onClick={() => setIsCar(false)}>הליכה</button>
      <button onClick={() => setIsCar(true)}>רכב</button>
      <button>מצא לי מקלט</button>
    </div>
  );
}

export default SearchScreen;
