import { useEffect, useState } from 'react';
import type { FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import Icon, { featureIcon } from './Icon';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import {
  fetchPreferences,
  createPreference,
  updatePreference,
  deletePreference,
} from '../store/preferences/preferencesThunks';
import type { Preference } from '../store/preferences/preferencesState';

interface Draft {
  name: string;
  description: string;
  category: string;
}

const EMPTY: Draft = { name: '', description: '', category: '' };

function PreferencesAdminScreen() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { items, loading, error } = useAppSelector((state) => state.preferences);

  const [newDraft, setNewDraft] = useState<Draft>(EMPTY);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editDraft, setEditDraft] = useState<Draft>(EMPTY);
  const [busy, setBusy] = useState(false);
  const [actionError, setActionError] = useState<string | null>(null);

  useEffect(() => {
    dispatch(fetchPreferences());
  }, [dispatch]);

  async function run(action: () => Promise<unknown>) {
    setBusy(true);
    setActionError(null);
    try {
      await action();
      return true;
    } catch (e) {
      setActionError(e instanceof Error ? e.message : 'הפעולה נכשלה');
      return false;
    } finally {
      setBusy(false);
    }
  }

  async function handleAdd(e: FormEvent) {
    e.preventDefault();
    if (!newDraft.name.trim()) return;
    const ok = await run(() =>
      dispatch(
        createPreference({
          name: newDraft.name.trim(),
          description: newDraft.description.trim() || undefined,
          category: newDraft.category.trim() || undefined,
        })
      ).unwrap()
    );
    if (ok) setNewDraft(EMPTY);
  }

  function startEdit(p: Preference) {
    setEditingId(p.preferenceId);
    setEditDraft({
      name: p.name,
      description: p.description ?? '',
      category: p.category ?? '',
    });
    setActionError(null);
  }

  async function saveEdit(id: number) {
    if (!editDraft.name.trim()) return;
    const ok = await run(() =>
      dispatch(
        updatePreference({
          preferenceId: id,
          name: editDraft.name.trim(),
          description: editDraft.description.trim() || undefined,
          category: editDraft.category.trim() || undefined,
        })
      ).unwrap()
    );
    if (ok) setEditingId(null);
  }

  async function remove(p: Preference) {
    if (!window.confirm(`למחוק את ההעדפה "${p.name}"?`)) return;
    await run(() => dispatch(deletePreference(p.preferenceId)).unwrap());
  }

  return (
    <section className="screen active">
      <div className="wrap inner" style={{ maxWidth: 900 }}>
        <div className="phead">
          <button
            className="iconbtn"
            type="button"
            onClick={() => navigate('/admin')}
            aria-label="חזרה לאזור הניהול"
          >
            <Icon name="arrow-r" />
          </button>
          <div>
            <h2>ניהול העדפות</h2>
            <div className="sub">
              {loading ? 'טוען...' : `${items.length} העדפות במערכת`}
            </div>
          </div>
        </div>

        <div className="stack">
          {/* הוספת העדפה */}
          <form className="card pref-form" onSubmit={handleAdd}>
            <h3 style={{ fontSize: 20, marginBottom: 14 }}>הוספת העדפה חדשה</h3>
            <div className="pref-grid">
              <div>
                <label className="lbl" htmlFor="pref-name">שם ההעדפה</label>
                <input
                  id="pref-name"
                  type="text"
                  value={newDraft.name}
                  onChange={(e) => setNewDraft({ ...newDraft, name: e.target.value })}
                  placeholder="למשל: כניסה ללא מדרגות"
                  required
                />
              </div>
              <div>
                <label className="lbl" htmlFor="pref-cat">קטגוריה</label>
                <input
                  id="pref-cat"
                  type="text"
                  value={newDraft.category}
                  onChange={(e) => setNewDraft({ ...newDraft, category: e.target.value })}
                  placeholder="אופציונלי"
                />
              </div>
              <div className="pref-wide">
                <label className="lbl" htmlFor="pref-desc">תיאור</label>
                <input
                  id="pref-desc"
                  type="text"
                  value={newDraft.description}
                  onChange={(e) => setNewDraft({ ...newDraft, description: e.target.value })}
                  placeholder="אופציונלי"
                />
              </div>
            </div>
            <div className="actions" style={{ marginTop: 16 }}>
              <button className="btn small primary" type="submit" disabled={busy || !newDraft.name.trim()}>
                <span className="sh"><Icon name="plus" /></span>
                הוספה
              </button>
            </div>
          </form>

          {actionError && <div className="err">שגיאה: {actionError}</div>}

          {/* רשימת העדפות */}
          {error ? (
            <div className="card empty err">שגיאה: {error}</div>
          ) : (
            <div className="card">
              {!loading && items.length === 0 && <div className="hint">אין העדפות להצגה.</div>}

              {items.map((p) =>
                editingId === p.preferenceId ? (
                  <div className="my-sh pref-edit" key={p.preferenceId}>
                    <div className="pref-grid grow">
                      <input
                        type="text"
                        aria-label="שם"
                        value={editDraft.name}
                        onChange={(e) => setEditDraft({ ...editDraft, name: e.target.value })}
                      />
                      <input
                        type="text"
                        aria-label="קטגוריה"
                        placeholder="קטגוריה"
                        value={editDraft.category}
                        onChange={(e) => setEditDraft({ ...editDraft, category: e.target.value })}
                      />
                      <input
                        className="pref-wide"
                        type="text"
                        aria-label="תיאור"
                        placeholder="תיאור"
                        value={editDraft.description}
                        onChange={(e) => setEditDraft({ ...editDraft, description: e.target.value })}
                      />
                    </div>
                    <div className="actions">
                      <button
                        className="btn small green"
                        type="button"
                        disabled={busy || !editDraft.name.trim()}
                        onClick={() => saveEdit(p.preferenceId)}
                      >
                        <span className="sh"><Icon name="check" /></span>
                        שמירה
                      </button>
                      <button className="btn small" type="button" onClick={() => setEditingId(null)}>
                        <span className="sh"><Icon name="x" /></span>
                        ביטול
                      </button>
                    </div>
                  </div>
                ) : (
                  <div className="my-sh" key={p.preferenceId}>
                    <span className="bubble"><Icon name={featureIcon(p.name)} /></span>
                    <div className="grow">
                      <h3 style={{ fontSize: 18 }}>{p.name}</h3>
                      {(p.description || p.category) && (
                        <div className="hint">
                          {p.category && <span className="chip" style={{ marginInlineEnd: 8 }}>{p.category}</span>}
                          {p.description}
                        </div>
                      )}
                    </div>
                    <div className="actions">
                      <button
                        className="iconbtn"
                        type="button"
                        onClick={() => startEdit(p)}
                        aria-label={`עריכת ${p.name}`}
                        title="עריכה"
                      >
                        <Icon name="edit" />
                      </button>
                      <button
                        className="iconbtn"
                        type="button"
                        disabled={busy}
                        onClick={() => remove(p)}
                        aria-label={`מחיקת ${p.name}`}
                        title="מחיקה"
                      >
                        <Icon name="x" />
                      </button>
                    </div>
                  </div>
                )
              )}
            </div>
          )}
        </div>
      </div>
    </section>
  );
}

export default PreferencesAdminScreen;