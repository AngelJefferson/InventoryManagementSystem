import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getProduct, createProduct, updateProduct } from '../api/productService';
import { getCategories } from '../api/categoryService';
import { getSuppliers } from '../api/supplierService';

export default function ProductForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEdit = Boolean(id);
  const [categories, setCategories] = useState([]);
  const [suppliers, setSuppliers] = useState([]);
  const [form, setForm] = useState({
    name: '', sku: '', description: '', categoryId: '', supplierId: '',
    price: { amount: 0, currency: 'USD' },
  });
  const [error, setError] = useState('');

  useEffect(() => {
    getCategories().then((r) => setCategories(r.data));
    getSuppliers().then((r) => setSuppliers(r.data));
    if (isEdit) getProduct(id).then((r) => {
      const p = r.data;
      setForm({
        name: p.name, sku: p.sku, description: p.description || '',
        categoryId: p.categoryId, supplierId: p.supplierId || '',
        price: p.price || { amount: 0, currency: 'USD' },
      });
    });
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const payload = {
        ...form,
        supplierId: form.supplierId || null,
        price: { amount: parseFloat(form.price.amount), currency: form.price.currency },
      };
      if (isEdit) await updateProduct(id, payload);
      else await createProduct(payload);
      navigate('/products');
    } catch (err) {
      setError(err.response?.data?.title || 'Error saving product');
    }
  };

  return (
    <div className="form-page">
      <h1>{isEdit ? 'Edit Product' : 'New Product'}</h1>
      <form onSubmit={handleSubmit}>
        {error && <div className="alert alert-error">{error}</div>}
        <div className="form-row">
          <div className="form-group">
            <label>Name</label>
            <input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} required />
          </div>
          <div className="form-group">
            <label>SKU</label>
            <input value={form.sku} onChange={(e) => setForm({ ...form, sku: e.target.value })} required />
          </div>
        </div>
        <div className="form-group">
          <label>Description</label>
          <textarea value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} />
        </div>
        <div className="form-row">
          <div className="form-group">
            <label>Category</label>
            <select value={form.categoryId} onChange={(e) => setForm({ ...form, categoryId: e.target.value })} required>
              <option value="">Select category</option>
              {categories.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>
          </div>
          <div className="form-group">
            <label>Supplier</label>
            <select value={form.supplierId} onChange={(e) => setForm({ ...form, supplierId: e.target.value })}>
              <option value="">Select supplier</option>
              {suppliers.map((s) => <option key={s.id} value={s.id}>{s.name}</option>)}
            </select>
          </div>
        </div>
        <div className="form-row">
          <div className="form-group">
            <label>Price</label>
            <input type="number" step="0.01" value={form.price.amount}
              onChange={(e) => setForm({ ...form, price: { ...form.price, amount: e.target.value } })} required />
          </div>
          <div className="form-group">
            <label>Currency</label>
            <select value={form.price.currency}
              onChange={(e) => setForm({ ...form, price: { ...form.price, currency: e.target.value } })}>
              <option value="USD">USD</option>
              <option value="MXN">MXN</option>
              <option value="EUR">EUR</option>
            </select>
          </div>
        </div>
        <div className="form-actions">
          <button type="submit" className="btn btn-primary">{isEdit ? 'Update' : 'Create'}</button>
          <button type="button" onClick={() => navigate('/products')} className="btn">Cancel</button>
        </div>
      </form>
    </div>
  );
}
