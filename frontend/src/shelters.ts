//נתוני דמה
export type Shelter = {
  id: number;
  name: string;
  distance: number;
  walkMin: number;
  carMin: number;
  features: string[];
};

export const shelters: Shelter[] = [
  { id: 1, name: 'מקלט ציבורי - הרצל 10', distance: 150, walkMin: 2, carMin: 1, features: ['ללא מדרגות', 'מרחב גדול', 'אוורור'] },
  { id: 2, name: 'ממ"ד פרטי - משפחת כהן', distance: 300, walkMin: 4, carMin: 1, features: ['אוורור'] },
  { id: 3, name: 'מקלט בית ספר אורט', distance: 450, walkMin: 6, carMin: 2, features: ['מרחב גדול', 'מקומות ישיבה'] },
  { id: 4, name: 'מקלט קהילתי - ויצמן 3', distance: 900, walkMin: 11, carMin: 3, features: ['ללא מדרגות', 'מקומות ישיבה'] },
  { id: 5, name: 'מקלט מרכז מסחרי', distance: 2500, walkMin: 30, carMin: 6, features: ['מרחב גדול', 'אוורור'] },
];