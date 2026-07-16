import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getProducts } from '../api/productService';
import { getCategories } from '../api/categoryService';
import { getEmployees } from '../api/employeeService';

export default function Dashboard() {
  const [stats, setStats] = useState({ equipos: 0, categorias: 0, empleados: 0 });

  useEffect(() => {
    Promise.all([
      getProducts().then((r) => setStats((s) => ({ ...s, equipos: r.data.length }))),
      getCategories().then((r) => setStats((s) => ({ ...s, categorias: r.data.length }))),
      getEmployees().then((r) => setStats((s) => ({ ...s, empleados: r.data.length }))),
    ]);
  }, []);

  return (
    <div>
      <h1>Panel de Control</h1>
      <div className="stats-grid">
        <div className="stat-card">
          <h3>{stats.equipos}</h3>
          <p>Equipos registrados</p>
          <Link to="/equipos">Ver todos</Link>
        </div>
        <div className="stat-card">
          <h3>{stats.categorias}</h3>
          <p>Categorías (Marcas)</p>
          <Link to="/categorias">Ver todas</Link>
        </div>
        <div className="stat-card">
          <h3>{stats.empleados}</h3>
          <p>Empleados</p>
          <Link to="/empleados">Ver todos</Link>
        </div>
      </div>
    </div>
  );
}
