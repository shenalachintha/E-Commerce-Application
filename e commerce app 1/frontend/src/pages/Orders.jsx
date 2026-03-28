import { useState, useEffect } from 'react'
import { Link, useLocation } from 'react-router-dom'
import { getOrderHistory } from '../api'

export default function Orders() {
  const [orders, setOrders] = useState([])
  const [page, setPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [loading, setLoading] = useState(true)
  const location = useLocation()
  const message = location.state?.message

  useEffect(() => {
    setLoading(true)
    getOrderHistory(page, 10)
      .then(({ data }) => {
        setOrders(data.items || [])
        setTotalPages(data.totalPages || 1)
      })
      .catch(() => setOrders([]))
      .finally(() => setLoading(false))
  }, [page])

  if (loading) return (
    <div className="flex justify-center items-center min-h-[400px]">
      <div className="w-12 h-12 border-4 border-primary-200 border-t-primary-600 rounded-full animate-spin"></div>
    </div>
  )

  const getStatusStyle = (status) => {
    switch (status) {
      case 'Delivered': return 'bg-green-50 text-green-600 border-green-100'
      case 'Cancelled': return 'bg-red-50 text-red-600 border-red-100'
      case 'Processing': return 'bg-blue-50 text-blue-600 border-blue-100'
      default: return 'bg-slate-50 text-slate-600 border-slate-100'
    }
  }

  return (
    <div className="space-y-16 animate-fade-in">
      <div className="border-b border-slate-100 pb-10 flex flex-col md:flex-row md:items-baseline justify-between gap-8">
        <div>
          <h1 className="text-5xl font-display font-black text-slate-900 uppercase tracking-tight">Order Archive</h1>
          <p className="text-sm font-bold text-slate-400 mt-4 uppercase tracking-[0.2em]">Procurement timeline & status tracking</p>
        </div>
        {message && (
          <div className="bg-primary-50 text-primary-600 px-8 py-4 rounded-[1.5rem] border border-primary-100 text-[10px] font-black uppercase tracking-widest flex items-center gap-4 animate-bounce-subtle">
            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>
            {message}
          </div>
        )}
      </div>

      {orders.length === 0 ? (
        <div className="max-w-3xl mx-auto py-24 text-center bg-white rounded-[4rem] border border-slate-50 shadow-premium-xl relative overflow-hidden">
          <div className="absolute top-0 left-0 w-32 h-32 bg-slate-50 rounded-full -ml-16 -mt-16 blur-2xl"></div>
          <div className="w-24 h-24 bg-slate-50 rounded-[2rem] flex items-center justify-center mx-auto mb-10 shadow-inner">
            <svg className="w-10 h-10 text-slate-200" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"/></svg>
          </div>
          <h2 className="text-3xl font-display font-black text-slate-900 mb-4 uppercase tracking-tight">No Acquisitions Distributed</h2>
          <p className="text-slate-400 font-bold uppercase tracking-widest text-xs mb-12">Your procurement history is currently empty.</p>
          <Link to="/" className="inline-flex px-12 py-6 bg-slate-900 text-white text-[10px] font-black uppercase tracking-[0.3em] rounded-[1.5rem] hover:bg-primary-600 transition-all active:scale-95 shadow-xl hover:shadow-primary-600/40">
            Browse Collections
          </Link>
        </div>
      ) : (
        <div className="space-y-8">
          {Array.isArray(orders) && orders.map((order) => (
            <div key={order.id} className="premium-card bg-white p-10 rounded-[3rem] border border-slate-50 group hover:-translate-y-1 transition-all duration-500">
              <div className="flex flex-col lg:flex-row justify-between items-start lg:items-center gap-10">
                <div className="flex items-center gap-8">
                  <div className="w-20 h-20 rounded-[2rem] bg-slate-50 flex items-center justify-center text-slate-200 border border-slate-100 group-hover:bg-slate-900 group-hover:text-white group-hover:border-slate-800 transition-all duration-500 shadow-inner">
                    <svg className="w-10 h-10" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"/></svg>
                  </div>
                  <div>
                    <h3 className="text-2xl font-display font-black text-slate-900 mb-2 tracking-tight">ORDER {order.orderNumber}</h3>
                    <div className="flex items-center gap-3">
                      <span className="w-2 h-2 bg-primary-600 rounded-full"></span>
                      <p className="text-[10px] font-black text-slate-400 uppercase tracking-[0.2em]">{new Date(order.orderDate).toLocaleDateString(undefined, { dateStyle: 'long' })}</p>
                    </div>
                  </div>
                </div>
                <div className="flex flex-wrap items-center gap-12 w-full lg:w-auto justify-between lg:justify-end border-t lg:border-t-0 border-slate-50 pt-8 lg:pt-0">
                  <div className="text-left lg:text-right">
                    <p className="text-[10px] font-black text-slate-400 uppercase tracking-widest mb-3">Shipment Intelligence</p>
                    <span className={`px-5 py-2 rounded-full text-[9px] font-black uppercase tracking-[0.2em] border shadow-sm ${getStatusStyle(order.status)}`}>
                      {order.status}
                    </span>
                  </div>
                  <div className="text-right">
                    <p className="text-[10px] font-black text-slate-400 uppercase tracking-widest mb-2">Total Value</p>
                    <p className="text-4xl font-display font-black text-slate-900 tracking-tighter">${Number(order.total).toLocaleString()}</p>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {totalPages > 1 && (
        <div className="flex justify-center pt-16">
          <nav className="flex items-center gap-4 bg-white px-6 py-3 rounded-[2rem] shadow-premium border border-slate-50">
            <button 
              onClick={() => setPage((p) => p - 1)} 
              disabled={page <= 1}
              className="w-12 h-12 flex items-center justify-center text-slate-300 hover:text-slate-900 disabled:opacity-30 transition-all hover:bg-slate-50 rounded-xl"
            >
              <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M15 19l-7-7 7-7"/></svg>
            </button>
            <div className="px-6 text-[11px] font-black text-slate-900 uppercase tracking-widest">
              Index <span className="text-primary-600 px-2">{page}</span> OF {totalPages}
            </div>
            <button 
              onClick={() => setPage((p) => p + 1)} 
              disabled={page >= totalPages}
              className="w-12 h-12 flex items-center justify-center text-slate-300 hover:text-slate-900 disabled:opacity-30 transition-all hover:bg-slate-50 rounded-xl"
            >
              <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M9 5l7 7-7 7"/></svg>
            </button>
          </nav>
        </div>
      )}
    </div>
  )
}
