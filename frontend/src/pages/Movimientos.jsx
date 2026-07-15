import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getProduct } from '../api/productService';
import { getMovements } from '../api/inventoryService';

export default function Movimientos() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [product, setProduct] = useState(null);
  const [movements, setMovements] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getProduct(id).then((r) => setProduct(r.data));
    getMovements(id).then((r) => setMovements(r.data)).finally(() => setLoading(false));
  }, [id]);

  if (loading) return <div className="loading">Cargando...</div>;

  return (
    <div>
      <h1>Movimientos: {product?.name}</h1>
      <p className="text-muted">SKU: {product?.sku}</p>

      <div className="table-container">
        <table className="table">
          <thead>
            <tr>
              <th>Fecha</th>
              <th>Tipo</th>
              <th>Cantidad</th>
              <th>Motivo</th>
              <th>Usuario</th>
            </tr>
          </thead>
          <tbody>
            {movements.map((m) => (
              <tr key={m.id}>
                <td>{new Date(m.date).toLocaleString()}</td>
                <td>
                  <span className={`badge ${m.type === 'Entry' ? 'badge-success' : 'badge-danger'}`}>
                    {m.type === 'Entry' ? 'Entrada' : 'Salida'}
                  </span>
                </td>
                <td className={m.type === 'Entry' ? 'text-success' : 'text-danger'}>
                  {m.type === 'Entry' ? `+${m.quantity}` : `-${m.quantity}`}
                </td>
                <td>{m.reason}</td>
                <td>{m.userId}</td>
              </tr>
            ))}
            {movements.length === 0 && (
              <tr><td colSpan="5" className="text-center">No hay movimientos registrados</td></tr>
            )}
          </tbody>
        </table>
      </div>

      <button onClick={() => navigate(`/equipos/${id}/inventario`)} className="btn" style={{ marginTop: 16 }}>
        Volver al inventario
      </button>
    </div>
  );
}
