import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login, register } from '../api/authService';
import { useAuth } from '../context/AuthContext';

export default function Login() {
  const [isRegister, setIsRegister] = useState(false);
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [email, setEmail] = useState('');
  const [error, setError] = useState('');
  const { loginUser } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = isRegister
        ? await register(username, password, email)
        : await login(username, password);
      loginUser(res.data.token, { username: res.data.username, role: res.data.role });
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.error || 'Invalid credentials');
    }
  };

  return (
    <div className="login-page">
      <form className="login-form" onSubmit={handleSubmit}>
        <h1>Inventory Management</h1>
        <p className="login-subtitle">{isRegister ? 'Create account' : 'Sign in to continue'}</p>
        {error && <div className="alert alert-error">{error}</div>}
        <div className="form-group">
          <label>Username</label>
          <input value={username} onChange={(e) => setUsername(e.target.value)} required />
        </div>
        {isRegister && (
          <div className="form-group">
            <label>Email</label>
            <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
          </div>
        )}
        <div className="form-group">
          <label>Password</label>
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        </div>
        <button type="submit" className="btn btn-primary btn-block">
          {isRegister ? 'Register' : 'Sign In'}
        </button>
        <p className="login-hint">
          {isRegister ? (
            <>Already have an account? <a href="#" onClick={(e) => { e.preventDefault(); setIsRegister(false); setError(''); }}>Sign in</a></>
          ) : (
            <>Don't have an account? <a href="#" onClick={(e) => { e.preventDefault(); setIsRegister(true); setError(''); }}>Register</a></>
          )}
        </p>
        {!isRegister && <p className="login-hint" style={{ marginTop: 4 }}>Demo: admin / admin123</p>}
      </form>
    </div>
  );
}
