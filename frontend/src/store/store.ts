// אחראי לחבר את כל החלקים יחד

import { configureStore } from '@reduxjs/toolkit';
import authReducer from './auth/authSlice';
import preferencesReducer from './preferences/preferencesSlice';
import profileReducer from './profile/profileSlice';
import sheltersReducer from './shelters/sheltersSlice';
import adminReducer from './admin/adminSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    preferences: preferencesReducer,
    profile: profileReducer,
    shelters: sheltersReducer,
    admin: adminReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;