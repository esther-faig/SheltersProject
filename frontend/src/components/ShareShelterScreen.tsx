import React from 'react';

interface Option {
  id: number;
  name: string;
}

const shelterTypes: Option[] = [
  { id: 1, name: 'ממ"ד פרטי' },
  { id: 2, name: 'מקלט פרטי' },
  { id: 3, name: 'מרחב מוגן אחר' },
];

const features: Option[] = [
  { id: 1, name: 'יש מדרגות' },
  { id: 2, name: 'נגיש' },
  { id: 3, name: 'יש אוורור' },
  { id: 4, name: 'מרווח' },
  { id: 5, name: 'מקומות ישיבה' },
];

function ShareShelterScreen() {
  return (
    <div>
      <h2>שיתוף מקלט פרטי</h2>
      <input type="text" placeholder="שם המקלט" />
      <select>
        {shelterTypes.map(type => (
          <option key={type.id}>{type.name}</option>
        ))}
      </select>
      <input type="text" placeholder="כתובת" />
      <div>כאן תוצג המפה</div>
      <h3>מאפיינים</h3>
      {features.map(feature => (
        <div key={feature.id}>
          <input type="checkbox" /> {feature.name}
        </div>
      ))}
      <input type="file" />
      <button>שיתוף המקלט</button>
    </div>
  );
}

export default ShareShelterScreen;
