import React, { useState } from 'react';

interface PreferenceRowProps {
  name: string;
}

function PreferenceRow(props: PreferenceRowProps) {
  const [isChecked, setIsChecked] = useState(false);
  const [isRequired, setIsRequired] = useState(false);

  return (
    <div>
      <input type="checkbox" checked={isChecked} onChange={() => setIsChecked(!isChecked)} />
      <span>{props.name} </span>
      {isChecked && (
        <button onClick={() => setIsRequired(!isRequired)}>
          {isRequired ? 'חובה ✓' : 'סמן כחובה'}
        </button>
      )}
    </div>
  );
}

export default PreferenceRow;
