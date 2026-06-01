'use client'

import Link from 'next/link'
import { usePathname } from 'next/navigation'

export default function NavBar() {
  const pathname = usePathname();

  const links = [
    {href: '/', label: 'Início', icon: '🏠'},
    {href: '/acervo', label: 'Acervo', icon: '📚'},
    {href: '/cadastro', label: 'Cadastro', icon: '➕'},
    {href: '/emprestimo', label: 'Empréstimo', icon: '↪️'},
    {href: '/devolucao', label: 'Devolução', icon: '↩️'},
  ];

  return (
    <nav className="bg-gradient-to-r from-indigo-600 via-indigo-700 to-purple-700 shadow-xl border-b border-indigo-500/20 sticky top-0 z-50">
      <div className="max-w-7xl mx-auto px-6">
        <div className="flex items-center justify-between h-20">
          <Link href="/" className="flex items-center gap-2 hover:opacity-90 transition-opacity">
            <div className="text-3xl">📚</div>
            <div>
              <h1 className="text-white font-bold text-lg">Biblioteca</h1>
              <p className="text-indigo-100 text-xs">Sistema de Gestão</p>
            </div>
          </Link>
          
          <div className="flex gap-1">
            {links.map(link => {
              const isActive = pathname === link.href;
              return (
                <Link
                  key={link.href}
                  href={link.href}
                  className={`flex items-center gap-1.5 px-4 py-2 rounded-lg text-sm font-medium transition-all duration-200 ${
                    isActive
                      ? 'bg-white/20 text-white shadow-lg backdrop-blur-sm'
                      : 'text-indigo-100 hover:bg-white/10 hover:text-white backdrop-blur-sm'
                  }`}
                  title={link.label}
                >
                  <span>{link.icon}</span>
                  <span className="hidden sm:inline">{link.label}</span>
                </Link>
              );
            })}
          </div>
        </div>
      </div>
    </nav>
  );
}