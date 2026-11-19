import { createAsyncThunk } from '@reduxjs/toolkit';
import { request } from '../../api/http';
import type { SharedShelter } from '../profile/profileState';

export interface NearbyShelter {
  name: string;
  address: string;
  shelterType: string;
  distanceMeters: number;
  detailsVisible: boolean;
}

// saved=false הוא אזהרה (לא שגיאה): המקלט לא נשמר עד שהמשתמש מאשר
export interface AddShelterResult {
  saved: boolean;
  message?: string | null;
  nearbyShelters: NearbyShelter[];
}

export const fetchMyShelters = createAsyncThunk('shelters/fetchMyShelters', async () => {
  return request<SharedShelter[]>('/Shelter/my');
});

// שיתוף מקלט חדש: FormData, לא JSON (בגלל התמונות)
export const addShelter = createAsyncThunk('shelters/addShelter', async (form: FormData) => {
  return request<AddShelterResult>('/Shelter', { method: 'POST', body: form });
});

export const setShelterActive = createAsyncThunk(
  'shelters/setShelterActive',
  async (data: { id: number; isActive: boolean }) => {
    await request(`/Shelter/${data.id}/availability`, {
      method: 'PUT',
      body: JSON.stringify({ isActive: data.isActive }),
    });
    return data;
  }
);

// מחיקת מקלט שלי: השרת מחזיר 204 בלי גוף, ולכן מחזירים את ה-id כדי להסיר אותו מהרשימה
export const deleteShelter = createAsyncThunk('shelters/deleteShelter', async (id: number) => {
  await request(`/Shelter/${id}`, { method: 'DELETE' });
  return id;
});