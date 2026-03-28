import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { getCart, updateCartItem, removeFromCart } from '../api'
import ImageWithFallback from '../components/ImageWithFallback'

export default function Cart() {
  const [cart, setCart] = useState(null)
  const [loading, setLoading] = useState(true)

  const loadCart = () => {
    getCart().then(({ data }) => setCart(data)).catch(() => setCart(null)).finally(() => setLoading(false))
  }
  useEffect(loadCart, [])

  const handleUpdate = (productId, quantity) => {
    if (quantity < 1) return
    updateCartItem(productId, quantity).then(() => loadCart())
  }
  const handleRemove = (productId) => {
    removeFromCart(productId).then(() => loadCart())
  }

  if (loading) return (
    <div className="flex justify-center items-center min-h-[400px]">
      <div className="w-12 h-12 border-4 border-primary-200 border-t-primary-600 rounded-full animate-spin"></div>
    </div>
  )

  if (!cart || !cart.items?.length) return (
    <div className="max-w-2xl mx-auto py-20 text-center">
      <div className="bg-white p-12 rounded-[2.5rem] shadow-sm border border-slate-100">
        <div className="w-24 h-24 bg-slate-50 rounded-full flex items-center justify-center mx-auto mb-8">
          <svg className="w-10 h-10 text-slate-300" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"/></svg>
        </div>
        <h2 className="text-3xl font-display font-bold text-slate-900 mb-4">Your bag is empty</h2>
        <p className="text-slate-500 mb-10 text-lg">Looks like you haven't added anything to your cart yet.</p>
        <Link to="/" className="inline-flex px-10 py-5 bg-primary-600 text-white font-bold rounded-2xl hover:bg-primary-700 hover:shadow-xl hover:shadow-primary-500/30 transition-all active:scale-95">
          Start Shopping
        </Link>
      </div>
    </div>
  )

  return (
    <div className="space-y-16 animate-fade-in">
      <div className="flex flex-col md:flex-row items-baseline justify-between gap-4 border-b border-slate-100 pb-10">
        <h1 className="text-5xl font-display font-black text-slate-900 uppercase tracking-tight">Shopping Bag</h1>
        <div className="flex items-center gap-3">
          <span className="text-[10px] font-black text-slate-400 uppercase tracking-[0.3em]">Inventory</span>
          <span className="px-4 py-1.5 bg-slate-900 text-white text-[10px] font-black rounded-full tracking-widest leading-none">{cart.items.length} UNITS</span>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-16 items-start">
        <div className="lg:col-span-8 space-y-8">
          {Array.isArray(cart.items) && cart.items.map((item) => (
            <div key={item.id} className="premium-card bg-white p-8 rounded-[3rem] border border-slate-50 flex flex-col md:flex-row gap-10">
              <div className="w-full md:w-40 aspect-square rounded-[2rem] overflow-hidden bg-slate-50 shadow-inner">
                <ImageWithFallback 
                  src={item.productImageUrl} 
                  alt={item.productName} 
                  className="w-full h-full object-cover grayscale-[20%] group-hover:grayscale-0 transition-all duration-700" 
                />
              </div>
              <div className="flex-grow flex flex-col justify-between py-2">
                <div className="flex justify-between items-start gap-6">
                  <div>
                    <Link to={`/products/${item.productId}`} className="text-2xl font-black text-slate-900 hover:text-primary-600 transition-colors uppercase tracking-tight">{item.productName}</Link>
                    <p className="text-[10px] font-black text-primary-600 uppercase tracking-[0.2em] mt-3">Selection #00{item.productId}</p>
                  </div>
                  <button onClick={() => handleRemove(item.productId)} className="w-12 h-12 flex items-center justify-center text-slate-300 hover:text-red-500 hover:bg-red-50 rounded-2xl transition-all">
                    <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"/></svg>
                  </button>
                </div>
                <div className="flex flex-wrap items-end justify-between gap-8 mt-10">
                  <div className="flex flex-col gap-3">
                    <span className="text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1">Quantity</span>
                    <div className="flex items-center bg-slate-50 rounded-[1.25rem] p-1 border border-slate-100">
                      <button 
                        onClick={() => handleUpdate(item.productId, item.quantity - 1)}
                        className="w-10 h-10 flex items-center justify-center rounded-xl bg-white shadow-sm hover:bg-slate-900 hover:text-white transition-all active:scale-90"
                      >
                        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M20 12H4"/></svg>
                      </button>
                      <span className="w-12 text-center text-sm font-black text-slate-900">{item.quantity}</span>
                      <button 
                        onClick={() => handleUpdate(item.productId, item.quantity + 1)}
                        className="w-10 h-10 flex items-center justify-center rounded-xl bg-white shadow-sm hover:bg-slate-900 hover:text-white transition-all active:scale-90"
                      >
                        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M12 4v16m8-8H4"/></svg>
                      </button>
                    </div>
                  </div>
                  <div className="text-right">
                    <p className="text-[10px] font-black text-slate-400 uppercase tracking-widest mb-2">Item Total</p>
                    <p className="text-3xl font-display font-black text-slate-900 tracking-tighter">${Number(item.lineTotal).toLocaleString()}</p>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>

        <div className="lg:col-span-4 sticky top-32">
          <div className="bg-slate-900 rounded-[3rem] p-12 text-white shadow-premium-xl border border-white/5 relative overflow-hidden">
            <div className="absolute top-0 right-0 w-48 h-48 bg-primary-600/10 rounded-full -mr-24 -mt-24 blur-3xl"></div>
            
            <h3 className="text-2xl font-display font-black mb-10 uppercase tracking-tight relative z-10">Summary</h3>
            <div className="space-y-6 pb-10 border-b border-white/10 relative z-10">
              <div className="flex justify-between items-center">
                <span className="text-[10px] font-bold text-slate-400 uppercase tracking-widest">Subtotal</span>
                <span className="text-lg font-black text-white">${Number(cart.total).toLocaleString()}</span>
              </div>
              <div className="flex justify-between items-center">
                <span className="text-[10px] font-bold text-slate-400 uppercase tracking-widest">Logistics</span>
                <span className="text-xs font-black text-primary-400 uppercase tracking-[0.2em]">Priority Free</span>
              </div>
              <div className="flex justify-between items-center">
                <span className="text-[10px] font-bold text-slate-400 uppercase tracking-widest">Estimated Vat</span>
                <span className="text-lg font-black text-white">$0.00</span>
              </div>
            </div>
            <div className="pt-10 mb-12 relative z-10">
              <div className="mb-2 text-[10px] font-black text-primary-400 uppercase tracking-[0.4em]">Total Amount Due</div>
              <div className="text-5xl font-display font-black text-white tracking-tighter">${Number(cart.total).toLocaleString()}</div>
            </div>
            <Link to="/checkout" className="block w-full text-center bg-white py-6 rounded-2xl font-black text-slate-900 text-xs uppercase tracking-[0.3em] hover:bg-primary-500 hover:text-white hover:shadow-2xl hover:shadow-primary-500/40 transition-all active:scale-[0.98] relative z-10">
              Complete Order
            </Link>
            <div className="mt-10 flex flex-col items-center gap-4 relative z-10">
              <div className="flex items-center gap-2.5 text-[10px] font-black text-slate-500 uppercase tracking-widest">
                <svg className="w-4 h-4 text-primary-500" fill="currentColor" viewBox="0 0 20 20"><path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd"/></svg>
                Secure Cloud Protocol
              </div>
              <span className="text-[9px] text-slate-600 font-bold uppercase tracking-[0.2em]">Next Day Express Guaranteed</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
