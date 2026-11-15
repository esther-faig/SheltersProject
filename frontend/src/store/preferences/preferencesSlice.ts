import { createSlice } from '@reduxjs/toolkit';
import { initialState } from './preferencesState';
import {
  fetchPreferences,
  createPreference,
  updatePreference,
  deletePreference,
} from './preferencesThunks';

const preferencesSlice = createSlice({
  name: 'preferences',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchPreferences.pending, (state) => { state.loading = true; state.error = null; })
      .addCase(fetchPreferences.fulfilled, (state, action) => {
        state.loading = false;
        state.items = action.payload;
      })
      .addCase(fetchPreferences.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message ?? 'Error';
      })
      .addCase(createPreference.fulfilled, (state, action) => {
        state.items.push(action.payload);
      })
      .addCase(updatePreference.fulfilled, (state, action) => {
        state.items = state.items.map((p) =>
          p.preferenceId === action.payload.preferenceId ? action.payload : p
        );
      })
      .addCase(deletePreference.fulfilled, (state, action) => {
        state.items = state.items.filter((p) => p.preferenceId !== action.payload);
      });
  },
});

export default preferencesSlice.reducer;