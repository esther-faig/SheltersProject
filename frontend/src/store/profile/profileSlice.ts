import { createSlice } from '@reduxjs/toolkit';
import { initialState } from './profileState';
import { fetchMe, saveMyPreferences } from './profileThunks';
import { logout } from '../auth/authThunks';

const profileSlice = createSlice({
  name: 'profile',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchMe.pending, (state) => { state.loading = true; state.error = null; })
      .addCase(fetchMe.fulfilled, (state, action) => {
        state.loading = false;
        state.user = action.payload;
      })
      .addCase(fetchMe.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message ?? 'Error';
      })
      .addCase(saveMyPreferences.fulfilled, (state, action) => {
        if (state.user) state.user.preferences = action.payload;
      })
      .addCase(saveMyPreferences.rejected, (state, action) => {
        state.error = action.error.message ?? 'Error';
      })
      .addCase(logout.fulfilled, () => initialState);   // בהתנתקות מנקים את המידע האישי
  },
});

export default profileSlice.reducer;