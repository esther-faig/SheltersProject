//import { getToken } from '../api/http';

export interface ChatMessage {
  role: 'user' | 'assistant';
  text: string;
}

// אותה כתובת בסיס כמו שאר האתר (VITE_API_URL ב-.env), במקום כתובת קבועה בקוד
const URL = `${import.meta.env.VITE_API_URL}/chat/stream`;

/**
 * פונקציה לביצוע Streaming של הודעות צ'אט מהשרת (Server-Sent Events / Chunked Response).
 * @param messages רשימת ההודעות ההיסטוריות בשיחה
 * @param onChunk קולבק המופעל בכל פעם שמגיע חלקיק טקסט (Chunk) חדש מהשרת
 * @param token טוקן ההזדהות (לא חובה). אם לא נשלח, לוקחים את הטוקן השמור של המשתמש המחובר
 */
export async function streamChat(
  messages: ChatMessage[],
  onChunk: (text: string) => void,
  token?: string | null
): Promise<void> {
  const headers: Record<string, string> = { 'Content-Type': 'application/json' };
  if (token) headers.Authorization = `Bearer ${token}`;

  let res: Response;
  try {
    res = await fetch(URL, { method: 'POST', headers, body: JSON.stringify({ messages }) });
  } catch {
    throw new Error('לא ניתן להתחבר לשרת. נסו שוב בעוד רגע.');
  }

  if (res.status === 429) throw new Error('יותר מדי הודעות בזמן קצר. נסו שוב בעוד דקה.');

  // שאר הפונקציה נשארת כמו שהיא, מהשורה: if (!res.ok || !res.body) { ...

  // בדיקה האם הבקשה הצליחה ויש גוף נתונים (Stream) לקריאה
  if (!res.ok || !res.body) {
    const err = await res.json().catch(() => null);
    throw new Error(err?.title ?? 'הבקשה נכשלה. נסו שוב.');
  }

  // פתיחת קורא (Reader) לזרם הנתונים בעזרת TextDecoderStream הממיר בייטים למחרוזות
  const reader = res.body.pipeThrough(new TextDecoderStream()).getReader();
  let buffer = '';

  while (true) {
    const { value, done } = await reader.read();
    if (done) break;

    buffer += value;
    const blocks = buffer.split('\n\n');
    buffer = blocks.pop()!;

    // מעבר על הבלוקים שהתקבלו ופענוח נתוני ה-Stream
    for (const block of blocks) {
      const data = block.split('\n').find(l => l.startsWith('data: '))?.slice(6);
      if (!data || data === '[DONE]') continue;

      const payload = JSON.parse(data);
      if (block.startsWith('event: error')) throw new Error(payload.title);

      // הפעלת הקולבק עם הטקסט שהתקבל
      onChunk(payload.text);
    }
  }
}