// ריכוז ה-Hooks של הפרויקט:
// 1. Hooks של Redux עם טיפוסים, כדי שרכיבי React יגשו למידע שב-Store.
// 2. Custom Hooks: useGeolocation ו-useDocumentTitle.

import { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import type { RootState, AppDispatch } from './store';

// ───────── Redux ─────────
export const useAppDispatch = useDispatch.withTypes<AppDispatch>();
export const useAppSelector = useSelector.withTypes<RootState>();

// ───────── Custom Hooks ─────────

export type Coords = { lat: number; lng: number };

// איתור מיקום המשתמש בדפדפן.
// failMessage: ההודעה שתוצג אם האיתור נכשל (כל מסך מנסח אותה בדרכו)
export function useGeolocation(failMessage: string) {
  const [locating, setLocating] = useState(false);

  // מחזיר Promise עם הקואורדינטות. בכשלון זורק Error עם הודעה מוכנה להצגה
  const locate = useCallback(
    () =>
      new Promise<Coords>((resolve, reject) => {
        if (!navigator.geolocation) {
          reject(new Error('הדפדפן לא תומך באיתור מיקום'));
          return;
        }

        setLocating(true);

        navigator.geolocation.getCurrentPosition(
          (position) => {
            setLocating(false);
            resolve({ lat: position.coords.latitude, lng: position.coords.longitude });
          },
          () => {
            setLocating(false);
            reject(new Error(failMessage));
          },
          { enableHighAccuracy: true, timeout: 10000 }
        );
      }),
    [failMessage]
  );

  return { locating, locate };
}

// קובע את כותרת הלשונית של הדפדפן
export function useDocumentTitle(title: string) {
  useEffect(() => {
    document.title = title;
  }, [title]);
}