import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getCategories, deleteCategory } from '../api/categoryService';

export default function Categories() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);

  const load = () => {
    setLoading(true);
    getCategories().then((r) => setCategories(r.data)).finally(() => setLoading(false));
  };

  useEffect(load, []);

  const handleDelete = async (id) => {
    if (!confirm('Delete this category?')) return;
    await deleteCategory(id);
    load();
  };

  if (loading) return <div className="loading">Loading...</div>;

  return (
    <div>
      <div className="page-header">
        <h1>Categories</h1>
        <Link to="/categories/new" className="btn btn-primary">+ New Category</Link>
      </div>
      <table className="table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {categories.map((c) => (
            <tr key={c.id}>
              <td>{c.name}</td>
              <td>{c.description}</td>
              <td className="actions">
                <Link to={`/categories/${c.id}/edit`} className="btn btn-sm">Edit</Link>
                <button onClick={() => handleDelete(c.id)} className="btn btn-sm btn-danger">Delete</button>
              </td>
            </tr>
          ))}
          {categories.length === 0 && (
            <tr><td colSpan="3" className="text-center">No categories found</td></tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
