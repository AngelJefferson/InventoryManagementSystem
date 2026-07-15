import { Outlet } from 'react-router-dom';
import Navbar from './Navbar';

export default function Layout() {
  return (
    <div className="layout">
      <Navbar />
      <main className="main-content">
        <Outlet />
      </main>
      <footer className="footer">
        Sistema de Inventario de Equipos Tecnológicos &copy; {new Date().getFullYear()}
      </footer>
    </div>
  );
}
