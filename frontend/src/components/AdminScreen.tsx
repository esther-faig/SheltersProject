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
    if (!reason) return;       // השרת דורש סיבה בדחייה
    dispatch(reviewShelter({ id, status: 'Rejected', rejectionReason: reason }));
  }

  if (loading) return <p>טוען...</p>;
  if (error) return <p>שגיאה: {error}</p>;

  return (
    <div>
      <h2>ניהול ואישור מקלטים</h2>
      {items.length === 0 ? <p>אין מקלטים הממתינים לאישור.</p> : items.map((s) => (
        <div key={s.shelterId} style={{ border: '1px solid #ccc', padding: 10, marginBottom: 10 }}>
          <h3>{s.name}</h3>
          <p><strong>סוג:</strong> {s.shelterType}</p>
          <p><strong>כתובת:</strong> {s.address}</p>
          <button onClick={() => dispatch(reviewShelter({ id: s.shelterId, status: 'Approved' }))}>אישור</button>
          <button onClick={() => reject(s.shelterId)}>דחייה</button>
        </div>
      ))}
    </div>
  );
}
export default AdminScreen;