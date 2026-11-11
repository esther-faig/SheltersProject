import { Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import HomePage from './components/HomePage';
import LoginScreen from './components/LoginScreen';
import RegisterScreen from './components/RegisterScreen';
import SearchShelterPage from './components/SearchShelterPage';
import ResultsScreen from './components/ResultsScreen';
import ShelterDetailsScreen from './components/ShelterDetailsScreen';
import ShareShelterScreen from './components/ShareShelterScreen';
import ProfileScreen from './components/ProfileScreen';
import AdminScreen from './components/AdminScreen';
import RequireAuth from './components/RequireAuth';
import NotFound from './components/NotFound';
import ChatScreen from './components/ChatScreen';
function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<HomePage />} />
        <Route path="/login" element={<LoginScreen />} />
        <Route path="/register" element={<RegisterScreen />} />
        <Route path="/search" element={<SearchShelterPage />} />
        <Route path="/results/:mode" element={<ResultsScreen />} />
        <Route path="/shelter/:id" element={<ShelterDetailsScreen />} />
        
        <Route path="/chat" element={<ChatScreen />} />
        
        {/* מסכים שדורשים התחברות */}
        <Route element={<RequireAuth />}>
          <Route path="/share" element={<ShareShelterScreen />} />
          <Route path="/profile" element={<ProfileScreen />} />
        </Route>

        {/* מסך מנהל */}
        <Route element={<RequireAuth admin />}>
          <Route path="/admin" element={<AdminScreen />} />
        </Route>

        <Route path="*" element={<NotFound />} />
      </Route>
    </Routes>
  );
}

export default App;