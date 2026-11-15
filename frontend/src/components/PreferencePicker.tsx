import Icon, { featureIcon } from './Icon';
import PreferenceRow from './PreferenceRow';
import type { Preference } from '../store/preferences/preferencesState';
import type { PickedPreference } from '../utils/pickedPreferences';

interface PreferencePickerProps {
  all: Preference[];                       // כל ההעדפות במערכת
  value: PickedPreference[];               // מה שנבחר, לפי סדר החשיבות
  onChange: (value: PickedPreference[]) => void;
}

function PreferencePicker({ all, value, onChange }: PreferencePickerProps) {
  const optional = value.filter((p) => !p.isRequired);
  const nameOf = (id: number) => all.find((p) => p.preferenceId === id)?.name ?? '';

  // סימון / ביטול / שינוי חובה
  function handleRow(id: number, checked: boolean, required: boolean) {
    if (!checked) {
      onChange(value.filter((p) => p.preferenceId !== id));
    } else if (value.some((p) => p.preferenceId === id)) {
      onChange(value.map((p) => (p.preferenceId === id ? { ...p, isRequired: required } : p)));
    } else {
      onChange([...value, { preferenceId: id, isRequired: required }]); // חדשה נכנסת בסוף
    }
  }

  // מחליף מקום עם ההעדפה הרגילה הקודמת / הבאה
  function move(id: number, direction: -1 | 1) {
    const ids = optional.map((p) => p.preferenceId);
    const i = ids.indexOf(id);
    const j = i + direction;
    if (i === -1 || j < 0 || j >= ids.length) return;

    [ids[i], ids[j]] = [ids[j], ids[i]];
    const required = value.filter((p) => p.isRequired);
    onChange([...ids.map((pid) => ({ preferenceId: pid, isRequired: false })), ...required]);
  }

  return (
    <>
      <div className="plist">
        {all.map((p) => {
          const item = value.find((v) => v.preferenceId === p.preferenceId);
          return (
            <PreferenceRow
              key={p.preferenceId}
              name={p.name}
              checked={item !== undefined}
              required={item?.isRequired ?? false}
              onChange={(checked, required) => handleRow(p.preferenceId, checked, required)}
            />
          );
        })}
      </div>

      {/* סידור רק כשיש לפחות שתי העדפות רגילות */}
      {optional.length > 1 && (
        <div className="order-box">
          <span className="lbl">סדר החשיבות של ההעדפות</span>
          <div className="hint" style={{ margin: '0 0 10px' }}>
            מהחשובה ביותר (1) ועד הפחות חשובה. מקלט שמתאים להעדפות הראשונות יקבל ציון גבוה יותר.
          </div>

          {optional.map((p, i) => (
            <div className="pref-line" key={p.preferenceId}>
              <span className="l">
                <span className="counter">{i + 1}</span>
                <Icon name={featureIcon(nameOf(p.preferenceId))} />
                {nameOf(p.preferenceId)}
              </span>

              <span style={{ display: 'flex', gap: 6 }}>
                <button
                  type="button"
                  className="iconx"
                  aria-label={`הזזת ${nameOf(p.preferenceId)} למעלה`}
                  disabled={i === 0}
                  onClick={() => move(p.preferenceId, -1)}
                >
                  <Icon name="arrow-up" />
                </button>
                <button
                  type="button"
                  className="iconx"
                  aria-label={`הזזת ${nameOf(p.preferenceId)} למטה`}
                  disabled={i === optional.length - 1}
                  onClick={() => move(p.preferenceId, 1)}
                >
                  <Icon name="arrow-down" />
                </button>
              </span>
            </div>
          ))}
        </div>
      )}
    </>
  );
}

export default PreferencePicker;