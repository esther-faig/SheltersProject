import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Icon, { featureIcon } from './Icon';
import ShelterIllustration from './ShelterIllustration';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchPendingShelters, reviewShelter } from '../store/admin/adminThunks';
import type { AdminShelter } from '../store/admin/adminState';

const IMAGES_URL = import.meta.env.VITE_IMAGES_URL as string;

// תמונה עם גיבוי: אם הקובץ לא נטען, מציגים את האיור במקום אייקון שבור
function ShelterThumb({ shelter }: { shelter: AdminShelter }) {
  const [failed, setFailed] = useState(false);
  const file = shelter.files?.[0];

  if (!file || failed) return <ShelterIllustration />;

  return (
    <img
      src={`${IMAGES_URL}/${file}`}
      alt={shelter.name}
      onError={() => setFailed(true)}
      style={{ width: '100%', height: '100%', objectFit: 'cover' }}
    />
  );
}

function AdminScreen() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { items, loading, error } = useAppSelector((state) => state.admin);

  useEffect(() => {
    dispatch(fetchPendingShelters());
  }, [dispatch]);

  function reject(id: number) {
    const reason = window.prompt('סיבת הדחייה:');
    if (!reason || !reason.trim()) return;

    dispatch(
      reviewShelter({
        id,
        status: 'Rejected',
        rejectionReason: reason.trim(),
      })
    );
  }

  if (loading) {
    return (
      <section className="screen active">
        <div className="wrap inner" style={{ maxWidth: 900 }}>
          <div className="card empty">טוען מקלטים...</div>
        </div>
      </section>
    );
  }

  if (error) {
    return (
      <section className="screen active">
        <div className="wrap inner" style={{ maxWidth: 900 }}>
          <div className="card empty">
            <span className="bubble"><Icon name="alert" /></span>
            <h3 style={{ fontSize: 22 }}>שגיאה בטעינת המקלטים</h3>
            <p className="err">{error}</p>
            <div className="actions" style={{ justifyContent: 'center', marginTop: 16 }}>
              <button
                type="button"
                className="btn primary"
                onClick={() => navigate('/admin')}
              >
                <span className="sh"><Icon name="check" /></span>
                נסי שוב
              </button>
            </div>
          </div>
        </div>
      </section>
    );
  }

  return (
    <section className="screen active">
      <div className="wrap inner" style={{ maxWidth: 900 }} id="adminBody">
        <div className="phead">
          <button
            className="iconbtn"
            type="button"
            onClick={() => navigate(-1)}
            aria-label="חזרה"
          >
            <Icon name="arrow-r" />
          </button>
          <div>
            <h2>ניהול ואישור מקלטים</h2>
            <div className="sub">
              מקלט מופיע בתוצאות החיפוש רק אחרי שאישרתם אותו.
            </div>
          </div>
        </div>

        <div className="stack">
          {items.length > 0 ? (
            items.map((shelter) => (
              <div className="card pend-item" key={shelter.shelterId}>
                <div className="thumb">
                  <ShelterThumb shelter={shelter} />
                </div>

                <div style={{ flex: 1, minWidth: 200 }}>
                  <h3 style={{ fontSize: 19, marginBottom: 6 }}>{shelter.name}</h3>

                  <div className="meta">
                    <span><Icon name="shield" />{shelter.shelterType}</span>
                    <span><Icon name="pin" />{shelter.address || 'כתובת לא צוינה'}</span>
                  </div>

                  <div className="chips">
                    {shelter.features?.some((f) => f.isAvailable) ? (
                      shelter.features
                        .filter((feature) => feature.isAvailable)
                        .map((feature) => (
                          <span className="chip" key={feature.preferenceId}>
                            <Icon name={featureIcon(feature.name)} />
                            {feature.name}
                          </span>
                        ))
                    ) : (
                      <span className="hint">לא סומנו מאפיינים</span>
                    )}
                  </div>
                </div>

                <div className="actions" style={{ flexDirection: 'column' }}>
                  <button
                    className="btn small green"
                    onClick={() =>
                      dispatch(
                        reviewShelter({
                          id: shelter.shelterId,
                          status: 'Approved',
                        })
                      )
                    }
                  >
                    <span className="sh"><Icon name="check" /></span>
                    אישור
                  </button>

                  <button
                    className="btn small"
                    onClick={() => reject(shelter.shelterId)}
                  >
                    <span className="sh"><Icon name="x" /></span>
                    דחייה
                  </button>
                </div>
              </div>
            ))
          ) : (
            <div className="card empty">
              <span className="bubble"><Icon name="shieldcheck" /></span>
              <h3 style={{ fontSize: 22 }}>אין מקלטים שממתינים לאישור</h3>
              <p style={{ color: 'var(--ink2)' }}>הכל מעודכן. כל הכבוד!</p>
            </div>
          )}
        </div>
      </div>
    </section>
  );
}

export default AdminScreen;