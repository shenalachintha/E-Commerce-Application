import axios from 'axios'

const API_BASE = import.meta.env.VITE_API_URL || '/api'

export const api = axios.create({
  baseURL: API_BASE,
  headers: { 'Content-Type': 'application/json' },
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

// Auth
export const register = (data) => api.post('/auth/register', data)
export const login = (data) => api.post('/auth/login', data)

// Products
export const getProducts = (params) => api.get('/products', { params })
export const getProduct = (id) => api.get(`/products/${id}`)

// Categories
export const getCategories = () => api.get('/categories')

// Cart (requires auth)
export const getCart = () => api.get('/cart')
export const addToCart = (productId, quantity) => api.post('/cart', { productId, quantity })
export const updateCartItem = (productId, quantity) => api.put('/cart', { productId, quantity })
export const removeFromCart = (productId) => api.delete(`/cart/${productId}`)
export const clearCart = () => api.delete('/cart')

// Orders (requires auth)
export const getOrderHistory = (page = 1, pageSize = 10) =>
  api.get('/orders/history', { params: { page, pageSize } })
export const createOrder = (data) => api.post('/orders', data)
export const getOrder = (id) => api.get(`/orders/${id}`)
