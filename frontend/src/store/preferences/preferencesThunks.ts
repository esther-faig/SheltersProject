import { createAsyncThunk } from '@reduxjs/toolkit';
import { request } from '../../api/http';
import type { Preference } from './preferencesState';

export const fetchPreferences = createAsyncThunk('preferences/fetchPreferences', async () => {
  return request<Preference[]>('/Preferences');
});