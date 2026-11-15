import { createAsyncThunk } from '@reduxjs/toolkit';
import { request } from '../../api/http';
import type { UserProfile } from '../profile/profileState';

// GET api/User: מותר רק למנהל (Admin)
export const fetchUsers = createAsyncThunk('users/fetchUsers', async () => {
  return request<UserProfile[]>('/User');
});