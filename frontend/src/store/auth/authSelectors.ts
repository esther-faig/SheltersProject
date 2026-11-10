import type { RootState } from '../store';

function readRole(token: string | null): string | null {
  if (!token) return null;
  try {
    const base64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
    const payload = JSON.parse(atob(base64));
    // השרת שם את התפקיד תחת השם הארוך של ClaimTypes.Role
    return payload.role
      ?? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      ?? null;
  } catch {
    return null;
  }
}

export const selectIsLoggedIn = (state: RootState) => state.auth.token !== null;
export const selectIsAdmin = (state: RootState) => readRole(state.auth.token) === 'Admin';