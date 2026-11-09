// ריכוז התקשורת בין ריאקט לשרת

const BASE_URL = import.meta.env.VITE_API_URL as string;
const TOKEN_KEY = 'token';

export const getToken = () => localStorage.getItem(TOKEN_KEY);
export const saveToken = (t: string) => localStorage.setItem(TOKEN_KEY, t);
export const clearToken = () => localStorage.removeItem(TOKEN_KEY);

export async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const headers = new Headers(options.headers);
  const token = getToken();
  if (token) headers.set('Authorization', `Bearer ${token}`);

  // JSON צריך header. FormData לא: הדפדפן מוסיף אותו לבד, אחרת העלאת תמונות נשברת
  if (options.body && !(options.body instanceof FormData)) {
    headers.set('Content-Type', 'application/json');
  }

  const response = await fetch(`${BASE_URL}${path}`, { ...options, headers });

  if (response.status === 401 && token) {   // טוקן שפג תוקף
    clearToken();
    window.location.href = '/login';
    throw new Error('נדרשת התחברות מחדש');
  }
  if (!response.ok) {
    throw new Error((await response.text()) || `שגיאה ${response.status}`);
  }

  const text = await response.text();        // register ו-204 חוזרים בלי גוף
  return (text ? JSON.parse(text) : undefined) as T;
}
