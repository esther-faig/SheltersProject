import React from 'react';
import FeatureTag from './FeatureTag';
// משמש להצגת פרטים של מקלט ובדרך כלל משתמשים בו בקומפוננטות אחרות


interface ShelterCardProps {
  name: string;
  distance: number;
  minutes: number;
  features: string[];
  isRecommended: boolean;
}

function ShelterCard(props: ShelterCardProps) {
  return (
    <div>
      {props.isRecommended ? <h3>⭐ {props.name}</h3> : <h4>{props.name}</h4>}
      <p>מרחק: {props.distance} מטר</p>
      <p>זמן הגעה: {props.minutes} דקות</p>
      <div>
        {props.features.map(feature => (
          <FeatureTag key={feature} text={feature} />
        ))}
      </div>
      <button>ניווט</button>
    </div>
  );
}

export default ShelterCard;
