'use client'

import {useEffect, useState} from "react";
import Link from "next/link";
import {libraryService, BookStats} from "@/services/libraryService";

export default function Home() {
  const [stats, setStats] = useState<BookStats | null>(null);
  const [loading, setLoading] = useState(true);

    useEffect(() => {
    loadStats();
  }, []);

  async function loadStats() {
    try {
      const data = await libraryService.getStats();
      setStats(data);
    } catch (error) {
      console.error("Erro ao carregar estatísticas:", error);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="space-y-12">
      {/* Header */}
      <div className="text-center space-y-4">
        <div className="flex justify-center">
          <div className="inline-block bg-gradient-to-r from-indigo-500 to-purple-500 p-3 rounded-2xl shadow-lg">
            <span className="text-4xl">📚</span>
          </div>
        </div>
        <h1 className="text-5xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">
          Sistema de Gerenciamento de Biblioteca
        </h1>
        <p className="text-lg text-gray-600 max-w-2xl mx-auto">
          Gerencie seu acervo, empréstimos e devoluções de forma intuitiva e eficiente.
        </p>
      </div>

      {/* Stats Section */}
      {loading ? (
        <div className="flex justify-center">
          <div className="animate-pulse text-gray-400">Carregando estatísticas...</div>
        </div>
      ) : stats ? (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          {[
            { label: 'Total de Livros', value: stats.totalBooks, icon: '📊', color: 'from-blue-400 to-blue-600' },
            { label: 'Emprestados', value: stats.borrowedBooks, icon: '📤', color: 'from-orange-400 to-orange-600' },
            { label: 'Disponíveis', value: stats.availableBooks, icon: '✅', color: 'from-green-400 to-green-600' },
          ].map((stat, idx) => (
            <div key={idx} className="group">
              <div className={`bg-gradient-to-br ${stat.color} rounded-2xl p-8 text-white shadow-lg hover:shadow-2xl transform hover:-translate-y-1 transition-all duration-300`}>
                <div className="flex items-start justify-between">
                  <div>
                    <p className="text-white/80 text-sm font-semibold uppercase tracking-wider">{stat.label}</p>
                    <p className="text-4xl font-bold mt-3">{stat.value}</p>
                  </div>
                  <div className="text-3xl opacity-80 group-hover:scale-110 transition-transform">{stat.icon}</div>
                </div>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <div className="bg-red-50 border border-red-200 rounded-xl p-6 text-red-700 text-center">
          <p className="font-semibold">⚠️ Erro ao carregar estatísticas</p>
          <p className="text-sm mt-1">Verifique a conexão com o servidor</p>
        </div>
      )}

      {/* Action Cards */}
      <div className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6'>
        {[
          { href: '/acervo', icon: '📖', label: 'Acervo', desc: 'Visualize e gerencie', color: 'from-indigo-400 to-indigo-600' },
          { href: '/cadastro', icon: '➕', label: 'Cadastro', desc: 'Adicione novos livros', color: 'from-purple-400 to-purple-600' },
          { href: '/emprestimo', icon: '↪️', label: 'Empréstimo', desc: 'Registre empréstimos', color: 'from-pink-400 to-pink-600' },
          { href: '/devolucao', icon: '↩️', label: 'Devolução', desc: 'Registre devoluções', color: 'from-green-400 to-green-600' },
        ].map((card, idx) => (
          <Link 
            key={idx}
            href={card.href} 
            className="group relative overflow-hidden bg-white rounded-2xl shadow-md hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-2">
            <div className={`absolute inset-0 bg-gradient-to-br ${card.color} opacity-0 group-hover:opacity-5 transition-opacity`} />
            <div className="p-8 text-center relative">
              <div className="text-5xl mb-4 group-hover:scale-110 transition-transform duration-300">{card.icon}</div>
              <h3 className="font-bold text-gray-900 text-lg">{card.label}</h3>
              <p className="text-sm text-gray-600 mt-2">{card.desc}</p>
              <div className={`mt-4 h-1 bg-gradient-to-r ${card.color} rounded-full transform scale-x-0 group-hover:scale-x-100 transition-transform origin-left`}></div>
            </div>
          </Link>
        ))}
      </div>
    </div>
  );
}




