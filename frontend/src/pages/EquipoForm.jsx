import { useState, useEffect, useRef } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getProduct, createProduct, updateProduct } from '../api/productService';
import { getCategories } from '../api/categoryService';
import { getSuppliers } from '../api/supplierService';
import { getEmployees } from '../api/employeeService';

export default function EquipoForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEdit = Boolean(id);
  const [categories, setCategories] = useState([]);
  const [suppliers, setSuppliers] = useState([]);
  const [employees, setEmployees] = useState([]);
  const [form, setForm] = useState({
    name: '', sku: '', model: '', description: '', categoryId: '', supplierId: '', employeeId: '',
  });
  const [error, setError] = useState('');
  const [showScanner, setShowScanner] = useState(false);
  const [ocrText, setOcrText] = useState('');
  const [empSearch, setEmpSearch] = useState('');
  const [empOpen, setEmpOpen] = useState(false);
  const videoRef = useRef(null);
  const canvasRef = useRef(null);
  const streamRef = useRef(null);
  const empRef = useRef(null);

  useEffect(() => {
    getCategories().then((r) => setCategories(r.data));
    getSuppliers().then((r) => setSuppliers(r.data));
    getEmployees().then((r) => setEmployees(r.data));
    if (isEdit) getProduct(id).then((r) => {
      const p = r.data;
      setForm({
        name: p.name, sku: p.sku, model: p.model || '', description: p.description || '',
        categoryId: p.categoryId, supplierId: p.supplierId || '', employeeId: p.employeeId || '',
      });
      if (p.employeeName) setEmpSearch(p.employeeName);
    });
    document.addEventListener('mousedown', (e) => { if (empRef.current && !empRef.current.contains(e.target)) setEmpOpen(false); });
    return () => stopCamera();
  }, [id]);

  const filteredEmps = employees.filter((e) =>
    e.fullName.toLowerCase().includes(empSearch.toLowerCase())
  );

  const selectEmployee = (emp) => {
    setForm({ ...form, employeeId: emp.id });
    setEmpSearch(emp.fullName);
    setEmpOpen(false);
  };

  const startCamera = async () => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ video: { facingMode: 'environment' } });
      streamRef.current = stream;
      if (videoRef.current) videoRef.current.srcObject = stream;
      setShowScanner(true);
    } catch (err) {
      alert('No se pudo acceder a la cámara: ' + err.message);
    }
  };

  const stopCamera = () => {
    if (streamRef.current) {
      streamRef.current.getTracks().forEach((t) => t.stop());
      streamRef.current = null;
    }
    setShowScanner(false);
  };

  const capturePhoto = async () => {
    if (!videoRef.current || !canvasRef.current) return;
    const video = videoRef.current;
    const canvas = canvasRef.current;
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    canvas.getContext('2d').drawImage(video, 0, 0);
    const imageData = canvas.toDataURL('image/jpeg');
    stopCamera();
    try {
      const Tesseract = await import('tesseract.js');
      const { data } = await Tesseract.recognize(imageData, 'spa');
      setOcrText(data.text);
      const lines = data.text.split('\n').filter((l) => l.trim());
      if (lines.length >= 2 && !form.model && !form.sku) {
        setForm({ ...form, model: lines[0].trim(), sku: lines[1].trim() });
      } else if (lines.length > 0 && !form.sku) {
        setForm({ ...form, sku: lines[0].trim() });
      }
    } catch (err) {
      alert('Error al procesar la imagen: ' + err.message);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const payload = {
        id, name: form.name, sku: form.sku, model: form.model, description: form.description,
        categoryId: form.categoryId, supplierId: form.supplierId || null,
        employeeId: form.employeeId || null,
      };
      if (isEdit) await updateProduct(id, payload);
      else await createProduct(payload);
      navigate('/equipos');
    } catch (err) {
      setError(err.response?.data?.title || 'Error al guardar el equipo');
    }
  };

  return (
    <div className="form-page">
      <h1>{isEdit ? 'Editar Equipo' : 'Nuevo Equipo'}</h1>
      <form onSubmit={handleSubmit}>
        {error && <div className="alert alert-error">{error}</div>}
        <div className="form-row">
          <div className="form-group">
            <label>Nombre del equipo</label>
            <input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} required />
          </div>
          <div className="form-group">
            <label>Modelo</label>
            <input value={form.model} onChange={(e) => setForm({ ...form, model: e.target.value })} />
          </div>
        </div>
        <div className="form-row">
          <div className="form-group">
            <label>S/N (Número de serie)</label>
            <input value={form.sku} onChange={(e) => setForm({ ...form, sku: e.target.value })} required placeholder="Único por equipo" />
          </div>
          <div className="form-group">
            <label>Categoría</label>
            <select value={form.categoryId} onChange={(e) => setForm({ ...form, categoryId: e.target.value })} required>
              <option value="">Seleccionar categoría</option>
              {categories.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>
          </div>
        </div>
        <div className="form-group">
          <label>Descripción</label>
          <textarea value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} />
        </div>
        <div className="form-row">
          <div className="form-group">
            <label>Proveedor (opcional)</label>
            <select value={form.supplierId} onChange={(e) => setForm({ ...form, supplierId: e.target.value })}>
              <option value="">Sin proveedor</option>
              {suppliers.map((s) => <option key={s.id} value={s.id}>{s.name}</option>)}
            </select>
          </div>
          <div className="form-group" ref={empRef} style={{ position: 'relative' }}>
            <label>Asignado a (opcional)</label>
            <input
              value={empSearch}
              onChange={(e) => { setEmpSearch(e.target.value); setEmpOpen(true); setForm({ ...form, employeeId: '' }); }}
              onFocus={() => setEmpOpen(true)}
              placeholder="Escribe para buscar empleado..."
              style={{ width: '100%' }}
            />
            {empOpen && (
              <div style={{
                position: 'absolute', zIndex: 100, background: '#fff', border: '1px solid var(--border)',
                borderRadius: 'var(--radius)', maxHeight: 200, overflowY: 'auto', width: '100%', marginTop: 2,
                boxShadow: 'var(--shadow)',
              }}>
                {filteredEmps.length === 0 ? (
                  <div style={{ padding: 10, color: 'var(--text-muted)', fontSize: '0.85rem' }}>Sin resultados</div>
                ) : filteredEmps.map((e) => (
                  <div key={e.id} onClick={() => selectEmployee(e)}
                    style={{ padding: '8px 12px', cursor: 'pointer', fontSize: '0.9rem', borderBottom: '1px solid var(--border)' }}
                    onMouseEnter={(e) => e.target.style.background = 'var(--bg)'}
                    onMouseLeave={(e) => e.target.style.background = 'transparent'}>
                    {e.fullName} {e.department ? `(${e.department})` : ''}
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>

        <div className="form-group">
          <label>Escanear etiqueta (OCR)</label>
          <p className="text-muted" style={{ fontSize: '0.8rem', marginBottom: 8 }}>
            Usa la cámara para capturar el modelo y número de serie automáticamente.
          </p>
          {!showScanner ? (
            <button type="button" className="btn btn-accent" onClick={startCamera}>
              📷 Abrir cámara
            </button>
          ) : (
            <div className="scanner-section">
              <h3>Escáner de etiquetas</h3>
              <video ref={videoRef} autoPlay playsInline className="video-preview" />
              <canvas ref={canvasRef} style={{ display: 'none' }} />
              <div style={{ display: 'flex', gap: 8, marginTop: 8 }}>
                <button type="button" className="btn btn-primary" onClick={capturePhoto}>Capturar</button>
                <button type="button" className="btn" onClick={stopCamera}>Cancelar</button>
              </div>
              {ocrText && (
                <div style={{ marginTop: 8 }}>
                  <strong>Texto detectado:</strong>
                  <pre style={{ background: '#f5f5f5', padding: 8, borderRadius: 4, marginTop: 4, fontSize: '0.8rem' }}>{ocrText}</pre>
                </div>
              )}
            </div>
          )}
        </div>

        <div className="form-actions">
          <button type="submit" className="btn btn-primary">{isEdit ? 'Actualizar' : 'Crear'}</button>
          <button type="button" onClick={() => navigate('/equipos')} className="btn">Cancelar</button>
        </div>
      </form>
    </div>
  );
}
