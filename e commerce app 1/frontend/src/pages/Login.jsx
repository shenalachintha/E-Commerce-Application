import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Login() {
  const { login } = useAuth()
  const navigate = useNavigate()
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      await login(email, password)
      navigate('/')
    } catch {
      setError('Invalid email or password.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-[70vh] flex items-center justify-center py-24 px-6 animate-fade-in">
      <div className="max-w-md w-full space-y-12 bg-white p-12 lg:p-16 rounded-[3.5rem] shadow-premium-xl border border-slate-50 relative overflow-hidden">
        <div className="absolute top-0 right-0 w-32 h-32 bg-primary-600/5 rounded-full -mr-16 -mt-16 blur-2xl"></div>
        
        <div className="text-center relative z-10">
          <Link to="/" className="text-4xl font-display font-black tracking-tighter text-slate-900 group">
            E<span className="text-primary-600 group-hover:animate-pulse">.</span>Store
          </Link>
          <h2 className="mt-8 text-3xl font-display font-black text-slate-900 uppercase tracking-tight">Welcome Back</h2>
          <p className="mt-4 text-sm text-slate-400 font-bold uppercase tracking-widest">Access your private collection</p>
        </div>
        
        {error && (
          <div className="bg-red-50 text-red-600 p-6 rounded-3xl border border-red-100 text-xs font-black uppercase tracking-widest flex items-center gap-4 animate-shake">
            <svg className="w-5 h-5 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"/></svg>
            {error}
          </div>
        )}

        <form className="space-y-8 relative z-10" onSubmit={handleSubmit}>
          <div className="space-y-6">
            <div className="flex flex-col gap-3">
              <label className="text-[10px] font-black text-slate-400 uppercase tracking-[0.3em] ml-1">Identity</label>
              <input 
                type="email" 
                required 
                className="w-full bg-slate-50 border-2 border-transparent rounded-[1.5rem] px-8 py-5 text-sm font-bold focus:border-primary-500/20 focus:bg-white transition-all outline-none placeholder-slate-300" 
                placeholder="EMAIL ADDRESS"
                value={email} 
                onChange={(e) => setEmail(e.target.value)}
                autoComplete="email"
              />
            </div>
            <div className="flex flex-col gap-3">
              <label className="text-[10px] font-black text-slate-400 uppercase tracking-[0.3em] ml-1">Security</label>
              <input 
                type="password" 
                required 
                className="w-full bg-slate-50 border-2 border-transparent rounded-[1.5rem] px-8 py-5 text-sm font-bold focus:border-primary-500/20 focus:bg-white transition-all outline-none placeholder-slate-300" 
                placeholder="PASSPHRASE"
                value={password} 
                onChange={(e) => setPassword(e.target.value)}
                autoComplete="current-password"
              />
            </div>
          </div>

          <div className="pt-4">
            <button 
              type="submit" 
              disabled={loading}
              className="w-full flex justify-center py-6 px-4 bg-slate-900 border-none font-black uppercase tracking-[0.3em] rounded-[1.5rem] text-white hover:bg-primary-600 hover:shadow-2xl hover:shadow-primary-600/40 transition-all active:scale-[0.98] disabled:opacity-50"
            >
              {loading ? 'Verifying...' : 'Authenticate'}
            </button>
          </div>
        </form>

        <div className="text-center relative z-10">
          <p className="text-[10px] font-bold text-slate-400 uppercase tracking-widest leading-relaxed">
            New to the platform? <br/>
            <Link to="/register" className="text-primary-600 hover:text-primary-700 transition-colors mt-2 block font-black">Create an Account</Link>
          </p>
        </div>
      </div>
    </div>
  )
}
