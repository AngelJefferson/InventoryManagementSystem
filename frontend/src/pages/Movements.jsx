import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getProduct } from '../api/productService';
import { getMovements } from '../api/inventoryService';

export default function Movements() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [product, setProduct] = useState(null);
  const [movements, setMovements] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getProduct(id).then((r) => setProduct(r.data));
    getMovements(id).then((r) => setMovements(r.data)).finally(() => setLoading(false));
  }, [id]);

  if (loading) return <div className="loading">Loading...</div>;

  return (
    <div>
      <h1>Movements: {product?.name}</h1>
      <p className="text-muted">SKU: {product?.sku}</p>

      <table className="table">
        <thead>
          <tr>
            <th>Date</th>
            <th>Type</th>
            <th>Quantity</th>
            <th>Reason</th>
            <th>User</th>
          </tr>
        </thead>
        <tbody>
          {movements.map((m) => (
            <tr key={m.id}>
              <td>{new Date(m.date).toLocaleString()}</td>
              <td>
                <span className={`badge ${m.type === 'Entry' ? 'badge-success' : 'badge-danger'}`}>
                  {m.type}
                </span>
              </td>
              <td className={m.quantity > 0 ? 'text-success' : 'text-danger'}>
                {m.quantity > 0 ? `+${m.quantity}` : m.quantity}
              </td>
              <td>{m.reason}</td>
              <td>{m.userId}</td>
            </tr>
          ))}
          {movements.length === 0 && (
            <tr><td colSpan="5" className="text-center">No movements recorded</td></tr>
          )}
        </tbody>
      </table>

      <button onClick={() => navigate(`/products/${id}/inventory`)} className="btn">Back to Inventory</button>
    </div>
  );
}
