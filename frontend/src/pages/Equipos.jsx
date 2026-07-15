import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getProducts, deleteProduct } from '../api/productService';

export default function Equipos() {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');

  const load = () => {
    setLoading(true);
    getProducts().then((r) => setProducts(r.data)).finally(() => setLoading(false));
  };

  useEffect(load, []);

  const handleDelete = async (id) => {
    if (!confirm('¿Eliminar este equipo?')) return;
    await deleteProduct(id);
    load();
  };

  const filtered = products.filter((p) =>
    p.name.toLowerCase().includes(search.toLowerCase()) ||
    p.sku.toLowerCase().includes(search.toLowerCase())
  );

  if (loading) return <div className="loading">Cargando...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Equipos</h1>
        <Link to="/equipos/nuevo" className="btn btn-primary">+ Nuevo Equipo</Link>
      </div>
      <div className="search-bar">
        <input placeholder="Buscar equipo por nombre o SKU..." value={search}
          onChange={(e) => setSearch(e.target.value)} />
      </div>
      <div className="table-container">
        <table className="table">
          <thead>
            <tr>
              <th>Nombre</th>
              <th>SKU</th>
              <th>Precio</th>
              <th>Categoría</th>
              <th>Proveedor</th>
              <th>Stock</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {filtered.map((p) => (
              <tr key={p.id}>
                <td>{p.name}</td>
                <td>{p.sku}</td>
                <td>${p.price?.amount?.toFixed(2)}</td>
                <td>{p.categoryName}</td>
                <td>{p.supplierName || <span className="text-muted">—</span>}</td>
                <td>{p.stockQuantity ?? 0}</td>
                <td className="actions">
                  <Link to={`/equipos/${p.id}/editar`} className="btn btn-sm">Editar</Link>
                  <Link to={`/equipos/${p.id}/inventario`} className="btn btn-sm">Stock</Link>
                  <Link to={`/equipos/${p.id}/movimientos`} className="btn btn-sm">Mov.</Link>
                  <button onClick={() => handleDelete(p.id)} className="btn btn-sm btn-danger">Eliminar</button>
                </td>
              </tr>
            ))}
            {filtered.length === 0 && (
              <tr><td colSpan="7" className="text-center">No se encontraron equipos</td></tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
