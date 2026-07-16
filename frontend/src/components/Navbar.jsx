import { useState } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { changePassword } from '../api/authService';

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [showPwModal, setShowPwModal] = useState(false);
  const [menuOpen, setMenuOpen] = useState(false);
  const [pwForm, setPwForm] = useState({ oldPassword: '', newPassword: '', confirmPassword: '' });
  const [pwError, setPwError] = useState('');
  const [pwSuccess, setPwSuccess] = useState('');

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const handleChangePassword = async (e) => {
    e.preventDefault();
    setPwError('');
    setPwSuccess('');
    if (pwForm.newPassword !== pwForm.confirmPassword) {
      setPwError('Las contraseñas no coinciden');
      return;
    }
    if (pwForm.newPassword.length < 6) {
      setPwError('La contraseña debe tener al menos 6 caracteres');
      return;
    }
    try {
      await changePassword(pwForm.oldPassword, pwForm.newPassword);
      setPwSuccess('Contraseña actualizada correctamente');
      setPwForm({ oldPassword: '', newPassword: '', confirmPassword: '' });
      setTimeout(() => setShowPwModal(false), 1500);
    } catch (err) {
      setPwError(err.response?.data?.error || 'Error al cambiar la contraseña');
    }
  };

  return (
    <>
      <nav className="navbar">
        <div className="nav-brand">
          <NavLink to="/">InventarioCAID</NavLink>
        </div>
        <button className="nav-toggle" onClick={() => setMenuOpen(!menuOpen)}>
          {menuOpen ? '✕' : '☰'}
        </button>
        <div className="nav-links">
          <NavLink to="/" end>Inicio</NavLink>
          <NavLink to="/equipos">Equipos</NavLink>
          <NavLink to="/categorias">Categorías</NavLink>
          <NavLink to="/proveedores">Proveedores</NavLink>
          <NavLink to="/empleados">Empleados</NavLink>
        </div>
        <div className="nav-user">
          <span>{user?.username} ({user?.role})</span>
          <button onClick={() => setShowPwModal(true)} className="btn-link">Cambiar Contraseña</button>
          <button onClick={handleLogout} className="btn-link">Cerrar Sesión</button>
        </div>
        <div className={`mobile-menu${menuOpen ? ' open' : ''}`}>
          <NavLink to="/" end onClick={() => setMenuOpen(false)}>Inicio</NavLink>
          <NavLink to="/equipos" onClick={() => setMenuOpen(false)}>Equipos</NavLink>
          <NavLink to="/categorias" onClick={() => setMenuOpen(false)}>Categorías</NavLink>
          <NavLink to="/proveedores" onClick={() => setMenuOpen(false)}>Proveedores</NavLink>
          <NavLink to="/empleados" onClick={() => setMenuOpen(false)}>Empleados</NavLink>
          <div className="nav-mobile-user">
            <span>{user?.username} ({user?.role})</span>
            <button onClick={() => { setShowPwModal(true); setMenuOpen(false); }} className="btn-link">Cambiar Contraseña</button>
            <button onClick={() => { handleLogout(); setMenuOpen(false); }} className="btn-link">Cerrar Sesión</button>
          </div>
        </div>
      </nav>

      {showPwModal && (
        <div style={{
          position: 'fixed', top: 0, left: 0, right: 0, bottom: 0,
          background: 'rgba(0,0,0,0.5)', display: 'flex', alignItems: 'center', justifyContent: 'center',
          zIndex: 1000,
        }}>
          <div className="card" style={{ width: 400, maxWidth: '90vw' }}>
            <h2>Cambiar Contraseña</h2>
            {pwError && <div className="alert alert-error">{pwError}</div>}
            {pwSuccess && <div className="alert alert-success">{pwSuccess}</div>}
            <form onSubmit={handleChangePassword}>
              <div className="form-group">
                <label>Contraseña actual</label>
                <input type="password" value={pwForm.oldPassword}
                  onChange={(e) => setPwForm({ ...pwForm, oldPassword: e.target.value })} required />
              </div>
              <div className="form-group">
                <label>Nueva contraseña</label>
                <input type="password" value={pwForm.newPassword}
                  onChange={(e) => setPwForm({ ...pwForm, newPassword: e.target.value })} required minLength={6} />
              </div>
              <div className="form-group">
                <label>Confirmar nueva contraseña</label>
                <input type="password" value={pwForm.confirmPassword}
                  onChange={(e) => setPwForm({ ...pwForm, confirmPassword: e.target.value })} required />
              </div>
              <div className="form-actions">
                <button type="submit" className="btn btn-primary">Actualizar</button>
                <button type="button" className="btn" onClick={() => { setShowPwModal(false); setPwError(''); setPwSuccess(''); }}>Cancelar</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </>
  );
}
