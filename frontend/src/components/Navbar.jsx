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
        <NavLink to="/">InventarioCAID</NavLink>
      </div>
      <div className="nav-links">
        <NavLink to="/" end>Inicio</NavLink>
        <NavLink to="/equipos">Equipos</NavLink>
        <NavLink to="/categorias">Categorías</NavLink>
        <NavLink to="/proveedores">Proveedores</NavLink>
        <NavLink to="/empleados">Empleados</NavLink>
      </div>
      <div className="nav-user">
        <span>{user?.username} ({user?.role})</span>
        <button onClick={handleLogout} className="btn-link">Cerrar Sesión</button>
      </div>
    </nav>
  );
}
