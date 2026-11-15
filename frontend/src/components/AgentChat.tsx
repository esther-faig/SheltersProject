import { useEffect, useRef, useState } from 'react';
import type { FormEvent } from 'react';
import { request } from '../api/http';

type Msg = { id: number; from: 'agent' | 'user'; text: string; error?: boolean };

const WELCOME: Msg[] = [
  { id: 1, from: 'agent', text: 'שלום! אני הסוכן של SafeZone.' },
  { id: 2, from: 'agent', text: 'אפשר לשאול אותי איפה המקלט הקרוב ביותר, או איך משתפים מקלט חדש.' },
];

function AgentChat() {
  const [open, setOpen] = useState(false);
  const [text, setText] = useState('');
  const [loading, setLoading] = useState(false);
  const [messages, setMessages] = useState<Msg[]>(WELCOME);
  const listRef = useRef<HTMLDivElement>(null);

  // גלילה אוטומטית להודעה האחרונה
  useEffect(() => {
    if (open && listRef.current) {
      listRef.current.scrollTop = listRef.current.scrollHeight;
    }
  }, [messages, open]);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    const value = text.trim();
    if (!value || loading) return;

    const userMsg: Msg = { id: Date.now(), from: 'user', text: value };
    setMessages((m) => [...m, userMsg]);
    setText('');
    setLoading(true);

    // ההיסטוריה שנשלחת לשרת: בלי הודעות הפתיחה ובלי הודעות שגיאה
    const history = [...messages, userMsg]
      .filter((m) => m.id > WELCOME.length && !m.error)
      .map((m) => ({ role: m.from === 'user' ? 'user' : 'assistant', text: m.text }));

    try {
      const res = await request<{ text: string }>('/Chat', {
        method: 'POST',
        body: JSON.stringify({ messages: history }),
      });
      setMessages((m) => [...m, { id: Date.now() + 1, from: 'agent', text: res.text }]);
    } catch {
      setMessages((m) => [
        ...m,
        { id: Date.now() + 1, from: 'agent', text: 'לא הצלחתי להתחבר לשרת. נסו שוב בעוד רגע.', error: true },
      ]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="agent">
      {open && (
        <section className="agent-panel" role="dialog" aria-label="צ'אט עם הסוכן">
          <header className="agent-head">
            <span className="agent-avatar" aria-hidden="true">
              <svg viewBox="0 0 24 24">
                <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
                <path d="m9 12 2 2 4-4" />
              </svg>
            </span>
            <div>
              <div className="agent-name">הסוכן של SafeZone</div>
              <div className="agent-status">זמין עכשיו</div>
            </div>
          </header>

          <div className="agent-list" ref={listRef} aria-live="polite">
            {messages.map((m) => (
              <div key={m.id} className={`agent-msg from-${m.from}`}>
                {m.text}
              </div>
            ))}
          </div>

          <form className="agent-form" onSubmit={handleSubmit}>
            <input
              className="agent-input"
              type="text"
              value={text}
              onChange={(e) => setText(e.target.value)}
              placeholder={loading ? 'הסוכן כותב...' : 'כתבו הודעה...'}
              aria-label="הודעה לסוכן"
            />
            <button className="agent-send" type="submit" disabled={!text.trim() || loading} aria-label="שליחה">
              <svg viewBox="0 0 24 24">
                <path d="M22 2 11 13" />
                <path d="M22 2 15 22l-4-9-9-4 20-7z" />
              </svg>
            </button>
          </form>
        </section>
      )}

      <button
        className="agent-fab"
        type="button"
        onClick={() => setOpen((o) => !o)}
        aria-label={open ? 'סגירת הצ׳אט' : 'פתיחת הצ׳אט'}
        aria-expanded={open}
      >
        {open ? (
          <svg viewBox="0 0 24 24">
            <path d="M18 6 6 18" />
            <path d="m6 6 12 12" />
          </svg>
        ) : (
          <svg viewBox="0 0 24 24">
            <path d="M21 11.5a8.4 8.4 0 0 1-.9 3.8 8.5 8.5 0 0 1-7.6 4.7 8.4 8.4 0 0 1-3.8-.9L3 21l1.9-5.7a8.4 8.4 0 0 1-.9-3.8 8.5 8.5 0 0 1 4.7-7.6 8.4 8.4 0 0 1 3.8-.9h.5a8.5 8.5 0 0 1 8 8v.5z" />
          </svg>
        )}
      </button>
    </div>
  );
}

export default AgentChat;