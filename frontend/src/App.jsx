import { Routes, Route, Navigate } from 'react-router-dom';
import './App.css';
import Layout from './components/Layout';
import ProtectedRoute from './components/ProtectedRoute';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Categories from './pages/Categories';
import CategoryForm from './pages/CategoryForm';
import Products from './pages/Products';
import ProductForm from './pages/ProductForm';
import Suppliers from './pages/Suppliers';
import SupplierForm from './pages/SupplierForm';
import Inventory from './pages/Inventory';
import Movements from './pages/Movements';

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/" element={<ProtectedRoute><Layout /></ProtectedRoute>}>
        <Route index element={<Dashboard />} />
        <Route path="categories" element={<Categories />} />
        <Route path="categories/new" element={<CategoryForm />} />
        <Route path="categories/:id/edit" element={<CategoryForm />} />
        <Route path="products" element={<Products />} />
        <Route path="products/new" element={<ProductForm />} />
        <Route path="products/:id/edit" element={<ProductForm />} />
        <Route path="products/:id/inventory" element={<Inventory />} />
        <Route path="products/:id/movements" element={<Movements />} />
        <Route path="suppliers" element={<Suppliers />} />
        <Route path="suppliers/new" element={<SupplierForm />} />
        <Route path="suppliers/:id/edit" element={<SupplierForm />} />
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
