//נתוני דמה
export type Shelter = {
  id: number;
  name: string;
  distance: number;
  walkMin: number;
  carMin: number;
  features: string[];
  lat: number;
  lng: number;
};

export const shelters: Shelter[] = [
  { id: 1, name: 'מקלט ציבורי - הרצל 10', distance: 150, walkMin: 2, carMin: 1, features: ['ללא מדרגות', 'מרחב גדול', 'אוורור'], lat: 32.0786, lng: 34.852 },
  { id: 2, name: 'ממ"ד פרטי - משפחת כהן', distance: 300, walkMin: 4, carMin: 1, features: ['אוורור'], lat: 32.0795, lng: 34.8545 },
  { id: 3, name: 'מקלט בית ספר אורט', distance: 450, walkMin: 6, carMin: 2, features: ['מרחב גדול', 'מקומות ישיבה'], lat: 32.077, lng: 34.849 },
  { id: 4, name: 'מקלט קהילתי - ויצמן 3', distance: 900, walkMin: 11, carMin: 3, features: ['ללא מדרגות', 'מקומות ישיבה'], lat: 32.0825, lng: 34.856 },
  { id: 5, name: 'מקלט מרכז מסחרי', distance: 2500, walkMin: 30, carMin: 6, features: ['מרחב גדול', 'אוורור'], lat: 32.095, lng: 34.87 },
];