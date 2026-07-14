import api from './axios';

export const login = (username, password) =>
  api.post('/auth/login', { username, password });

export const register = (username, password, email, role = 'User') =>
  api.post('/auth/register', { username, password, email, role });

export const getMe = () =>
  api.get('/auth/me');
