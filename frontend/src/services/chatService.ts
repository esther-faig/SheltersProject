export interface ChatMessage {
  role: 'user' | 'assistant';
  text: string;
}

const URL = 'https://localhost:7273/api/chat/stream';

/**
 * פונקציה לביצוע Streaming של הודעות צ'אט מהשרת (Server-Sent Events / Chunked Response).
 * @param messages רשימת ההודעות ההיסטוריות בשיחה
 * @param onChunk קולבק המופעל בכל פעם שמגיע חלקיק טקסט (Chunk) חדש מהשרת
 * @param token טוקן ההזדהות של המשתמש (Bearer Token)
 */
export async function streamChat(
  messages: ChatMessage[],
  onChunk: (text: string) => void,
  token: string // קבלת הטוקן כפרמטר לצורך ההזדהות בבקשה
): Promise<void> {
  const res = await fetch(URL, {
    method: 'POST',
    headers: { 
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}` // הוספת כותרת ההזדהות הידנית כיוון ש-fetch אינו עובר דרך interceptors
    },
    body: JSON.stringify({ messages }),
  });
  
  // בדיקה האם הבקשה הצליחה ויש גוף נתונים (Stream) לקריאה
  if (!res.ok || !res.body) {
    const err = await res.json().catch(() => null);
    throw new Error(err?.title ?? 'Chat request failed');
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