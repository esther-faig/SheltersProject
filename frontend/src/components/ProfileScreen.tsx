import React from 'react';
import MyShelterCard from './MyShelterCard';

interface Preference {
  id: number;
  name: string;
}

interface MyShelter {
  id: number;
  name: string;
  address: string;
}

const myPreferences: Preference[] = [
  { id: 1, name: 'ללא מדרגות' },
  { id: 2, name: 'אוורור טוב' },
];

const myShelters: MyShelter[] = [
  { id: 1, name: 'ממ"ד פרטי', address: 'רחוב הגפן 5, גבעת שמואל' },
];

function ProfileScreen() {
  return (
    <div>
      <h2>הפרופיל שלי</h2>
      <p>שם: רחל כהן</p>
      <p>מייל: rachel@example.com</p>

      <h3>ההעדפות שלי</h3>
      <ul>
        {myPreferences.map(preference => (
          <li key={preference.id}>{preference.name}</li>
        ))}
      </ul>
      <button>עריכת העדפות</button>

      <h3>המקלטים ששיתפתי</h3>
      {myShelters.map(shelter => (
        <MyShelterCard key={shelter.id} name={shelter.name} address={shelter.address} />
      ))}
    </div>
  );
}

export default ProfileScreen;
