import { useState, useEffect } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { getCart, createOrder } from '../api'
import ImageWithFallback from '../components/ImageWithFallback'

export default function Checkout() {
  const navigate = useNavigate()
  const [cart, setCart] = useState(null)
  const [shippingAddress, setShippingAddress] = useState('')
  const [notes, setNotes] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(true)
  const [submitting, setSubmitting] = useState(false)

  useEffect(() => {
    getCart().then(({ data }) => setCart(data)).catch(() => setCart(null)).finally(() => setLoading(false))
  }, [])

  const handleSubmit = async (e) => {
    e.preventDefault()
    if (!shippingAddress.trim()) { setError('Shipping address is required'); return }
    setError('')
    setSubmitting(true)
    try {
      await createOrder({ shippingAddress: shippingAddress.trim(), billingAddress: shippingAddress.trim(), notes: notes.trim() || null })
      navigate('/orders', { state: { message: 'Order placed successfully!' } })
    } catch (err) {
      setError(err.response?.data?.message || 'Checkout failed.')
    } finally {
      setSubmitting(false)
    }
  }

  if (loading) return (
    <div className="flex justify-center items-center min-h-[400px]">
      <div className="w-12 h-12 border-4 border-primary-200 border-t-primary-600 rounded-full animate-spin"></div>
    </div>
  )

  if (!cart || !cart.items?.length) return (
    <div className="max-w-2xl mx-auto py-20 text-center">
      <div className="bg-white p-12 rounded-[2.5rem] shadow-sm border border-slate-100">
        <h2 className="text-2xl font-bold text-slate-900 mb-4">Your cart is empty</h2>
        <p className="text-slate-500 mb-8">You need items in your cart to proceed to checkout.</p>
        <Link to="/" className="inline-flex px-8 py-4 bg-primary-600 text-white font-bold rounded-2xl hover:bg-primary-700 transition-all">
          Browse Products
        </Link>
      </div>
    </div>
  )

  return (
    <div className="space-y-16 animate-fade-in">
      <div className="border-b border-slate-100 pb-10">
        <h1 className="text-5xl font-display font-black text-slate-900 uppercase tracking-tight">Final Registry</h1>
        <p className="text-sm font-bold text-slate-400 mt-4 uppercase tracking-[0.2em]">Procurement finalization & delivery protocol</p>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-20 items-start">
        <div className="lg:col-span-7">
          <form onSubmit={handleSubmit} className="space-y-12">
            <section className="bg-white p-12 lg:p-16 rounded-[3.5rem] border border-slate-50 shadow-premium-xl space-y-10 relative overflow-hidden">
              <div className="absolute top-0 right-0 w-32 h-32 bg-primary-600/5 rounded-full -mr-16 -mt-16 blur-2xl"></div>
              
              <div className="flex items-center gap-4 relative z-10">
                <span className="w-10 h-10 rounded-[1rem] bg-slate-900 text-white flex items-center justify-center text-xs font-black">01</span>
                <h2 className="text-xl font-display font-black text-slate-900 uppercase tracking-tight">Logistics Protocol</h2>
              </div>
              
              {error && (
                <div className="bg-red-50 text-red-600 p-6 rounded-3xl border border-red-100 text-xs font-black uppercase tracking-widest flex items-center gap-4 animate-shake relative z-10">
                   <svg className="w-5 h-5 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"/></svg>
                  {error}
                </div>
              )}

              <div className="space-y-8 relative z-10">
                <div className="flex flex-col gap-3">
                  <label className="text-[10px] font-black text-slate-400 uppercase tracking-[0.3em] ml-1">Universal Delivery Address</label>
                  <textarea 
                    className="w-full bg-slate-50 border-2 border-transparent rounded-[1.5rem] px-8 py-5 text-sm font-bold focus:border-primary-500/20 focus:bg-white transition-all outline-none min-h-[140px] placeholder-slate-300" 
                    value={shippingAddress} 
                    onChange={(e) => setShippingAddress(e.target.value)} 
                    required 
                    placeholder="FULL DESTINATION DETAILS"
                  />
                </div>
                
                <div className="flex flex-col gap-3">
                  <label className="text-[10px] font-black text-slate-400 uppercase tracking-[0.3em] ml-1">Special Intelligence (Optional)</label>
                  <textarea 
                    className="w-full bg-slate-50 border-2 border-transparent rounded-[1.5rem] px-8 py-5 text-sm font-bold focus:border-primary-500/20 focus:bg-white transition-all outline-none min-h-[100px] placeholder-slate-300" 
                    value={notes} 
                    onChange={(e) => setNotes(e.target.value)} 
                    placeholder="SECURE DELIVERY INSTRUCTIONS..."
                  />
                </div>
              </div>
            </section>

            <button 
              type="submit" 
              disabled={submitting}
              className="w-full bg-slate-900 text-white text-xs font-black uppercase tracking-[0.4em] py-7 rounded-[2rem] hover:bg-primary-600 hover:shadow-2xl hover:shadow-primary-600/40 transition-all active:scale-[0.99] disabled:opacity-50"
            >
              {submitting ? 'Executing Protocol...' : 'Confirm Acquisition'}
            </button>
          </form>
        </div>

        <div className="lg:col-span-5 sticky top-32">
          <div className="bg-white rounded-[3.5rem] p-12 border border-slate-100 shadow-premium-xl relative overflow-hidden">
            <h3 className="text-xl font-display font-black text-slate-900 mb-10 uppercase tracking-tight">Order Inventory</h3>
            <div className="space-y-8 max-h-[400px] overflow-y-auto pr-4 custom-scrollbar">
              {cart.items.map((item) => (
                <div key={item.id} className="flex gap-6 group">
                  <div className="w-20 h-20 rounded-2xl bg-slate-50 overflow-hidden flex-shrink-0 border border-slate-100 shadow-inner">
                    <ImageWithFallback 
                      src={item.productImageUrl} 
                      className="w-full h-full object-cover grayscale-[20%] group-hover:grayscale-0 transition-all duration-500" 
                      alt={item.productName} 
                    />
                  </div>
                  <div className="flex-grow flex flex-col justify-center">
                    <h4 className="text-sm font-black text-slate-900 uppercase tracking-tight line-clamp-1 group-hover:text-primary-600 transition-colors">{item.productName}</h4>
                    <div className="flex justify-between items-center mt-2">
                      <span className="text-[10px] font-black text-slate-400 uppercase tracking-widest">UNIT QTY: {item.quantity}</span>
                      <span className="text-sm font-black text-slate-900">${Number(item.lineTotal).toLocaleString()}</span>
                    </div>
                  </div>
                </div>
              ))}
            </div>
            
            <div className="mt-10 pt-10 border-t border-slate-100 space-y-5">
              <div className="flex justify-between items-center">
                <span className="text-[10px] font-bold text-slate-400 uppercase tracking-widest">Subtotal</span>
                <span className="text-sm font-black text-slate-900">${Number(cart.total).toLocaleString()}</span>
              </div>
              <div className="flex justify-between items-center">
                <span className="text-[10px] font-bold text-slate-400 uppercase tracking-widest">Logistics</span>
                <span className="text-[9px] font-black text-primary-600 uppercase tracking-[0.2em]">Priority Compliment</span>
              </div>
              <div className="flex justify-between items-end pt-6 border-t border-slate-50">
                <span className="text-[10px] font-black text-slate-900 uppercase tracking-[0.4em]">Total Value</span>
                <span className="text-4xl font-display font-black text-primary-600 tracking-tighter">${Number(cart.total).toLocaleString()}</span>
              </div>
            </div>
            
            <div className="mt-10 flex items-center justify-center p-4 bg-slate-50 rounded-2xl">
              <div className="flex items-center gap-2 text-[9px] font-black text-slate-400 uppercase tracking-[0.2em]">
                <svg className="w-4 h-4 text-green-500" fill="currentColor" viewBox="0 0 20 20"><path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd"/></svg>
                End-to-End Encryption Enabled
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
