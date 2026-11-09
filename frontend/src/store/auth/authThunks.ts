import { createAsyncThunk } from '@reduxjs/toolkit';
import { request, saveToken, clearToken } from '../../api/http';

export const register = createAsyncThunk(
  'auth/register',
  async (data: { fullName: string; email: string; password: string }) => {
    await request<void>('/User/register', { method: 'POST', body: JSON.stringify(data) });
  }
);

export const login = createAsyncThunk(
  'auth/login',
  async (data: { email: string; password: string }) => {
    const res = await request<{ token: string; expiresAt: string }>('/User/login', {
      method: 'POST',
      body: JSON.stringify(data),
    });
    saveToken(res.token);
    return res.token;
  }
);

export const logout = createAsyncThunk('auth/logout', async () => {
  clearToken();
});