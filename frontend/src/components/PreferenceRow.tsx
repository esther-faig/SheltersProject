import Icon, { featureIcon } from './Icon';

interface PreferenceRowProps {
  name: string;
  checked: boolean;
  required: boolean;
  onChange: (checked: boolean, required: boolean) => void;
}

function PreferenceRow({ name, checked, required, onChange }: PreferenceRowProps) {
  return (
    <div className="prow">
      <label className="chk">
        <input type="checkbox" checked={checked} onChange={() => onChange(!checked, false)} />
        <span className="box"><Icon name="check" /></span>
        <span className="lt">
          <Icon name={featureIcon(name)} />
          {name}
        </span>
      </label>
      <label className="must" title="האם זו דרישת חובה?">
        <input
          type="checkbox"
          checked={checked && required}
          disabled={!checked}
          onChange={() => onChange(true, !required)}
        />
        <span><Icon name="lock" />חובה</span>
      </label>
    </div>
  );
}

export default PreferenceRow;
