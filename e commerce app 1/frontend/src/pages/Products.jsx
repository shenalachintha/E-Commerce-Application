import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { getProducts, getCategories } from '../api'
import ImageWithFallback from '../components/ImageWithFallback'

export default function Products() {
  const [products, setProducts] = useState([])
  const [categories, setCategories] = useState([])
  const [page, setPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [search, setSearch] = useState('')
  const [categoryId, setCategoryId] = useState('')
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    getCategories()
      .then(({ data }) => setCategories(Array.isArray(data) ? data : (data?.items || [])))
      .catch(() => setCategories([]))
  }, [])

  useEffect(() => {
    setLoading(true)
    const params = { page, pageSize: 12 }
    if (search) params.search = search
    if (categoryId) params.categoryId = categoryId
    getProducts(params)
      .then(({ data }) => {
        setProducts(data?.items || [])
        setTotalPages(data?.totalPages || 1)
      })
      .catch(() => setProducts([]))
      .finally(() => setLoading(false))
  }, [page, search, categoryId])

  const handleSearch = (e) => {
    e.preventDefault()
    setPage(1)
    const formData = new FormData(e.target)
    setSearch(formData.get('search') || '')
    setCategoryId(formData.get('category') || '')
  }

  return (
    <div className="space-y-20">
      <div className="relative overflow-hidden bg-slate-900 rounded-[3rem] p-12 lg:p-20 text-white shadow-2xl">
        <div className="absolute top-0 right-0 -mt-20 -mr-20 w-96 h-96 bg-primary-600/20 rounded-full blur-[100px]"></div>
        <div className="absolute bottom-0 left-0 -mb-20 -ml-20 w-96 h-96 bg-secondary-600/10 rounded-full blur-[100px]"></div>
        
        <div className="relative z-10 max-w-3xl">
          <h1 className="text-5xl lg:text-7xl font-display font-black leading-tight mb-8">
            Curated <span className="text-transparent bg-clip-text bg-gradient-to-r from-primary-400 to-primary-200">Collections</span>
          </h1>
          <form onSubmit={handleSearch} className="flex flex-wrap gap-4 bg-white/10 backdrop-blur-md p-2 rounded-3xl border border-white/10">
            <input 
              name="search" 
              type="text" 
              className="flex-grow bg-transparent border-none px-6 py-4 text-lg text-white placeholder-slate-400 focus:ring-0 outline-none" 
              placeholder="Search our luxury items..." 
              defaultValue={search} 
            />
            <select 
              name="category" 
              className="bg-transparent border-none px-6 py-4 text-sm font-bold text-slate-300 focus:ring-0 outline-none cursor-pointer border-l border-white/10" 
              defaultValue={categoryId}
            >
              <option value="" className="text-slate-900">All Categories</option>
              {Array.isArray(categories) && categories.map((c) => <option key={c.id} value={c.id} className="text-slate-900">{c.name}</option>)}
            </select>
            <button 
              type="submit" 
              className="px-10 py-4 bg-white text-slate-900 font-black uppercase tracking-widest rounded-2xl hover:bg-primary-500 hover:text-white transition-all active:scale-95"
            >
              Find
            </button>
          </form>
        </div>
      </div>

      {loading ? (
        <div className="flex justify-center items-center h-96">
          <div className="w-16 h-16 border-8 border-slate-100 border-t-primary-600 rounded-full animate-spin"></div>
        </div>
      ) : (
        <>
          {products.length === 0 ? (
            <div className="text-center py-32 glass rounded-[3rem]">
              <h3 className="text-2xl font-bold text-slate-900">End of the search?</h3>
              <p className="text-slate-500 mt-4 max-w-sm mx-auto">We couldn't find items matching your criteria. Try exploring our other premium collections.</p>
            </div>
          ) : (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-12">
              {Array.isArray(products) && products.map((p) => (
                <Link key={p.id} to={`/products/${p.id}`} className="group block">
                  <div className="premium-card bg-white rounded-[2.5rem] overflow-hidden border border-slate-50 h-full flex flex-col">
                    <div className="relative aspect-[4/5] overflow-hidden bg-slate-100">
                      <ImageWithFallback 
                        src={p.imageUrl} 
                        className="w-full h-full object-cover transition-transform duration-1000 group-hover:scale-110" 
                        alt={p.name} 
                      />
                      <div className="absolute bottom-6 left-6 right-6">
                        <div className="glass backdrop-blur-xl px-4 py-3 rounded-2xl flex justify-between items-center transform translate-y-20 opacity-0 group-hover:translate-y-0 group-hover:opacity-100 transition-all duration-500">
                          <span className="text-[10px] font-black uppercase tracking-widest text-slate-900">View Details</span>
                          <svg className="w-4 h-4 text-slate-900" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M14 5l7 7m0 0l-7 7m7-7H3"/></svg>
                        </div>
                      </div>
                    </div>
                    <div className="p-8 flex-grow flex flex-col justify-between">
                      <div>
                        <div className="flex justify-between items-start mb-4">
                          <span className="text-[10px] font-black text-primary-600 uppercase tracking-[0.2em]">New Arrival</span>
                          <span className="text-[10px] font-bold text-slate-400">#00{p.id}</span>
                        </div>
                        <h3 className="text-xl font-black text-slate-900 group-hover:text-primary-600 transition-colors leading-tight mb-2 uppercase">{p.name}</h3>
                      </div>
                      <div className="mt-8 flex items-baseline gap-2">
                        <span className="text-3xl font-display font-black text-slate-900">${Number(p.price).toLocaleString()}</span>
                        <span className="text-xs font-bold text-slate-400 underline decoration-primary-500/30">Free Shipping</span>
                      </div>
                    </div>
                  </div>
                </Link>
              ))}
            </div>
          )}

          {totalPages > 1 && (
            <div className="flex justify-center pt-24">
              <nav className="flex items-center gap-4 bg-white p-3 rounded-[2rem] shadow-premium border border-slate-50">
                <button 
                  onClick={() => setPage((p) => p - 1)} 
                  disabled={page <= 1}
                  className="w-12 h-12 flex items-center justify-center text-slate-400 hover:text-white hover:bg-slate-900 disabled:opacity-20 transition-all rounded-2xl"
                >
                  <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M15 19l-7-7 7-7"/></svg>
                </button>
                <div className="px-6 text-sm font-black text-slate-900 tracking-widest uppercase">
                  Page <span className="text-primary-600">{page}</span> of {totalPages}
                </div>
                <button 
                  onClick={() => setPage((p) => p + 1)} 
                  disabled={page >= totalPages}
                  className="w-12 h-12 flex items-center justify-center text-slate-400 hover:text-white hover:bg-slate-900 disabled:opacity-20 transition-all rounded-2xl"
                >
                  <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M9 5l7 7-7 7"/></svg>
                </button>
              </nav>
            </div>
          )}
        </>
      )}
    </div>
  )
}
