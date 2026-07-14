import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getSupplier, createSupplier, updateSupplier } from '../api/supplierService';

export default function SupplierForm() {
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
      if (isEdit) await updateSupplier(id, form);
      else await createSupplier(form);
      navigate('/suppliers');
    } catch (err) {
      setError(err.response?.data?.title || 'Error saving supplier');
    }
  };

  return (
    <div className="form-page">
      <h1>{isEdit ? 'Edit Supplier' : 'New Supplier'}</h1>
      <form onSubmit={handleSubmit}>
        {error && <div className="alert alert-error">{error}</div>}
        <div className="form-row">
          <div className="form-group">
            <label>Name</label>
            <input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} required />
          </div>
          <div className="form-group">
            <label>Contact Name</label>
            <input value={form.contactName} onChange={(e) => setForm({ ...form, contactName: e.target.value })} />
          </div>
        </div>
        <div className="form-row">
          <div className="form-group">
            <label>Email</label>
            <input type="email" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
          </div>
          <div className="form-group">
            <label>Phone</label>
            <input value={form.phone} onChange={(e) => setForm({ ...form, phone: e.target.value })} />
          </div>
        </div>
        <div className="form-group">
          <label>Address</label>
          <textarea value={form.address} onChange={(e) => setForm({ ...form, address: e.target.value })} />
        </div>
        <div className="form-actions">
          <button type="submit" className="btn btn-primary">{isEdit ? 'Update' : 'Create'}</button>
          <button type="button" onClick={() => navigate('/suppliers')} className="btn">Cancel</button>
        </div>
      </form>
    </div>
  );
}
