import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import HomePage from './components/HomePage';
import AdminScreen from './components/AdminScreen';
import NoSheltersScreen from './components/NoSheltersScreen';
import PreferenceRow from './components/PreferenceRow';
import ProfileScreen from './components/ProfileScreen';
import RegisterScreen from './components/RegisterScreen';
import ResultsScreen from './components/ResultsScreen';
import SearchShelterPage from './components/SearchShelterPage';
import ShareShelterScreen from './components/ShareShelterScreen';
import ShelterCard from './components/ShelterCard';
import ShelterDetailsScreen from './components/ShelterDetailsScreen';
import FeatureTag from './components/FeatureTag';
import NotFound from './components/NotFound';
import RequireAuth from './components/RequireAuth';
import LoginScreen from './components/LoginScreen';

function App() {
    return (
<Route element={<Layout />}>
  <Route path="/" element={<HomePage />} />
  <Route path="/login" element={<LoginScreen />} />
  <Route path="/register" element={<RegisterScreen />} />
  <Route path="/search" element={<SearchShelterPage />} />
  <Route path="/results/:mode" element={<ResultsScreen />} />
  <Route path="/shelter/:id" element={<ShelterDetailsScreen />} />

  <Route element={<RequireAuth />}>
    <Route path="/share" element={<ShareShelterScreen />} />
    <Route path="/profile" element={<ProfileScreen />} />
  </Route>
  <Route element={<RequireAuth admin />}>
    <Route path="/admin" element={<AdminScreen />} />
  </Route>

  <Route path="*" element={<NotFound />} />
</Route>
    );
}

export default App;