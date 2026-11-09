import { createSlice } from '@reduxjs/toolkit';
import { initialState } from './preferencesState';
import { fetchPreferences } from './preferencesThunks';

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
      });
  },
});

export default preferencesSlice.reducer;