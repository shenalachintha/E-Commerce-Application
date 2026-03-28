import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Register() {
  const { register } = useAuth()
  const navigate = useNavigate()
  const [form, setForm] = useState({ email: '', password: '', firstName: '', lastName: '' })
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value })

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      await register(form.email, form.password, form.firstName, form.lastName)
      navigate('/')
    } catch (err) {
      setError(err.response?.data?.message || 'Registration failed. Email may already exist.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-[75vh] flex items-center justify-center py-24 px-6 animate-fade-in">
      <div className="max-w-xl w-full space-y-12 bg-white p-12 lg:p-16 rounded-[4rem] shadow-premium-xl border border-slate-50 relative overflow-hidden">
        <div className="absolute top-0 right-0 w-48 h-48 bg-primary-600/5 rounded-full -mr-24 -mt-24 blur-3xl"></div>
        
        <div className="text-center relative z-10">
          <Link to="/" className="text-4xl font-display font-black tracking-tighter text-slate-900 group">
            E<span className="text-primary-600 group-hover:animate-pulse">.</span>Store
          </Link>
          <h2 className="mt-8 text-3xl font-display font-black text-slate-900 uppercase tracking-tight">Create Account</h2>
          <p className="mt-4 text-sm text-slate-400 font-bold uppercase tracking-widest">Join our exclusive community</p>
        </div>
        
        {error && (
          <div className="bg-red-50 text-red-600 p-6 rounded-3xl border border-red-100 text-xs font-black uppercase tracking-widest flex items-center gap-4 animate-shake relative z-10">
            <svg className="w-5 h-5 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"/></svg>
            {error}
          </div>
        )}

        <form className="space-y-8 relative z-10" onSubmit={handleSubmit}>
          <div className="space-y-6">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
              <div className="flex flex-col gap-3">
                <label className="text-[10px] font-black text-slate-400 uppercase tracking-[0.3em] ml-1">First Name</label>
                <input 
                  type="text" 
                  name="firstName"
                  required 
                  className="w-full bg-slate-50 border-2 border-transparent rounded-[1.5rem] px-8 py-5 text-sm font-bold focus:border-primary-500/20 focus:bg-white transition-all outline-none placeholder-slate-300" 
                  placeholder="GIVEN NAME"
                  value={form.firstName} 
                  onChange={handleChange}
                />
              </div>
              <div className="flex flex-col gap-3">
                <label className="text-[10px] font-black text-slate-400 uppercase tracking-[0.3em] ml-1">Last Name</label>
                <input 
                  type="text" 
                  name="lastName"
                  required 
                  className="w-full bg-slate-50 border-2 border-transparent rounded-[1.5rem] px-8 py-5 text-sm font-bold focus:border-primary-500/20 focus:bg-white transition-all outline-none placeholder-slate-300" 
                  placeholder="SURNAME"
                  value={form.lastName} 
                  onChange={handleChange}
                />
              </div>
            </div>

            <div className="flex flex-col gap-3">
              <label className="text-[10px] font-black text-slate-400 uppercase tracking-[0.3em] ml-1">Identity</label>
              <input 
                type="email" 
                name="email"
                required 
                className="w-full bg-slate-50 border-2 border-transparent rounded-[1.5rem] px-8 py-5 text-sm font-bold focus:border-primary-500/20 focus:bg-white transition-all outline-none placeholder-slate-300" 
                placeholder="EMAIL ADDRESS"
                value={form.email} 
                onChange={handleChange}
                autoComplete="email"
              />
            </div>
            
            <div className="flex flex-col gap-3">
              <label className="text-[10px] font-black text-slate-400 uppercase tracking-[0.3em] ml-1">Security</label>
              <input 
                type="password" 
                name="password"
                required 
                minLength={6}
                className="w-full bg-slate-50 border-2 border-transparent rounded-[1.5rem] px-8 py-5 text-sm font-bold focus:border-primary-500/20 focus:bg-white transition-all outline-none placeholder-slate-300" 
                placeholder="MIN. 6 CHARACTERS"
                value={form.password} 
                onChange={handleChange}
                autoComplete="new-password"
              />
            </div>
          </div>

          <div className="pt-4">
            <button 
              type="submit" 
              disabled={loading}
              className="w-full flex justify-center py-6 px-4 bg-slate-900 border-none font-black uppercase tracking-[0.3em] rounded-[1.5rem] text-white hover:bg-primary-600 hover:shadow-2xl hover:shadow-primary-600/40 transition-all active:scale-[0.98] disabled:opacity-50"
            >
              {loading ? 'Creating Identity...' : 'Register Account'}
            </button>
          </div>
        </form>

        <div className="text-center relative z-10">
          <p className="text-[10px] font-bold text-slate-400 uppercase tracking-widest leading-relaxed">
            Already registered? <br/>
            <Link to="/login" className="text-primary-600 hover:text-primary-700 transition-colors mt-2 block font-black">Secure Sign In</Link>
          </p>
        </div>
      </div>
    </div>
  )
}
