import { ReactNode, useContext, useEffect, useState } from 'react';
import { Navigate, Route, RouteProps, Routes} from 'react-router-dom';
import AddSong from './AddSong';
import AddWorshipOrder from './AddWorshipOrder';
import './App.css'
import { AppContext, AppContextValues } from './AppContext';
import Home from './Home';
import Loading from './Loading';
import Login from './Login';
import Logout from './Logout';
import NavBar from './NavBar';
import { getCurrentUser, getSongs } from './services/apiClient';
import Songs from './Songs';
import { Song, User } from './types';
import WorshipOrders from './WorshipOrders';

function App() {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [songs, setSongs] = useState<Song[] | null>(null);

  useEffect(() => {
    async function loadAsync() {
      const user = await getCurrentUser();
      setUser(user);

      if (user) {
        await initializeUserSession();
      }

      setLoading(false);
    }

    loadAsync();
  }, []);

  function handleLoggedIn(user: User) {

    initializeUserSession();
  }

  function handleLoggedOut() {
    setSongs([]);
  }

  async function initializeUserSession() {
    setLoading(true);
    const songs = await getSongs();
    setSongs(songs);
    setLoading(false);
  }

  if (loading) return <Loading />;

  const appContextValues: AppContextValues = {
    user,
    songs,
    refreshSongs: async () => {
      const songs = await getSongs();
      setSongs(songs);
    }
  }

  return (<>
  <AppContext.Provider value={appContextValues}>
    <NavBar />

    <div className="content">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login onLoggedIn={handleLoggedIn} />} />
        <Route path="/logout" element={<Logout onLoggedOut={handleLoggedOut} />} />
        <Route path="/songs" element={<AuthRequired><Songs /></AuthRequired>} />
        <Route path="/songs/add" element={<AuthRequired><AddSong /></AuthRequired>} />
        <Route path="/worship-orders" element={<AuthRequired><WorshipOrders /></AuthRequired>} />
        <Route path="/worship-orders/add" element={<AuthRequired><AddWorshipOrder /></AuthRequired>} />
      </Routes>
    </div>
  </AppContext.Provider>

  </>)
}

export default App

type AuthRequiredProps = {
  children: ReactNode | ReactNode[]
};

function AuthRequired({ children }: AuthRequiredProps): JSX.Element {
  const appContext = useContext(AppContext);

  if (!appContext.user) return <Navigate to="/login" replace />

  return <>{children}</>
}
