import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getSuppliers, deleteSupplier } from '../api/supplierService';

export default function Proveedores() {
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');

  const load = () => {
    setLoading(true);
    getSuppliers().then((r) => setSuppliers(r.data)).finally(() => setLoading(false));
  };

  useEffect(load, []);

  const handleDelete = async (id) => {
    if (!confirm('¿Eliminar este proveedor?')) return;
    await deleteSupplier(id);
    load();
  };

  const filtered = suppliers.filter((s) =>
    s.name.toLowerCase().includes(search.toLowerCase())
  );

  if (loading) return <div className="loading">Cargando...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Proveedores</h1>
        <Link to="/proveedores/nuevo" className="btn btn-primary">+ Nuevo Proveedor</Link>
      </div>
      <div className="search-bar">
        <input placeholder="Buscar proveedor..." value={search}
          onChange={(e) => setSearch(e.target.value)} />
      </div>
      <div className="table-container">
        <table className="table">
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Contacto</th>
              <th>Correo</th>
              <th>Teléfono</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {filtered.map((s) => (
              <tr key={s.id}>
                <td>{s.name}</td>
                <td>{s.contactName}</td>
                <td>{s.email}</td>
                <td>{s.phone}</td>
                <td className="actions">
                  <Link to={`/proveedores/${s.id}/editar`} className="btn btn-sm">Editar</Link>
                  <button onClick={() => handleDelete(s.id)} className="btn btn-sm btn-danger">Eliminar</button>
                </td>
              </tr>
            ))}
            {filtered.length === 0 && (
              <tr><td colSpan="5" className="text-center">No se encontraron proveedores</td></tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
