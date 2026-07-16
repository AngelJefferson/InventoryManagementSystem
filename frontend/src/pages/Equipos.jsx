import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getProducts, deleteProduct } from '../api/productService';
import * as XLSX from 'xlsx';

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

  const exportExcel = () => {
    const data = products.map((p) => ({
      'Tipo de Equipo': p.name,
      'Marca': p.categoryName,
      'Modelo': p.model || '—',
      'Nº de Serie': p.sku,
      'Número de Activo': p.assetNumber || '',
      'Usuario Asignado': p.employeeName || p.department || '—',
      'Departamento': p.department || '',
      'Ubicación Física': p.physicalLocation || '',
      'Sistema Operativo': p.operatingSystem || '',
      'Configuración Hardware': p.hardwareConfiguration || '',
      'Estado': p.status || '',
      'Fecha de Adquisición': p.acquisitionDate ? new Date(p.acquisitionDate).toLocaleDateString() : '',
      'Observaciones': p.observations || '',
      'Fecha de Mantenimiento': p.maintenanceDate ? new Date(p.maintenanceDate).toLocaleDateString() : '',
      'Proveedor': p.supplierName || '',
    }));
    const ws = XLSX.utils.json_to_sheet(data);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Equipos');
    XLSX.writeFile(wb, 'InventarioCAID_Equipos.xlsx');
  };

  const filtered = products.filter((p) =>
    p.name.toLowerCase().includes(search.toLowerCase()) ||
    (p.model || '').toLowerCase().includes(search.toLowerCase()) ||
    p.sku.toLowerCase().includes(search.toLowerCase()) ||
    (p.assetNumber || '').toLowerCase().includes(search.toLowerCase())
  );

  if (loading) return <div className="loading">Cargando...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Equipos</h1>
        <div style={{ display: 'flex', gap: 8 }}>
          <button className="btn btn-accent" onClick={exportExcel}>📥 Exportar Excel</button>
          <Link to="/equipos/nuevo" className="btn btn-primary">+ Nuevo Equipo</Link>
        </div>
      </div>
      <div className="search-bar">
        <input placeholder="Buscar equipo por nombre, modelo, Nº de Serie o activo..." value={search}
          onChange={(e) => setSearch(e.target.value)} />
      </div>
      <div className="table-container">
        <table className="table">
          <thead>
            <tr>
              <th>Tipo de Equipo</th>
              <th>Marca</th>
              <th>Modelo</th>
              <th>Nº de Serie</th>
              <th>N° Activo</th>
              <th>Asignado a</th>
              <th>Departamento</th>
              <th>Ubicación</th>
              <th>Estado</th>
              <th>Proveedor</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {filtered.map((p) => (
              <tr key={p.id}>
                <td>{p.name}</td>
                <td>{p.categoryName}</td>
                <td>{p.model || <span className="text-muted">—</span>}</td>
                <td><code>{p.sku}</code></td>
                <td>{p.assetNumber || <span className="text-muted">—</span>}</td>
                <td>{p.employeeName || p.department || <span className="text-muted">—</span>}</td>
                <td>{p.department || <span className="text-muted">—</span>}</td>
                <td>{p.physicalLocation || <span className="text-muted">—</span>}</td>
                <td>{p.status || <span className="text-muted">—</span>}</td>
                <td>{p.supplierName || <span className="text-muted">—</span>}</td>
                <td className="actions">
                  <Link to={`/equipos/${p.id}/editar`} className="btn btn-sm">Editar</Link>
                  <button onClick={() => handleDelete(p.id)} className="btn btn-sm btn-danger">Eliminar</button>
                </td>
              </tr>
            ))}
            {filtered.length === 0 && (
              <tr><td colSpan="11" className="text-center">No se encontraron equipos</td></tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
