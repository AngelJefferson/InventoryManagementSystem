import api from './axios';

export const getStock = (productId) => api.get(`/inventory/${productId}`);
export const adjustStock = (data) => api.post('/inventory/adjust', data);
export const getMovements = (productId) => api.get(`/inventory/movements/${productId}`);
