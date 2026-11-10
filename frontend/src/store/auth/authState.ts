import { getToken } from '../../api/http';

export interface AuthState {
  token: string | null;
  loading: boolean;
  error: string | null;
}

export const initialState: AuthState = {
  token: getToken(),     // כדי שרענון הדף לא ינתק את המשתמשת
  loading: false,
  error: null,
};