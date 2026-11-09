export interface AdminFeature { preferenceId: number; name: string; isAvailable: boolean; }
export interface AdminShelter {
  shelterId: number;
  name: string;
  address: string;
  shelterType: string;
  status: string;
  features: AdminFeature[];
  files: string[];
}
export interface AdminState {
  items: AdminShelter[];
  loading: boolean;
  error: string | null;
}
export const initialState: AdminState = { items: [], loading: false, error: null };