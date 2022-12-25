import { Link } from 'react-router-dom';
import './NavBar.scss';

export default function NavBar() {
  return <nav>

    <ul className='menu'>
      <li><Link to={'/'}>GoldSongLib</Link></li>
      <li><Link to={'/songs'}>Songs</Link></li>
      <li><Link to={'/worship-orders'}>Worship Orders</Link></li>
    </ul>
  </nav>
}