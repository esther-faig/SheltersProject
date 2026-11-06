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


function App() {
    return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<HomePage />} />
        <Route path="/search" element={<SearchShelterPage />} />
        <Route path="/results/:mode" element={<ResultsScreen />} />
        <Route path="/shelter/:id" element={<ShelterDetailsScreen />} />
        <Route path="/register" element={<RegisterScreen />} />
        <Route path="/share" element={<ShareShelterScreen />} />
        <Route path="/profile" element={<ProfileScreen />} />
        <Route path="/admin" element={<AdminScreen />} />
        <Route path="*" element={<NotFound />} />
      </Route>
    </Routes>
    );
}

export default App;