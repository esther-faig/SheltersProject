import { request } from './http';
import type { ShelterSearchResult } from './shelterSearch';

export interface MyShelterDetails {
  shelterId: number;
  name: string;
  shelterType: string;
  address: string;
  city: string;
  latitude: number;
  longitude: number;
  maxCapacity: number;
  features: { preferenceId: number; name: string; isAvailable: boolean | null }[];
  files: string[];
}

// ציבורי: רק מקלט מאושר ופעיל
export const getShelter = (id: number) => request<ShelterSearchResult>(`/Shelter/${id}`);

// המקלט שלי לעריכה (גם אם ממתין לאישור)
export const getMyShelter = (id: number) => request<MyShelterDetails>(`/Shelter/my/${id}`);

export const updateMyShelter = (id: number, form: FormData) =>
  request<MyShelterDetails>(`/Shelter/${id}`, { method: 'PUT', body: form });

// תיקון מאפיינים במקלט מיובא
export const updateShelterFeatures = (
  id: number,
  features: { preferenceId: number; isAvailable: boolean }[]
) =>
  request<ShelterSearchResult>(`/Shelter/${id}/features`, {
    method: 'PUT',
    body: JSON.stringify({ features }),
  });