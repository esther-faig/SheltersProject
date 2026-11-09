import { createAsyncThunk } from '@reduxjs/toolkit';
import { request } from '../../api/http';
import type { UserProfile, UserPreference } from './profileState';

export const fetchMe = createAsyncThunk('profile/fetchMe', async () => {
  return request<UserProfile>('/User/me');
});

// השרת מחליף את כל ההעדפות: מה שלא נשלח נמחק. לכן שולחים תמיד את הרשימה המלאה
export const saveMyPreferences = createAsyncThunk(
  'profile/saveMyPreferences',
  async (preferences: UserPreference[]) => {
    await request<void>('/UserPreference/me', {
      method: 'PUT',
      body: JSON.stringify({ preferences }),
    });
    return preferences;     // הופך ל-payload, כמו ש-deleteProduct מחזיר id
  }
);