import { Link } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Layout({ children }) {
  const { user, logout } = useAuth()

  return (
    <div className="flex flex-col min-h-screen">
      <nav className="sticky top-0 z-50 glass shadow-soft backdrop-blur-3xl border-b border-white/40">
        <div className="max-w-7xl mx-auto px-6 lg:px-12">
          <div className="flex justify-between h-20 items-center">
            <div className="flex items-center gap-16">
              <Link to="/" className="text-3xl font-display font-black tracking-tight text-slate-900 group">
                E<span className="text-primary-600 group-hover:animate-pulse">.</span>Store
              </Link>
              <div className="hidden lg:flex items-center gap-10">
                <Link to="/" className="text-[14px] font-bold text-slate-500 hover:text-primary-600 uppercase tracking-widest transition-all">Collections</Link>
                {user && (
                  <>
                    <Link to="/cart" className="text-[14px] font-bold text-slate-500 hover:text-primary-600 uppercase tracking-widest transition-all">Cart</Link>
                    <Link to="/orders" className="text-[14px] font-bold text-slate-500 hover:text-primary-600 uppercase tracking-widest transition-all">History</Link>
                  </>
                )}
              </div>
            </div>
            <div className="flex items-center gap-8">
              {user ? (
                <div className="flex items-center gap-8">
                  <div className="hidden sm:flex flex-col items-end">
                    <span className="text-[10px] font-black text-slate-400 uppercase tracking-[0.2em] mb-0.5">Connected</span>
                    <span className="text-sm font-bold text-slate-900">{user.name}</span>
                  </div>
                  <button 
                    onClick={logout} 
                    className="px-6 py-3 text-xs font-black uppercase tracking-widest text-slate-900 bg-white border-2 border-slate-900 rounded-xl hover:bg-slate-900 hover:text-white transition-all shadow-premium active:scale-95"
                  >
                    Logout
                  </button>
                </div>
              ) : (
                <div className="flex items-center gap-6">
                  <Link to="/login" className="text-xs font-black uppercase tracking-widest text-slate-500 hover:text-primary-600 transition-colors">Login</Link>
                  <Link 
                    to="/register" 
                    className="px-8 py-3.5 text-xs font-black uppercase tracking-widest text-white bg-slate-900 rounded-xl hover:bg-primary-600 hover:shadow-2xl hover:shadow-primary-600/40 transition-all active:scale-95"
                  >
                    Register
                  </Link>
                </div>
              )}
            </div>
          </div>
        </div>
      </nav>
      <main className="flex-grow max-w-7xl mx-auto w-full px-6 lg:px-12 py-16 animate-fade-in">
        {children}
      </main>
      <footer className="bg-white border-t border-slate-100 py-24">
        <div className="max-w-7xl mx-auto px-6 lg:px-12">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-16 border-b border-slate-100 pb-20">
            <div className="col-span-1 md:col-span-2">
              <div className="text-4xl font-display font-black text-slate-900 mb-8">
                E<span className="text-primary-600">.</span>Store
              </div>
              <p className="text-lg text-slate-500 leading-relaxed max-w-md font-medium">Elevating your shopping experience with curated collections and seamless service. Your style, delivered with precision.</p>
            </div>
            <div>
              <h4 className="text-[10px] font-black text-slate-400 uppercase tracking-[0.25em] mb-8">Navigation</h4>
              <ul className="space-y-4 text-sm font-bold">
                <li><Link to="/" className="text-slate-900 hover:text-primary-600 transition-colors">All Products</Link></li>
                <li><Link to="/cart" className="text-slate-900 hover:text-primary-600 transition-colors">Shopping Bag</Link></li>
                <li><Link to="/orders" className="text-slate-900 hover:text-primary-600 transition-colors">Orders</Link></li>
              </ul>
            </div>
            <div>
              <h4 className="text-[10px] font-black text-slate-400 uppercase tracking-[0.25em] mb-8">Support</h4>
              <ul className="space-y-4 text-sm font-bold">
                <li><a href="#" className="text-slate-900 hover:text-primary-600 transition-colors">Delivery Info</a></li>
                <li><a href="#" className="text-slate-900 hover:text-primary-600 transition-colors">Returns Policy</a></li>
                <li><a href="#" className="text-slate-900 hover:text-primary-600 transition-colors">Contact Support</a></li>
              </ul>
            </div>
          </div>
          <div className="pt-12 text-center">
            <p className="text-[10px] font-black uppercase tracking-[0.4em] text-slate-400">&copy; 2026 E-Store Platform. Engineered for Excellence.</p>
          </div>
        </div>
      </footer>
    </div>
  )
}
