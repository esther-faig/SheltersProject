import { request } from './http';

export type AlertType = 'Alarm' | 'Warning';
export type TravelMode = 'WALKING' | 'DRIVING';

export interface ShelterSearchResult {
  shelterId: number;
  name: string;
  address: string;
  shelterType: string;
  city: string;
  distanceMeters: number;
  travelTimeSeconds: number;
  matchScore: number;
  isRecommended: boolean;
  isEstimate: boolean;
  matchReasons: string[];
  features: { preferenceId: number; name: string; isAvailable: boolean | null }[];
  files: string[];
}

export interface ShelterSearchResponse {
  results: ShelterSearchResult[];
  hasResults: boolean;
  message: string;
  alertType: string;
  maxSecondsToReach: number;
  offerDrivingSearch: boolean;
  emergencyGuidance: {
    title: string;
    instructions: string; // שורות מופרדות ב-\n
    source: string;
    sourceUrl: string;
  } | null;
}

// הטוקן (אם קיים) נשלח אוטומטית מ-request, ולכן משתמש רשום מקבל התאמה להעדפות
export function searchShelters(body: {
  latitude: number;
  longitude: number;
  travelMode: TravelMode;
  alertType?: AlertType;
}) {
  return request<ShelterSearchResponse>('/ShelterSearch', {
    method: 'POST',
    body: JSON.stringify(body),
  });
}