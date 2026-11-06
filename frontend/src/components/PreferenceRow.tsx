import { useState } from 'react';

interface PreferenceRowProps {
  name: string;
}

function PreferenceRow(props: PreferenceRowProps) {
  const [isChecked, setIsChecked] = useState(false);
  const [isRequired, setIsRequired] = useState(false);

  return (
    <div className={isChecked ? 'pref-row on' : 'pref-row'}>
      <label>
        <input type="checkbox" checked={isChecked} onChange={() => setIsChecked(!isChecked)} />
        {props.name}
      </label>
      {isChecked && (
        <button type="button" className="req-btn" onClick={() => setIsRequired(!isRequired)}>
          {isRequired ? 'חובה ✓' : 'סמן כחובה'}
        </button>
      )}
    </div>
  );
}

export default PreferenceRow;