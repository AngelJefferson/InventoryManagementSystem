import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getSupplier, createSupplier, updateSupplier } from '../api/supplierService';

export default function ProveedorForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEdit = Boolean(id);
  const [form, setForm] = useState({ name: '', contactName: '', email: '', phone: '', address: '' });
  const [error, setError] = useState('');

  useEffect(() => {
    if (isEdit) getSupplier(id).then((r) => {
      const s = r.data;
      setForm({ name: s.name, contactName: s.contactName || '', email: s.email || '', phone: s.phone || '', address: s.address || '' });
    });
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (isEdit) await updateSupplier(id, { id, ...form });
      else await createSupplier(form);
      navigate('/proveedores');
    } catch (err) {
      const data = err.response?.data; setError(data?.title || data?.error || data?.detail || 'Error al guardar el proveedor');
    }
  };

  return (
    <div className="form-page">
      <h1>{isEdit ? 'Editar Proveedor' : 'Nuevo Proveedor'}</h1>
      <form onSubmit={handleSubmit}>
        {error && <div className="alert alert-error">{error}</div>}
        <div className="form-row">
          <div className="form-group">
            <label>Nombre</label>
            <input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} required />
          </div>
          <div className="form-group">
            <label>Nombre del contacto</label>
            <input value={form.contactName} onChange={(e) => setForm({ ...form, contactName: e.target.value })} />
          </div>
        </div>
        <div className="form-row">
          <div className="form-group">
            <label>Correo electrónico</label>
            <input type="email" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
          </div>
          <div className="form-group">
            <label>Teléfono</label>
            <input value={form.phone} onChange={(e) => setForm({ ...form, phone: e.target.value })} />
          </div>
        </div>
        <div className="form-group">
          <label>Dirección</label>
          <textarea value={form.address} onChange={(e) => setForm({ ...form, address: e.target.value })} />
        </div>
        <div className="form-actions">
          <button type="submit" className="btn btn-primary">{isEdit ? 'Actualizar' : 'Crear'}</button>
          <button type="button" onClick={() => navigate('/proveedores')} className="btn">Cancelar</button>
        </div>
      </form>
    </div>
  );
}
