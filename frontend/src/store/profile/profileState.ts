export interface UserPreference {
  preferenceId: number;
  name?: string;
  priority: number;
  isRequired: boolean;
}
export interface SharedShelter {
  shelterId: number;
  name: string;
  address: string;
  shelterType: string;
  status: string;      // Pending | Approved | Rejected
  isActive: boolean;
  files: string[];  
}
export interface UserProfile {
  userId: number;
  fullName: string;
  email: string;
  preferences: UserPreference[];
  sharedShelters: SharedShelter[];
}
export interface ProfileState {
  user: UserProfile | null;
  loading: boolean;
  error: string | null;
}
export const initialState: ProfileState = { user: null, loading: false, error: null };