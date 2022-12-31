import { useContext } from 'react';
import { Link } from 'react-router-dom';
import { AppContext } from './AppContext';
import './NavBar.scss';

export default function NavBar() {
  const appContext = useContext(AppContext);

  return <nav>
    <Link to={'/'}>GoldSongLib</Link>

    <ul className='menu'>
      {appContext.user && 
        <>
          <li><Link to={'/songs'}>Songs</Link></li>
          <li><Link to={'/worship-orders'}>Worship Orders</Link></li>
        </>
      }
    </ul>

    {appContext.user
      ? <span>{appContext.user.username}</span>
      : <Link to="/login">Sign In</Link>}
  </nav>
}