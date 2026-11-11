import { useState } from 'react';
import { streamChat, type ChatMessage } from '../services/chatService';

function ChatScreen() {
  // ניהול ה-State של הרכיב
  const [messages, setMessages] = useState<ChatMessage[]>([]); // היסטוריית הודעות הצ'אט
  const [input, setInput] = useState('');                      // תוכן תיבת ההקלדה
  const [loading, setLoading] = useState(false);                 // דגל מצב טעינה/סטרימינג
  const [error, setError] = useState('');                      // הודעת שגיאה במידה וקיימת

  /**
   * פונקציה הטיפול בשליחת הודעה חדשה
   */
  async function send() {
    // מניעת שליחת הודעה ריקה או שליחה כפולה בזמן שטעינה פעילה
    if (!input.trim() || loading) return;

    // 1. יצירת ההיסטוריה המעודכנת כולל הודעת המשתמש החדשה
    const history: ChatMessage[] = [...messages, { role: 'user', text: input }];

    // 2. הוספת הודעת Assistant ריקה בסוף הרשימה שמידע יישפך אליה בסטרימינג
    setMessages([...history, { role: 'assistant', text: '' }]);
    setInput('');
    setError('');
    setLoading(true);

    try {
      // 3. הפעלת הסטרימינג ועדכון ההודעה האחרונה בזה אחר זה (Chunk by Chunk)
      await streamChat(history, chunk => {
        setMessages(list => {
          const last = list[list.length - 1]; // שליפת ההודעה הריקה של ה-assistant
          return [...list.slice(0, -1), { ...last, text: last.text + chunk }]; // שרשור המקטע החדש
        });
      });
    } catch (e) {
      // תפיסת שגיאות והצגתן ב-UI
      setError((e as Error).message);
    } finally {
      // סיום תהליך השרשור
      setLoading(false);
    }
  }

  return (
    <div dir="rtl">
      <h2>עוזר היערכות</h2>

      {/* הרצת מפת הודעות והצגתן */}
      {messages.map((m, i) => (
        <p key={i}>
          <strong>{m.role === 'user' ? 'את/ה' : 'עוזר'}: </strong>
          {/* אם הודעת ה-assistant עדיין ריקה והשרת בטעינה, נציג חיווי 'חושב...' */}
          {m.text || (loading ? 'חושב...' : '')}
        </p>
      ))}

      {/* הצגת שגיאה באדום במידה וקיימת */}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* תיבת הקלט וכפתור השליחה */}
      <input 
        value={input} 
        onChange={e => setInput(e.target.value)} 
        onKeyDown={e => e.key === 'Enter' && send()} // שיפור נוחות: שליחה בלחיצה על Enter
        placeholder="שאל/י שאלה על מקלטים..."
      />
      <button onClick={send} disabled={loading}>
        {loading ? 'שולח...' : 'שליחה'}
      </button>
    </div>
  );
}

export default ChatScreen;