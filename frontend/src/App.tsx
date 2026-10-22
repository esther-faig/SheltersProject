import React from 'react';
import HomeScreen from './components/HomeScreen';
import SearchScreen from './components/SearchScreen';
import ResultsScreen from './components/ResultsScreen';
import RegisterScreen from './components/RegisterScreen';
import ShareShelterScreen from './components/ShareShelterScreen';
import ProfileScreen from './components/ProfileScreen';

function App() {
  return (
    <div dir="rtl">
      <HomeScreen />
      <hr />
      <SearchScreen />
      <hr />
      <ResultsScreen />
      <hr />
      <RegisterScreen />
      <hr />
      <ShareShelterScreen />
      <hr />
      <ProfileScreen />
    </div>
  );
}

export default App;
