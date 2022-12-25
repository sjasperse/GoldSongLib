import { Route, Routes} from 'react-router-dom';
import AddSong from './AddSong';
import AddWorshipOrder from './AddWorshipOrder';
import './App.css'
import Home from './Home';
import NavBar from './NavBar';
import Songs from './Songs';
import WorshipOrders from './WorshipOrders';

function App() {
  return (<>
    <NavBar />

    <div className="content">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/songs" element={<Songs />} />
        <Route path="/songs/add" element={<AddSong />} />
        <Route path="/worship-orders" element={<WorshipOrders />} />
        <Route path="/worship-orders/add" element={<AddWorshipOrder />} />
      </Routes>
    </div>
  </>)
}

export default App
