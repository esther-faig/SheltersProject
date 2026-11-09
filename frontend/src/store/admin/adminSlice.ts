import { createSlice } from '@reduxjs/toolkit';
import { initialState } from './adminState';
import { fetchPendingShelters, reviewShelter } from './adminThunks';
import { logout } from '../auth/authThunks';

const adminSlice = createSlice({
  name: 'admin',
  initialState,
  reducers: {},
//   Reducer היא פונקציה שאחראית לעדכן את מצב הנתונים (State) של האפליקציה.
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
// action – המידע שהגיע יחד עם הפעולה.
// addCase היא פונקציה שמשתמשים בה בתוך extraReducers, כדי להגדיר מה יקרה כאשר פעולה מסוימת מתרחשת.
export default adminSlice.reducer;