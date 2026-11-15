import type { UserProfile } from '../profile/profileState';

export interface UsersState {
  items: UserProfile[];
  loading: boolean;
  error: string | null;
}
export const initialState: UsersState = { items: [], loading: false, error: null };