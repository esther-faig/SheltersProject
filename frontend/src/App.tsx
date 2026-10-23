import React from 'react';
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

function App() {
    return (
        <div>
            {/* <ResultsScreen/> */}
            <ShelterCard 
                name="מקלט מרכזי" 
                distance="500 מטר" 
                minutes={5} 
                features={["נגיש", "תאורת חירום"]} 
                isRecommended={true} 
            />
        </div>
    );
}

export default App;