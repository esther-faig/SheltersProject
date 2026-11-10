import type { SharedShelter } from '../profile/profileState';

export interface SheltersState {
  items: SharedShelter[];
  loading: boolean;
  error: string | null;
}
export const initialState: SheltersState = { items: [], loading: false, error: null };