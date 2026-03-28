import { useState, useEffect } from 'react'
import { useParams, useNavigate, Link } from 'react-router-dom'
import { getProduct, addToCart } from '../api'
import ImageWithFallback from '../components/ImageWithFallback'
import { useAuth } from '../context/AuthContext'

export default function ProductDetail() {
  const { id } = useParams()
  const navigate = useNavigate()
  const { user } = useAuth()
  const [product, setProduct] = useState(null)
  const [quantity, setQuantity] = useState(1)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    getProduct(id).then(({ data }) => setProduct(data)).catch(() => setProduct(null)).finally(() => setLoading(false))
  }, [id])

  const handleAddToCart = (e) => {
    e.preventDefault()
    if (!user) { navigate('/login'); return }
    setError('')
    addToCart(product.id, quantity)
      .then(() => navigate('/cart'))
      .catch((err) => setError(err.response?.data?.message || 'Failed to add to cart'))
  }

  if (loading) return (
    <div className="flex justify-center items-center min-h-[400px]">
      <div className="w-12 h-12 border-4 border-primary-200 border-t-primary-600 rounded-full animate-spin"></div>
    </div>
  )

  if (!product) return (
    <div className="max-w-2xl mx-auto py-20 text-center">
      <div className="bg-white p-12 rounded-3xl shadow-sm border border-slate-100">
        <h2 className="text-2xl font-bold text-slate-900 mb-4">Product not found</h2>
        <p className="text-slate-500 mb-8">The product you're looking for might have been moved or deleted.</p>
        <Link to="/" className="inline-flex px-8 py-4 bg-primary-600 text-white font-bold rounded-2xl hover:bg-primary-700 transition-all">
          Back to Collections
        </Link>
      </div>
    </div>
  )

  return (
    <div className="space-y-16 animate-fade-in">
      <nav className="flex items-center gap-4 text-[10px] font-black uppercase tracking-[0.3em] text-slate-400">
        <Link to="/" className="hover:text-primary-600 transition-colors">Collections</Link>
        <span className="w-1.5 h-1.5 bg-slate-200 rounded-full"></span>
        <span className="text-slate-900">{product.name}</span>
      </nav>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-24 items-start">
        <div className="relative group">
          <div className="absolute -inset-4 bg-gradient-to-tr from-primary-500/10 to-secondary-500/10 rounded-[4rem] blur-2xl opacity-0 group-hover:opacity-100 transition-duration-700"></div>
          <div className="relative aspect-[4/5] bg-white rounded-[3.5rem] overflow-hidden border border-slate-100 shadow-premium-xl">
            <ImageWithFallback 
              src={product.imageUrl} 
              className="w-full h-full object-cover transition-transform duration-1000 group-hover:scale-105" 
              alt={product.name} 
            />
            <div className="absolute top-8 left-8">
              <span className="glass backdrop-blur-2xl px-6 py-2.5 rounded-2xl text-[10px] font-black uppercase tracking-widest text-slate-900 border border-white/50 shadow-sm">
                Premium Grade
              </span>
            </div>
          </div>
        </div>

        <div className="py-8">
          <div className="mb-12">
            <div className="flex items-center gap-3 mb-6">
              <span className="w-12 h-[2px] bg-primary-600"></span>
              <span className="text-primary-600 text-[10px] font-black uppercase tracking-[0.3em]">{product.categoryName || 'Exclusive'}</span>
            </div>
            <h1 className="text-6xl font-display font-black text-slate-900 leading-[1.1] mb-8 uppercase tracking-tight">{product.name}</h1>
            <p className="text-xl text-slate-500 leading-relaxed font-medium">{product.description}</p>
          </div>

          <div className="flex items-center gap-8 mb-16">
            <div className="text-5xl font-display font-black text-slate-900 tracking-tighter">${Number(product.price).toLocaleString()}</div>
            <div className="h-10 w-[1px] bg-slate-200"></div>
            <div className="text-[10px] font-black text-slate-400 uppercase tracking-widest leading-relaxed">
              Price includes international <br/> express shipping & taxes
            </div>
          </div>

          <div className="space-y-12">
            {error && (
              <div className="bg-red-50 text-red-600 p-6 rounded-3xl border border-red-100 text-sm font-bold flex items-center gap-4 animate-shake">
                <svg className="w-6 h-6 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"/></svg>
                {error}
              </div>
            )}

            <form onSubmit={handleAddToCart} className="space-y-10">
              <div className="flex flex-col gap-4">
                <label className="text-[10px] font-black text-slate-400 uppercase tracking-[0.25em] ml-1">Selection Quantity</label>
                <div className="flex items-center bg-white border border-slate-100 w-fit rounded-[1.5rem] p-1.5 shadow-sm gap-4">
                  <button 
                    type="button" 
                    onClick={() => setQuantity(q => Math.max(1, q - 1))}
                    className="w-12 h-12 flex items-center justify-center rounded-xl bg-slate-50 hover:bg-slate-900 hover:text-white transition-all active:scale-90"
                  >
                    <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M20 12H4"/></svg>
                  </button>
                  <input 
                    type="number" 
                    readOnly
                    value={quantity} 
                    className="w-12 bg-transparent text-center text-lg font-black text-slate-900 focus:outline-none" 
                  />
                  <button 
                    type="button" 
                    onClick={() => setQuantity(q => Math.min(product.stockQuantity, q + 1))}
                    className="w-12 h-12 flex items-center justify-center rounded-xl bg-slate-50 hover:bg-slate-900 hover:text-white transition-all active:scale-90"
                  >
                    <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M12 4v16m8-8H4"/></svg>
                  </button>
                </div>
              </div>

              <div className="flex flex-col sm:flex-row gap-6">
                <button 
                  type="submit" 
                  disabled={!user || product.stockQuantity < 1}
                  className="flex-grow bg-slate-900 text-white text-xs font-black uppercase tracking-[0.3em] py-6 px-12 rounded-[2rem] hover:bg-primary-600 hover:shadow-2xl hover:shadow-primary-600/40 transition-all active:scale-[0.98] disabled:opacity-30 disabled:pointer-events-none"
                >
                  {product.stockQuantity > 0 ? 'Secure Acquisition' : 'Out of Stock'}
                </button>
                {!user && (
                  <Link to="/login" className="flex items-center justify-center px-12 border-2 border-slate-900 text-slate-900 text-xs font-black uppercase tracking-[0.2em] rounded-[2rem] hover:bg-slate-900 hover:text-white transition-all">
                    Sign in to purchase
                  </Link>
                )}
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  )
}
