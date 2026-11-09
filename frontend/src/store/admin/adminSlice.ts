import { createSlice } from '@reduxjs/toolkit';
import { initialState } from './adminState';
import { fetchPendingShelters, reviewShelter } from './adminThunks';
import { logout } from '../auth/authThunks';

const adminSlice = createSlice({
  name: 'admin',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchPendingShelters.pending, (state) => { state.loading = true; state.error = null; })
      .addCase(fetchPendingShelters.fulfilled, (state, action) => {
        state.loading = false;
        state.items = action.payload;
      })
      .addCase(fetchPendingShelters.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message ?? 'Error';
      })
      .addCase(reviewShelter.fulfilled, (state, action) => {
        state.items = state.items.filter((s) => s.shelterId !== action.payload);
      })
      .addCase(reviewShelter.rejected, (state, action) => {
        state.error = action.error.message ?? 'Error';
      })
      .addCase(logout.fulfilled, () => initialState);
  },
});

export default adminSlice.reducer;