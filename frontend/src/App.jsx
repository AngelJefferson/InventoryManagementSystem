import { Routes, Route, Navigate } from 'react-router-dom';
import Layout from './components/Layout';
import ProtectedRoute from './components/ProtectedRoute';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Equipos from './pages/Equipos';
import EquipoForm from './pages/EquipoForm';
import Categorias from './pages/Categorias';
import CategoriaForm from './pages/CategoriaForm';
import Proveedores from './pages/Proveedores';
import ProveedorForm from './pages/ProveedorForm';
import Empleados from './pages/Empleados';
import EmpleadoForm from './pages/EmpleadoForm';
import './App.css';

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/" element={<ProtectedRoute><Layout /></ProtectedRoute>}>
        <Route index element={<Dashboard />} />
        <Route path="equipos" element={<Equipos />} />
        <Route path="equipos/nuevo" element={<EquipoForm />} />
        <Route path="equipos/:id/editar" element={<EquipoForm />} />
        <Route path="categorias" element={<Categorias />} />
        <Route path="categorias/nueva" element={<CategoriaForm />} />
        <Route path="categorias/:id/editar" element={<CategoriaForm />} />
        <Route path="proveedores" element={<Proveedores />} />
        <Route path="proveedores/nuevo" element={<ProveedorForm />} />
        <Route path="proveedores/:id/editar" element={<ProveedorForm />} />
        <Route path="empleados" element={<Empleados />} />
        <Route path="empleados/nuevo" element={<EmpleadoForm />} />
        <Route path="empleados/:id/editar" element={<EmpleadoForm />} />
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
