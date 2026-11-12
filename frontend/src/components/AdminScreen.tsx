
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Icon, { featureIcon } from './Icon';
import ShelterIllustration from './ShelterIllustration';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchPendingShelters, reviewShelter } from '../store/admin/adminThunks';

function AdminScreen() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { items, loading, error } = useAppSelector((state) => state.admin);

  useEffect(() => {
    dispatch(fetchPendingShelters());
  }, [dispatch]);

  function reject(id: number) {
    const reason = window.prompt('סיבת הדחייה:');
    if (!reason) return;

    dispatch(
      reviewShelter({
        id,
        status: 'Rejected',
        rejectionReason: reason,
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
          <div className="card empty err">שגיאה: {error}</div>
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
                  {shelter.files?.length > 0 ? (
                    <img
                      src={shelter.files[0]}
                      alt={shelter.name}
                      style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                    />
                  ) : (
                    <ShelterIllustration />
                  )}
                </div>

                <div style={{ flex: 1, minWidth: 200 }}>
                  <h3 style={{ fontSize: 19, marginBottom: 6 }}>{shelter.name}</h3>

                  <div className="meta">
                    <span><Icon name="shield" />{shelter.shelterType}</span>
                    <span><Icon name="pin" />{shelter.address}</span>
                  </div>

                  <div className="chips">
                    {shelter.features?.length > 0 ? (
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
