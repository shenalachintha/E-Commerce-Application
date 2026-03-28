import { createContext, useContext, useState, useEffect } from 'react'
import * as api from '../api'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const token = localStorage.getItem('token')
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]))
        setUser({ email: payload.email, name: payload.unique_name || payload.email })
      } catch (e) {
        console.error('Invalid token found in storage:', e)
        localStorage.removeItem('token')
      }
    }
    setLoading(false)
  }, [])

  const login = async (email, password) => {
    const { data } = await api.login({ email, password })
    localStorage.setItem('token', data.token)
    setUser({ email: data.email, name: `${data.firstName} ${data.lastName}` })
    return data
  }

  const register = async (email, password, firstName, lastName) => {
    const { data } = await api.register({ email, password, firstName, lastName })
    localStorage.setItem('token', data.token)
    setUser({ email: data.email, name: `${data.firstName} ${data.lastName}` })
    return data
  }

  const logout = () => {
    localStorage.removeItem('token')
    setUser(null)
  }

  return (
    <AuthContext.Provider value={{ user, loading, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}
