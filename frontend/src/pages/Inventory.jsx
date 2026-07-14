import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getProduct } from '../api/productService';
import { getStock, adjustStock } from '../api/inventoryService';
export default function Inventory() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [product, setProduct] = useState(null);
  const [stock, setStock] = useState(null);
  const [form, setForm] = useState({ quantity: 1, type: 'Entry', reference: '' });
  const [message, setMessage] = useState('');

  useEffect(() => {
    getProduct(id).then((r) => setProduct(r.data));
    getStock(id).then((r) => setStock(r.data)).catch(() => setStock({ quantity: 0 }));
  }, [id]);

  const handleAdjust = async (e) => {
    e.preventDefault();
    try {
      await adjustStock({
        productId: id,
        quantity: Math.abs(form.quantity),
        reference: form.reference,
        isAddition: form.type === 'Entry',
      });
      setMessage('Stock adjusted successfully');
      getStock(id).then((r) => setStock(r.data));
      setForm({ quantity: 1, type: 'Entry', reference: '' });
    } catch (err) {
      setMessage(`Error: ${err.response?.data?.title || err.message}`);
    }
  };

  if (!product) return <div className="loading">Loading...</div>;

  return (
    <div className="inventory-page">
      <h1>Inventory: {product.name}</h1>
      <p className="text-muted">SKU: {product.sku} | Current Stock: <strong>{stock?.quantity ?? 0}</strong></p>

      <div className="card">
        <h2>Adjust Stock</h2>
        {message && <div className="alert">{message}</div>}
        <form onSubmit={handleAdjust}>
          <div className="form-row">
            <div className="form-group">
              <label>Type</label>
              <select value={form.type} onChange={(e) => setForm({ ...form, type: e.target.value })}>
                <option value="Entry">Entry</option>
                <option value="Exit">Exit</option>
              </select>
            </div>
            <div className="form-group">
              <label>Quantity</label>
              <input type="number" min="1" value={form.quantity}
                onChange={(e) => setForm({ ...form, quantity: parseInt(e.target.value) || 1 })} required />
            </div>
          </div>
          <div className="form-group">
            <label>Reference</label>
            <input value={form.reference} onChange={(e) => setForm({ ...form, reference: e.target.value })} required />
          </div>
          <button type="submit" className="btn btn-primary">Adjust</button>
        </form>
      </div>

      <button onClick={() => navigate(`/products/${id}/movements`)} className="btn">View Movements</button>
      <button onClick={() => navigate('/products')} className="btn" style={{ marginLeft: 8 }}>Back to Products</button>
    </div>
  );
}
