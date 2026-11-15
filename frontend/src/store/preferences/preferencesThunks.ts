import { createAsyncThunk } from '@reduxjs/toolkit';
import { request } from '../../api/http';
import type { Preference } from './preferencesState';

export const fetchPreferences = createAsyncThunk('preferences/fetchPreferences', async () => {
  return request<Preference[]>('/Preferences');
});

type PreferenceInput = Omit<Preference, 'preferenceId'>;

// ---- ניהול העדפות (מנהל בלבד) ----
export const createPreference = createAsyncThunk(
  'preferences/createPreference',
  async (data: PreferenceInput) => {
    return request<Preference>('/Preferences', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  }
);

export const updatePreference = createAsyncThunk(
  'preferences/updatePreference',
  async (data: Preference) => {
    await request<void>(`/Preferences/${data.preferenceId}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
    return data;          // משתמשים במה ששלחנו כדי לעדכן את הרשימה
  }
);

export const deletePreference = createAsyncThunk(
  'preferences/deletePreference',
  async (id: number) => {
    await request<void>(`/Preferences/${id}`, { method: 'DELETE' });
    return id;            // מחזירים id ומסננים אותו מהרשימה
  }
);