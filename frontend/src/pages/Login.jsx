import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../api/authService';
import { useAuth } from '../context/AuthContext';

export default function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const { loginUser } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await login(username, password);
      loginUser(res.data.token, { username: res.data.username, role: res.data.role });
      navigate('/');
    } catch {
      setError('Invalid credentials');
    }
  };

  return (
    <div className="login-page">
      <form className="login-form" onSubmit={handleSubmit}>
        <h1>Inventory Management</h1>
        <p className="login-subtitle">Sign in to continue</p>
        {error && <div className="alert alert-error">{error}</div>}
        <div className="form-group">
          <label>Username</label>
          <input value={username} onChange={(e) => setUsername(e.target.value)} required />
        </div>
        <div className="form-group">
          <label>Password</label>
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        </div>
        <button type="submit" className="btn btn-primary btn-block">Sign In</button>
        <p className="login-hint">Demo: admin / admin123</p>
      </form>
    </div>
  );
}
