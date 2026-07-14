import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getProducts, deleteProduct } from '../api/productService';

export default function Products() {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);

  const load = () => {
    setLoading(true);
    getProducts().then((r) => setProducts(r.data)).finally(() => setLoading(false));
  };

  useEffect(load, []);

  const handleDelete = async (id) => {
    if (!confirm('Delete this product?')) return;
    await deleteProduct(id);
    load();
  };

  if (loading) return <div className="loading">Loading...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Products</h1>
        <Link to="/products/new" className="btn btn-primary">+ New Product</Link>
      </div>
      <table className="table">
        <thead>
          <tr>
            <th>Name</th>
            <th>SKU</th>
            <th>Price</th>
            <th>Category</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {products.map((p) => (
            <tr key={p.id}>
              <td>{p.name}</td>
              <td>{p.sku}</td>
              <td>${p.price?.amount?.toFixed(2)}</td>
              <td>{p.categoryName}</td>
              <td className="actions">
                <Link to={`/products/${p.id}/edit`} className="btn btn-sm">Edit</Link>
                <Link to={`/products/${p.id}/inventory`} className="btn btn-sm">Stock</Link>
                <Link to={`/products/${p.id}/movements`} className="btn btn-sm">Movements</Link>
                <button onClick={() => handleDelete(p.id)} className="btn btn-sm btn-danger">Delete</button>
              </td>
            </tr>
          ))}
          {products.length === 0 && (
            <tr><td colSpan="5" className="text-center">No products found</td></tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
