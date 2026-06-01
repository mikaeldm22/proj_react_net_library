'use client'

import {useEffect, useState} from "react";
import {libraryService, Book} from "@/services/libraryService";

export default function DevolucaoPage() {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [returning, setReturning] = useState<string | null>(null);

  useEffect(() => {
    loadBooks();
  }, []);

  async function loadBooks() {
    try {
      const data = await libraryService.getAllBooks();
      setBooks(data.filter(book => book.status === 'EMPRESTADO'));
    } catch  {
      alert("Erro ao carregar livros");
    } finally {
      setLoading(false);
    }   
 }

 async function handleReturn(code: string, title: string) {
    if (!confirm(`Confirma a devolução do livro ${title}?`)) 
      return;

      setReturning(code);
      try {
        await libraryService.returnBook(code);
        alert("Devolução realizada com sucesso!");
        setBooks(books.filter(book => book.code !== code));
      } catch {
        alert("Erro ao realizar devolução");
      } finally {
        setReturning(null);
      }
    }

    if (loading) {
      return <p className="text-center text-gray-500 py-12">Carregando livros emprestados...</p>;
    }

    return (
      <div className="space-y-8">
        <div className="flex items-center gap-4">
            <div className="bg-gradient-to-br from-green-400 to-green-600 p-3 rounded-xl text-white text-2xl">↩️</div>
            <div>
              <h1 className="text-3xl font-bold text-gray-900">Registrar Devolução</h1>
              <p className="text-gray-600 text-sm mt-1">Registre a devolução de livros emprestados</p>
            </div>
        </div>

        {books.length === 0 ? (
          <div className="text-center py-16 bg-white rounded-2xl shadow-md border border-gray-100">
            <p className="text-gray-500 text-lg font-semibold">📭 Nenhum livro emprestado</p>
            <p className="text-gray-400 mt-2">Todos os livros estão disponíveis.</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {books.map((book) => (
              <div key={book.code} className="bg-white rounded-2xl shadow-lg hover:shadow-2xl transition-all duration-300 overflow-hidden border border-gray-100 hover:border-green-300 transform hover:-translate-y-1">
                <div className="bg-gradient-to-r from-green-50 to-green-100 px-6 py-4 border-b border-green-200">
                  <h3 className="text-lg font-bold text-gray-900">{book.title}</h3>
                </div>
                
                <div className="px-6 py-6 space-y-3">
                  <div>
                    <p className="text-xs font-bold text-gray-500 uppercase tracking-wider">Código</p>
                    <p className="text-sm font-semibold text-green-600 mt-1">{book.code}</p>
                  </div>
                  
                  <div className="grid grid-cols-2 gap-3">
                    <div>
                      <p className="text-xs font-bold text-gray-500 uppercase tracking-wider">Autor</p>
                      <p className="text-sm text-gray-700 mt-1">{book.author}</p>
                    </div>
                    <div>
                      <p className="text-xs font-bold text-gray-500 uppercase tracking-wider">Editora</p>
                      <p className="text-sm text-gray-700 mt-1">{book.publisher}</p>
                    </div>
                  </div>

                  <div>
                    <p className="text-xs font-bold text-gray-500 uppercase tracking-wider">Ano de Publicação</p>
                    <p className="text-sm text-gray-700 mt-1">{book.year}</p>
                  </div>

                  <div className="border-t border-gray-200 pt-4 mt-4 space-y-3">
                    <div>
                      <p className="text-xs font-bold text-orange-600 uppercase tracking-wider">👤 Emprestado para</p>
                      <p className="text-sm font-semibold text-gray-900 mt-1">{book.borrowerName}</p>
                    </div>
                    {book.borrowedAt && (
                      <div>
                        <p className="text-xs font-bold text-gray-500 uppercase tracking-wider">📅 Data de Empréstimo</p>
                        <p className="text-sm text-gray-700 mt-1">{new Date(book.borrowedAt).toLocaleDateString('pt-BR', { weekday: 'short', year: 'numeric', month: 'long', day: 'numeric' })}</p>
                      </div>
                    )}
                  </div>
                </div>

                <div className="px-6 py-4 bg-gray-50 border-t border-gray-200">
                  <button
                    onClick={() => handleReturn(book.code, book.title)}
                    disabled={returning === book.code}
                    className="w-full bg-gradient-to-r from-green-500 to-green-600 text-white py-3 px-4 rounded-xl hover:from-green-600 hover:to-green-700 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-2 disabled:opacity-50 transition-all font-bold shadow-md hover:shadow-lg transform hover:-translate-y-0.5">
                    {returning === book.code ? '⏳ Processando...' : '✓ Registrar Devolução'}
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    );
}
