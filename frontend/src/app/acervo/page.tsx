'use client'

import {useEffect, useState} from "react";
import {libraryService, Book} from "@/services/libraryService";

export default function AcervoPage() {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [deleting, setDeleting] = useState<string | null>(null);
  const [modalTitle, setModalTitle] = useState<string | null>(null);

  useEffect(() => {
    loadBooks();
  }, []);

  async function loadBooks() {
    try {
      const data = await libraryService.getAllBooks();
      setBooks(data);
    } catch {
      alert('Erro ao carregar livros.');
    } finally {
      setLoading(false);
    }
  }

  function openDeleteModal(title: string) {
    setModalTitle(title);
  }

  function closeDeleteModal() {
    setModalTitle(null);
  }

  async function handleDeleteFromModel(code: string) {
    if (!confirm(`Tem certeza que deseja excluir o esse exemplar?`)) return;

    setDeleting(code);
    try {
      await libraryService.deleteBook(code);
      alert('Livro excluído com sucesso!');
      closeDeleteModal();
      await loadBooks();
    }catch {
      alert('Erro ao excluir livro.');
    } finally {
      setDeleting(null);
    }
}
    
    function getStatusColor(status: string) {
      return status === 'DISPONIVEL'
      ? 'bg-green-100 text-green-800'
      : 'bg-orange-100 text-orange-800'

    }

    function getBooksWithSameTitle(title: string) {
      return books.filter(book => book.title === title);
    }

    if (loading) {
        return <div className="text-center text-gray-500 py-12">Carregando Acervo...</div>;
    }

    return (
        <div className="space-y-8">
         <div className="flex items-center justify-between mb-8">
          <div className="flex items-center gap-4">
            <div className="bg-gradient-to-br from-indigo-400 to-indigo-600 p-3 rounded-xl text-white text-2xl">📖</div>
            <div>
              <h1 className="text-3xl font-bold text-gray-900">Acervo de Livros</h1>
              <p className="text-gray-600 text-sm mt-1">Gerencie todos os livros cadastrados</p>
            </div>
          </div>
          <div className="bg-gradient-to-r from-indigo-50 to-purple-50 border border-indigo-200 px-4 py-3 rounded-xl">
            <span className="text-sm font-semibold text-indigo-700">📊 {books.length} Livro(s)</span>
          </div>
         </div>

        {books.length === 0 ? (
          <div className="text-center py-16 bg-white rounded-2xl shadow-md border border-gray-100">
            <p className="text-gray-500 text-lg font-semibold">📭 Nenhum livro cadastrado</p>
            <p className="text-gray-400  mt-2">Clique em "Cadastrar Livro" para adicionar novos exemplares ao acervo.</p>
          </div>
        ) : (
          <div className="bg-white rounded-2xl shadow-lg overflow-hidden border border-gray-100">
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gradient-to-r from-gray-50 to-gray-100">
                  <tr>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-700 uppercase tracking-wider">Código</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-700 uppercase tracking-wider">Título</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-700 uppercase tracking-wider">Autor</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-700 uppercase tracking-wider">Editora</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-700 uppercase tracking-wider">Gênero</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-700 uppercase tracking-wider">Ano</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-700 uppercase tracking-wider">Status</th>
                    <th className="px-6 py-4 text-left text-xs font-bold text-gray-700 uppercase tracking-wider">Responsável</th>
                    <th className="px-6 py-4 text-center text-xs font-bold text-gray-700 uppercase tracking-wider">Ação</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-200">
                  {books.map((book) => (
                    <tr key={book.code} className='hover:bg-indigo-50 transition-colors duration-150'>
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-semibold text-indigo-600">{book.code}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{book.title}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{book.author}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{book.publisher}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{book.genre}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{book.year}</td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <span className={`inline-flex items-center px-3 py-1.5 rounded-full text-xs font-bold ${getStatusColor(book.status)}`}>
                          {book.status === 'DISPONIVEL' ? '✓ Disponível' : '⊘ Indisponível'}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">
                        {book.borrowerName ? <span className="font-medium text-orange-600">{book.borrowerName}</span> : <span className="text-gray-400">—</span>}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <button
                          onClick={() => openDeleteModal(book.title)}
                          className="inline-flex items-center gap-2 text-red-600 hover:text-red-800 hover:bg-red-50 px-3 py-2 rounded-lg text-sm font-semibold transition-colors">
                          🗑️ Excluir
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            </div>
        )}

        {/* Modal de Exclusão */}
        {modalTitle && (
            <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
              <div className="fixed inset-0 bg-black/50 backdrop-blur-sm" onClick={closeDeleteModal}/>
              <div className="relative bg-white rounded-2xl shadow-2xl max-w-lg w-full max-h-[80vh] overflow-hidden flex flex-col border border-gray-200">
                <div className="px-8 py-6 border-b border-gray-200 bg-gradient-to-r from-red-50 to-orange-50">
                  <h2 className="text-lg font-bold text-gray-900">
                    ⚠️ Remover exemplar de &ldquo;{modalTitle}&rdquo;
                  </h2>
                  <p className="text-sm text-gray-600 mt-2">
                    Selecione qual exemplar deseja excluir:
                  </p>
                </div>
                <div className="px-8 py-6 overflow-y-auto flex-1 space-y-3">
                    {getBooksWithSameTitle(modalTitle).map(book => (
                     <div key={book.code} 
                     className="flex items-center justify-between p-4 border-2 border-gray-200 rounded-xl hover:border-red-300 hover:bg-red-50 transition-colors">
                        <div className="flex-1">
                          <p className="text-sm font-bold text-gray-900">📚 {book.code}</p>
                          <p className="text-sm text-gray-600 mt-1">{book.author} - {book.publisher} ({book.year})</p>
                          <p className="text-sm text-gray-600 mt-2">
                            Status: <span className={`px-2 py-1 rounded-full font-bold text-xs ${getStatusColor(book.status)}`}>
                              {book.status === 'DISPONIVEL' ? '✓ Disponível' : '⊘ Indisponível'}
                            </span>
                            {book.borrowerName && (
                                <span className="ml-3 text-orange-600 font-semibold"> 👤 {book.borrowerName}</span>)}
                          </p>
                        </div>
                       <button
                         onClick={() => handleDeleteFromModel(book.code)}
                         disabled={deleting === book.code}
                         className="ml-4 px-4 py-2.5 bg-red-600 text-white text-sm rounded-lg hover:bg-red-700 disabled:opacity-50 font-bold transition-colors">
                            {deleting === book.code ? '⏳...' : '🗑️ Excluir'}   
                       </button>
                    </div>
                  ))}
                </div>
                <div className="px-8 py-4 border-t border-gray-200 flex justify-end bg-gray-50">
                  <button
                    onClick={closeDeleteModal}
                    className="px-6 py-2.5 bg-gray-300 text-gray-800 rounded-lg hover:bg-gray-400 text-sm font-bold transition-colors">
                    Fechar
                  </button>
                </div>
              </div>
            </div>
          )}
        </div>
    );
}
