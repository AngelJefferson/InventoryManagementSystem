import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getProducts, getLowStock } from '../api/productService';
import { getCategories } from '../api/categoryService';
import { getSuppliers } from '../api/supplierService';

export default function Dashboard() {
  const [stats, setStats] = useState({ equipos: 0, categorias: 0, proveedores: 0 });
  const [lowStock, setLowStock] = useState([]);

  useEffect(() => {
    Promise.all([
      getProducts().then((r) => setStats((s) => ({ ...s, equipos: r.data.length }))),
      getCategories().then((r) => setStats((s) => ({ ...s, categorias: r.data.length }))),
      getSuppliers().then((r) => setStats((s) => ({ ...s, proveedores: r.data.length }))),
      getLowStock(5).then((r) => setLowStock(r.data)).catch(() => {}),
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
      {lowStock.length > 0 && (
        <div className="card">
          <h2>Equipos con stock bajo</h2>
          <table className="table">
            <thead>
              <tr>
                <th>Equipo</th>
                <th>SKU</th>
                <th>Stock</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {lowStock.map((p) => (
                <tr key={p.id}>
                  <td>{p.name}</td>
                  <td>{p.sku}</td>
                  <td className="text-danger">{p.stockQuantity}</td>
                  <td><Link to={`/equipos/${p.id}/inventario`}>Ajustar</Link></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
