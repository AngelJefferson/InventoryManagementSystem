import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getProduct } from '../api/productService';
import { getStock, adjustStock } from '../api/inventoryService';

export default function Inventario() {
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
      setMessage('Stock ajustado correctamente');
      getStock(id).then((r) => setStock(r.data));
      setForm({ quantity: 1, type: 'Entry', reference: '' });
    } catch (err) {
      setMessage(`Error: ${err.response?.data?.title || err.message}`);
    }
  };

  if (!product) return <div className="loading">Cargando...</div>;

  return (
    <div>
      <h1>Inventario: {product.name}</h1>
      <p className="text-muted">SKU: {product.sku} | Stock actual: <strong>{stock?.quantity ?? 0}</strong></p>

      <div className="card">
        <h2>Ajustar stock</h2>
        {message && <div className={`alert ${message.startsWith('Error') ? 'alert-error' : 'alert-success'}`}>{message}</div>}
        <form onSubmit={handleAdjust}>
          <div className="form-row">
            <div className="form-group">
              <label>Tipo</label>
              <select value={form.type} onChange={(e) => setForm({ ...form, type: e.target.value })}>
                <option value="Entry">Entrada</option>
                <option value="Exit">Salida</option>
              </select>
            </div>
            <div className="form-group">
              <label>Cantidad</label>
              <input type="number" min="1" value={form.quantity}
                onChange={(e) => setForm({ ...form, quantity: parseInt(e.target.value) || 1 })} required />
            </div>
          </div>
          <div className="form-group">
            <label>Referencia / Motivo</label>
            <input value={form.reference} onChange={(e) => setForm({ ...form, reference: e.target.value })} required />
          </div>
          <button type="submit" className="btn btn-primary">Ajustar</button>
        </form>
      </div>

      <div className="actions" style={{ marginTop: 16 }}>
        <button onClick={() => navigate(`/equipos/${id}/movimientos`)} className="btn">Ver movimientos</button>
        <button onClick={() => navigate('/equipos')} className="btn">Volver a equipos</button>
      </div>
    </div>
  );
}
