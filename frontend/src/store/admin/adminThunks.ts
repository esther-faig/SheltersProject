import { createAsyncThunk } from '@reduxjs/toolkit';
import { request } from '../../api/http';
import type { AdminShelter } from './adminState';

export const fetchPendingShelters = createAsyncThunk('admin/fetchPendingShelters', async () => {
  return request<AdminShelter[]>('/Shelter/admin?status=Pending');
});

export const reviewShelter = createAsyncThunk(
  'admin/reviewShelter',
  async (data: { id: number; status: 'Approved' | 'Rejected'; rejectionReason?: string }) => {
    await request(`/Shelter/${data.id}/status`, {
      method: 'PUT',
      body: JSON.stringify({ status: data.status, rejectionReason: data.rejectionReason }),
    });
    return data.id;       // כמו deleteProduct: מחזירים id, ומסננים אותו מהרשימה
  }
);