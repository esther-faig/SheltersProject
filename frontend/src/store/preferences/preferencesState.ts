export interface Preference {
  preferenceId: number;
  name: string;
  description?: string;
  category?: string;
}
export interface PreferencesState {
  items: Preference[];
  loading: boolean;
  error: string | null;
}
export const initialState: PreferencesState = { items: [], loading: false, error: null };