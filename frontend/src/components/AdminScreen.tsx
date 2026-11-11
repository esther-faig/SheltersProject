
import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { fetchPendingShelters, reviewShelter } from '../store/admin/adminThunks';

function AdminScreen() {
  const dispatch = useAppDispatch();
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
    return <div className="admin-message">טוען מקלטים...</div>;
  }

  if (error) {
    return <div className="admin-message admin-error">שגיאה: {error}</div>;
  }

  return (
    <div className="screen admin-screen">
      <div className="admin-header">
        <div>
          <h2 className="title">ניהול ואישור מקלטים</h2>
          <p className="admin-subtitle">
            מקלט מופיע בתוצאות החיפוש רק אחרי שאישרתם אותו.
          </p>
        </div>
      </div>

      <div className="admin-list">
        {items.length > 0 ? (
          items.map((shelter) => (
            <div className="admin-pending-card" key={shelter.shelterId}>
              <div className="admin-thumb">
                {shelter.files?.length > 0 ? (
                  <img src={shelter.files[0]} alt={shelter.name} />
                ) : (
                  <span>🏠</span>
                )}
              </div>

              <div className="admin-shelter-info">
                <h3>{shelter.name}</h3>

                <div className="admin-meta">
                  <span>🛡️ {shelter.shelterType}</span>
                  <span>📍 {shelter.address}</span>
                </div>

                <div className="admin-features">
                  {shelter.features?.length > 0 ? (
                    shelter.features
                      .filter((feature) => feature.isAvailable)
                      .map((feature) => (
                        <span className="admin-feature-chip" key={feature.preferenceId}>
                          {feature.name}
                        </span>
                      ))
                  ) : (
                    <span className="admin-no-features">
                      לא סומנו מאפיינים
                    </span>
                  )}
                </div>
              </div>

              <div className="admin-actions">
                <button
                  className="admin-approve-btn"
                  onClick={() =>
                    dispatch(
                      reviewShelter({
                        id: shelter.shelterId,
                        status: 'Approved',
                      })
                    )
                  }
                >
                  ✓ אישור
                </button>

                <button
                  className="admin-reject-btn"
                  onClick={() => reject(shelter.shelterId)}
                >
                  ✕ דחייה
                </button>
              </div>
            </div>
          ))
        ) : (
          <div className="admin-empty-card">
            <div className="admin-empty-icon">🛡️</div>
            <h3>אין מקלטים שממתינים לאישור</h3>
            <p>הכל מעודכן. כל הכבוד!</p>
          </div>
        )}
      </div>
    </div>
  );
}

export default AdminScreen;
