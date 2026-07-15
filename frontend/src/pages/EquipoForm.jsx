import { useState, useEffect, useRef } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getProduct, createProduct, updateProduct } from '../api/productService';
import { getCategories } from '../api/categoryService';
import { getSuppliers } from '../api/supplierService';

export default function EquipoForm() {
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
  const [showScanner, setShowScanner] = useState(false);
  const [ocrText, setOcrText] = useState('');
  const videoRef = useRef(null);
  const canvasRef = useRef(null);
  const streamRef = useRef(null);

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
    return () => stopCamera();
  }, [id]);

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
      if (lines.length > 0 && !form.sku) setForm({ ...form, sku: lines[0].trim() });
    } catch (err) {
      alert('Error al procesar la imagen: ' + err.message);
    }
  };

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
            <label>SKU / Número de serie</label>
            <input value={form.sku} onChange={(e) => setForm({ ...form, sku: e.target.value })} required />
          </div>
        </div>
        <div className="form-group">
          <label>Descripción</label>
          <textarea value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} />
        </div>
        <div className="form-row">
          <div className="form-group">
            <label>Categoría</label>
            <select value={form.categoryId} onChange={(e) => setForm({ ...form, categoryId: e.target.value })} required>
              <option value="">Seleccionar categoría</option>
              {categories.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>
          </div>
          <div className="form-group">
            <label>Proveedor (opcional)</label>
            <select value={form.supplierId} onChange={(e) => setForm({ ...form, supplierId: e.target.value })}>
              <option value="">Sin proveedor</option>
              {suppliers.map((s) => <option key={s.id} value={s.id}>{s.name}</option>)}
            </select>
          </div>
        </div>
        <div className="form-row">
          <div className="form-group">
            <label>Precio</label>
            <input type="number" step="0.01" value={form.price.amount}
              onChange={(e) => setForm({ ...form, price: { ...form.price, amount: e.target.value } })} required />
          </div>
          <div className="form-group">
            <label>Moneda</label>
            <select value={form.price.currency}
              onChange={(e) => setForm({ ...form, price: { ...form.price, currency: e.target.value } })}>
              <option value="USD">USD</option>
              <option value="DOP">DOP</option>
              <option value="EUR">EUR</option>
            </select>
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
