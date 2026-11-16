import Icon from './Icon';

export const PUBLIC_INFO_TEXT =
  'מידע ציבורי: פרטי המקלט נאספו ממאגרי מידע ציבוריים (למשל עיריות) ועשויים להיות לא מעודכנים.';

export function formatTravel(seconds: number) {
  return seconds < 60 ? `${seconds} שנ׳` : `${Math.ceil(seconds / 60)} דק׳`;
}

export function formatDistance(meters: number) {
  return meters >= 1000 ? `${(meters / 1000).toFixed(1)} ק״מ` : `${Math.round(meters)} מטר`;
}

// סוג המקלט
export function TypeChip({ type }: { type: string }) {
  return (
    <span className="chip">
      <Icon name="shield" />
      {type}
    </span>
  );
}

// מי הוסיף את המקלט: מידע ציבורי או משתמש
export function ShelterSourceBadge({ isImported }: { isImported: boolean }) {
  return isImported ? (
    <span className="chip warn">
      <Icon name="info" />
      מידע ציבורי
    </span>
  ) : (
    <span className="chip ok">
      <Icon name="heart" />
      שותף ע״י משתמש
    </span>
  );
}

export function ShelterSourceNote({ isImported }: { isImported: boolean }) {
  if (!isImported) return null;
  return (
    <div className="info" style={{ marginTop: 10 }}>
      <Icon name="info" />
      <span>{PUBLIC_INFO_TEXT}</span>
    </div>
  );
}