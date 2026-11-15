import type { UserPreference } from '../store/profile/profileState';

// העדפה שנבחרה. הסדר ברשימה הוא סדר החשיבות
export interface PickedPreference {
  preferenceId: number;
  isRequired: boolean;
}

// הופך העדפות מהשרת לרשימה מסודרת לפי Priority (לעריכה בפרופיל)
export function fromUserPreferences(list: UserPreference[]): PickedPreference[] {
  return [...list]
    .sort((a, b) => a.priority - b.priority)
    .map((p) => ({ preferenceId: p.preferenceId, isRequired: p.isRequired }));
}

// מחשב Priority לשמירה: הרגילות קודם (1..k רצוף, לפי הסדר), ואחריהן החובה.
// שומר שדות נוספים (כמו name), כדי שהמידע ב-Redux לא יאבד
export function toPriorityList<T extends { isRequired: boolean }>(
  items: T[]
): (T & { priority: number })[] {
  const ordered = [
    ...items.filter((p) => !p.isRequired),
    ...items.filter((p) => p.isRequired),
  ];
  return ordered.map((p, i) => ({ ...p, priority: i + 1 }));
}