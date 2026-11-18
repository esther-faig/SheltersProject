// ריכוז התקשורת בין ריאקט לשרת

const BASE_URL = import.meta.env.VITE_API_URL as string;
const TOKEN_KEY = 'token';

export const getToken = () => localStorage.getItem(TOKEN_KEY);
export const saveToken = (t: string) => localStorage.setItem(TOKEN_KEY, t);
export const clearToken = () => localStorage.removeItem(TOKEN_KEY);

const HEBREW = /[\u0590-\u05FF]/;
const NETWORK_ERROR = 'לא ניתן להתחבר לשרת. בדקו את החיבור לאינטרנט ונסו שוב';
const SERVER_ERROR = 'אירעה שגיאה בשרת. נסו שוב מאוחר יותר';
const STATUS_MESSAGES: Record<number, string> = {
  400: 'הנתונים שנשלחו אינם תקינים',
  401: 'נדרשת התחברות',
  403: 'אין לך הרשאה לבצע פעולה זו',
  404: 'הפריט המבוקש לא נמצא',
  409: 'הפעולה מתנגשת עם נתונים קיימים',
  413: 'הקובץ גדול מדי',
  429: 'יותר מדי בקשות. נסו שוב בעוד רגע',
};

// מחזיר תמיד הודעה בעברית: מהשרת אם היא בעברית, ואחרת לפי קוד הסטטוס
function toHebrewMessage(status: number, raw: string): string {
  let message = raw;
  try {
    message = (JSON.parse(raw) as { title?: string }).title ?? raw;
  } catch {
    // לא JSON: משתמשים בטקסט כמו שהוא
  }
  if (message && HEBREW.test(message)) return message;
  return STATUS_MESSAGES[status] ?? (status >= 500 ? SERVER_ERROR : `שגיאה ${status}`);
}

export async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const isAuthCall = path.startsWith('/User/login') || path.startsWith('/User/register');

  const headers = new Headers(options.headers);
  const token = getToken();
  if (token && !isAuthCall) headers.set('Authorization', `Bearer ${token}`);

  // JSON צריך header. FormData לא: הדפדפן מוסיף אותו לבד, אחרת העלאת תמונות נשברת
  if (options.body && !(options.body instanceof FormData)) {
    headers.set('Content-Type', 'application/json');
  }

  let response: Response;
  try {
    response = await fetch(`${BASE_URL}${path}`, { ...options, headers });
  } catch {
    throw new Error(NETWORK_ERROR);
  }

  if (response.status === 401 && token && !isAuthCall) {   // טוקן שפג תוקף
    clearToken();
    window.location.href = '/login';
    throw new Error('נדרשת התחברות מחדש');
  }

  if (!response.ok) {
    throw new Error(toHebrewMessage(response.status, await response.text()));
  }

  const text = await response.text();        // register ו-204 חוזרים בלי גוף
  return (text ? JSON.parse(text) : undefined) as T;
}