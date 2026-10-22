import React from 'react';
import ShelterCard from './ShelterCard';

interface ShelterData {
  id: number;
  name: string;
  distance: number;
  minutes: number;
  features: string[];
}

const recommendedShelter: ShelterData = {
  id: 1,
  name: 'מקלט ציבורי - הרצל 10',
  distance: 150,
  minutes: 2,
  features: ['ללא מדרגות', 'מרחב גדול', 'אוורור'],
};

const otherShelters: ShelterData[] = [
  { id: 2, name: 'ממ"ד פרטי - משפחת כהן', distance: 300, minutes: 4, features: ['אוורור'] },
  { id: 3, name: 'מקלט בית ספר אורט', distance: 450, minutes: 6, features: ['מרחב גדול', 'מקומות ישיבה'] },
];

function ResultsScreen() {
  return (
    <div>
      <h2>המקלט המומלץ עבורך</h2>
      <ShelterCard
        name={recommendedShelter.name}
        distance={recommendedShelter.distance}
        minutes={recommendedShelter.minutes}
        features={recommendedShelter.features}
        isRecommended={true}
      />
      <h2>אפשרויות נוספות</h2>
      {otherShelters.map(shelter => (
        <ShelterCard
          key={shelter.id}
          name={shelter.name}
          distance={shelter.distance}
          minutes={shelter.minutes}
          features={shelter.features}
          isRecommended={false}
        />
      ))}
    </div>
  );
}

export default ResultsScreen;
