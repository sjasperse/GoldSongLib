import { ReactNode, useContext, useEffect, useState } from 'react';
import { Navigate, Route, RouteProps, Routes} from 'react-router-dom';
import AddSong from './AddSong';
import AddWorshipOrder from './AddWorshipOrder';
import './App.css'
import { AppContext } from './AppContext';
import Home from './Home';
import Loading from './Loading';
import Login from './Login';
import Logout from './Logout';
import NavBar from './NavBar';
import { getCurrentUser } from './services/apiClient';
import Songs from './Songs';
import { User } from './types';
import WorshipOrders from './WorshipOrders';

function App() {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    getCurrentUser()
      .then(user => {
        if (user) setUser(user);
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  if (loading) return <Loading />;

  return (<>
  <AppContext.Provider value={{
    user: user
  }}>
    <NavBar />

    <div className="content">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login setUser={setUser} />} />
        <Route path="/logout" element={<Logout setUser={setUser} />} />
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
