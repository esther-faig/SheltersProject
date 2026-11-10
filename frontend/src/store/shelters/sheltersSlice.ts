import { createSlice } from '@reduxjs/toolkit';
import { initialState } from './sheltersState';
import { fetchMyShelters, addShelter, setShelterActive } from './sheltersThunks';
import { logout } from '../auth/authThunks';

const sheltersSlice = createSlice({
  name: 'shelters',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchMyShelters.pending, (state) => { state.loading = true; state.error = null; })
      .addCase(fetchMyShelters.fulfilled, (state, action) => {
        state.loading = false;
        state.items = action.payload;
      })
      .addCase(fetchMyShelters.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message ?? 'Error';
      })
      .addCase(addShelter.pending, (state) => { state.loading = true; state.error = null; })
      .addCase(addShelter.fulfilled, (state) => { state.loading = false; })
      .addCase(addShelter.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message ?? 'Error';
      })
      .addCase(setShelterActive.fulfilled, (state, action) => {
        const index = state.items.findIndex((s) => s.shelterId === action.payload.id);
        if (index !== -1) state.items[index].isActive = action.payload.isActive;
      })
      .addCase(logout.fulfilled, () => initialState);
  },
});

export default sheltersSlice.reducer;