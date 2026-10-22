import React from 'react';

interface MyShelterCardProps {
  name: string;
  address: string;
}

function MyShelterCard(props: MyShelterCardProps) {
  return (
    <div>
      <h4>{props.name}</h4>
      <p>{props.address}</p>
      <button>עריכת מקלט</button>
    </div>
  );
}

export default MyShelterCard;
