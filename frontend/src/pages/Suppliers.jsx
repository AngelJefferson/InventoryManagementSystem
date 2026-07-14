import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getSuppliers, deleteSupplier } from '../api/supplierService';

export default function Suppliers() {
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);

  const load = () => {
    setLoading(true);
    getSuppliers().then((r) => setSuppliers(r.data)).finally(() => setLoading(false));
  };

  useEffect(load, []);

  const handleDelete = async (id) => {
    if (!confirm('Delete this supplier?')) return;
    await deleteSupplier(id);
    load();
  };

  if (loading) return <div className="loading">Loading...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Suppliers</h1>
        <Link to="/suppliers/new" className="btn btn-primary">+ New Supplier</Link>
      </div>
      <table className="table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Contact</th>
            <th>Email</th>
            <th>Phone</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {suppliers.map((s) => (
            <tr key={s.id}>
              <td>{s.name}</td>
              <td>{s.contactName}</td>
              <td>{s.email}</td>
              <td>{s.phone}</td>
              <td className="actions">
                <Link to={`/suppliers/${s.id}/edit`} className="btn btn-sm">Edit</Link>
                <button onClick={() => handleDelete(s.id)} className="btn btn-sm btn-danger">Delete</button>
              </td>
            </tr>
          ))}
          {suppliers.length === 0 && (
            <tr><td colSpan="5" className="text-center">No suppliers found</td></tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
