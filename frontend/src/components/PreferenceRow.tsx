import { useState } from 'react';

interface PreferenceRowProps {
name: string;
  checked: boolean;
  required: boolean;
  onChange: (checked: boolean, required: boolean) => void;
}

function PreferenceRow({ name, checked, required, onChange }: PreferenceRowProps) {


  return (
    <div className={checked ? 'pref-row on' : 'pref-row'}>
      <label>
        <input type="checkbox" checked={checked} onChange={() => onChange(!checked, false)} />
        {name}
      </label>
      {checked && (
        <button type="button" className="req-btn" onClick={() => onChange(true, !required)}>
          {required ? 'חובה ✓' : 'סמן כחובה'}
        </button>
      )}
    </div>
  );
}

export default PreferenceRow;