import { createSlice } from '@reduxjs/toolkit';
import { initialState } from './usersState';
import { fetchUsers } from './usersThunks';
import { logout } from '../auth/authThunks';

const usersSlice = createSlice({
  name: 'users',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchUsers.pending, (state) => { state.loading = true; state.error = null; })
      .addCase(fetchUsers.fulfilled, (state, action) => {
        state.loading = false;
        state.items = action.payload;
      })
      .addCase(fetchUsers.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message ?? 'Error';
      })
      .addCase(logout.fulfilled, () => initialState);
  },
});

export default usersSlice.reducer;