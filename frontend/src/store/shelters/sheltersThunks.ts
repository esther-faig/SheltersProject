import { createAsyncThunk } from '@reduxjs/toolkit';
import { request } from '../../api/http';
import type { SharedShelter } from '../profile/profileState';

export const fetchMyShelters = createAsyncThunk('shelters/fetchMyShelters', async () => {
  return request<SharedShelter[]>('/Shelter/my');
});

// שיתוף מקלט חדש: FormData, לא JSON (בגלל התמונות)
export const addShelter = createAsyncThunk('shelters/addShelter', async (form: FormData) => {
  await request('/Shelter', { method: 'POST', body: form });
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