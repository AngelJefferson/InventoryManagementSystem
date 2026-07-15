import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getCategory, createCategory, updateCategory } from '../api/categoryService';

export default function CategoriaForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEdit = Boolean(id);
  const [form, setForm] = useState({ name: '', description: '' });
  const [error, setError] = useState('');

  useEffect(() => {
    if (isEdit) getCategory(id).then((r) => setForm({ name: r.data.name, description: r.data.description }));
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (isEdit) await updateCategory(id, form);
      else await createCategory(form);
      navigate('/categorias');
    } catch (err) {
      setError(err.response?.data?.title || 'Error al guardar la categoría');
    }
  };

  return (
    <div className="form-page">
      <h1>{isEdit ? 'Editar Categoría' : 'Nueva Categoría'}</h1>
      <form onSubmit={handleSubmit}>
        {error && <div className="alert alert-error">{error}</div>}
        <div className="form-group">
          <label>Nombre</label>
          <input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} required />
        </div>
        <div className="form-group">
          <label>Descripción</label>
          <textarea value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} />
        </div>
        <div className="form-actions">
          <button type="submit" className="btn btn-primary">{isEdit ? 'Actualizar' : 'Crear'}</button>
          <button type="button" onClick={() => navigate('/categorias')} className="btn">Cancelar</button>
        </div>
      </form>
    </div>
  );
}
