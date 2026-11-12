// RequireAuth היא שומרת סף שבודקת אם המשתמש מחובר ואם יש לו הרשאה מתאימה, ורק אז מאפשרת להציג את העמוד.
import { Navigate, Outlet } from 'react-router-dom';
import { useAppSelector } from '../store/hooks';
import { selectIsLoggedIn, selectIsAdmin } from '../store/auth/authSelectors';

function RequireAuth({ admin = false }: { admin?: boolean }) {
  const loggedIn = useAppSelector(selectIsLoggedIn);
  const isAdmin = useAppSelector(selectIsAdmin);

  if (!loggedIn) return <Navigate to="/login" replace />;
  if (admin && !isAdmin) return <Navigate to="/" replace />;
  return <Outlet />;
}
export default RequireAuth;