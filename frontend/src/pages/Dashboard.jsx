import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getProducts, getLowStock } from '../api/productService';
import { getCategories } from '../api/categoryService';
import { getSuppliers } from '../api/supplierService';

export default function Dashboard() {
  const [stats, setStats] = useState({ products: 0, categories: 0, suppliers: 0 });
  const [lowStock, setLowStock] = useState([]);

  useEffect(() => {
    Promise.all([
      getProducts().then((r) => setStats((s) => ({ ...s, products: r.data.length }))),
      getCategories().then((r) => setStats((s) => ({ ...s, categories: r.data.length }))),
      getSuppliers().then((r) => setStats((s) => ({ ...s, suppliers: r.data.length }))),
      getLowStock(5).then((r) => setLowStock(r.data)).catch(() => {}),
    ]);
  }, []);

  return (
    <div className="dashboard">
      <h1>Dashboard</h1>
      <div className="stats-grid">
        <div className="stat-card">
          <h3>{stats.products}</h3>
          <p>Products</p>
          <Link to="/products">View all</Link>
        </div>
        <div className="stat-card">
          <h3>{stats.categories}</h3>
          <p>Categories</p>
          <Link to="/categories">View all</Link>
        </div>
        <div className="stat-card">
          <h3>{stats.suppliers}</h3>
          <p>Suppliers</p>
          <Link to="/suppliers">View all</Link>
        </div>
      </div>
      {lowStock.length > 0 && (
        <div className="low-stock-section">
          <h2>Low Stock Products</h2>
          <table className="table">
            <thead>
              <tr>
                <th>Product</th>
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
                  <td><Link to={`/products/${p.id}/inventory`}>Adjust</Link></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
