import { NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="navbar">
      <div className="nav-brand">
        <NavLink to="/">InventoryMS</NavLink>
      </div>
      <div className="nav-links">
        <NavLink to="/" end>Dashboard</NavLink>
        <NavLink to="/categories">Categories</NavLink>
        <NavLink to="/products">Products</NavLink>
        <NavLink to="/suppliers">Suppliers</NavLink>
      </div>
      <div className="nav-user">
        <span>{user?.username} ({user?.role})</span>
        <button onClick={handleLogout} className="btn-link">Logout</button>
      </div>
    </nav>
  );
}
