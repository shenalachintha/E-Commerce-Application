import React, { Component } from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import Layout from './components/Layout'
import Products from './pages/Products'
import ProductDetail from './pages/ProductDetail'
import Cart from './pages/Cart'
import Login from './pages/Login'
import Register from './pages/Register'
import Orders from './pages/Orders'
import Checkout from './pages/Checkout'
import { useAuth } from './context/AuthContext'

function PrivateRoute({ children }) {
  const { user, loading } = useAuth()
  if (loading) return (
    <div className="flex justify-center items-center h-screen">
      <div className="w-16 h-16 border-8 border-slate-100 border-t-primary-600 rounded-full animate-spin"></div>
    </div>
  )
  if (!user) return <Navigate to="/login" replace />
  return children
}

export default function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Products />} />
        <Route path="/products/:id" element={<ProductDetail />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/cart" element={<PrivateRoute><Cart /></PrivateRoute>} />
        <Route path="/checkout" element={<PrivateRoute><Checkout /></PrivateRoute>} />
        <Route path="/orders" element={<PrivateRoute><Orders /></PrivateRoute>} />
      </Routes>
    </Layout>
  )
}
