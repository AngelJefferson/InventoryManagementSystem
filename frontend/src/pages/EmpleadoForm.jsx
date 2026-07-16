import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getEmployee, createEmployee, updateEmployee } from '../api/employeeService';

export default function EmpleadoForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEdit = Boolean(id);
  const [form, setForm] = useState({
    fullName: '', department: '', position: '', email: '',
  });
  const [error, setError] = useState('');

  useEffect(() => {
    if (isEdit) getEmployee(id).then((r) => {
      const e = r.data;
      setForm({
        fullName: e.fullName, department: e.department || '',
        position: e.position || '', email: e.email || '',
      });
    });
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (isEdit) await updateEmployee(id, { id, ...form });
      else await createEmployee(form);
      navigate('/empleados');
    } catch (err) {
      setError(err.response?.data?.title || 'Error al guardar el empleado');
    }
  };

  return (
    <div className="form-page">
      <h1>{isEdit ? 'Editar Empleado' : 'Nuevo Empleado'}</h1>
      <form onSubmit={handleSubmit}>
        {error && <div className="alert alert-error">{error}</div>}
        <div className="form-row">
          <div className="form-group">
            <label>Nombre completo</label>
            <input value={form.fullName} onChange={(e) => setForm({ ...form, fullName: e.target.value })} required />
          </div>
          <div className="form-group">
            <label>Departamento</label>
            <input value={form.department} onChange={(e) => setForm({ ...form, department: e.target.value })} />
          </div>
        </div>
        <div className="form-row">
          <div className="form-group">
            <label>Puesto</label>
            <input value={form.position} onChange={(e) => setForm({ ...form, position: e.target.value })} />
          </div>
          <div className="form-group">
            <label>Correo electrónico</label>
            <input type="email" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
          </div>
        </div>
        <div className="form-actions">
          <button type="submit" className="btn btn-primary">{isEdit ? 'Actualizar' : 'Crear'}</button>
          <button type="button" onClick={() => navigate('/empleados')} className="btn">Cancelar</button>
        </div>
      </form>
    </div>
  );
}
