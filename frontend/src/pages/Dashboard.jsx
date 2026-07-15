import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getProducts } from '../api/productService';
import { getCategories } from '../api/categoryService';
import { getSuppliers } from '../api/supplierService';

export default function Dashboard() {
  const [stats, setStats] = useState({ equipos: 0, categorias: 0, proveedores: 0 });

  useEffect(() => {
    Promise.all([
      getProducts().then((r) => setStats((s) => ({ ...s, equipos: r.data.length }))),
      getCategories().then((r) => setStats((s) => ({ ...s, categorias: r.data.length }))),
      getSuppliers().then((r) => setStats((s) => ({ ...s, proveedores: r.data.length }))),
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
          <p>Categorías</p>
          <Link to="/categorias">Ver todas</Link>
        </div>
        <div className="stat-card">
          <h3>{stats.proveedores}</h3>
          <p>Proveedores</p>
          <Link to="/proveedores">Ver todos</Link>
        </div>
      </div>
    </div>
  );
}
