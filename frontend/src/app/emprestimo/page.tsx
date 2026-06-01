'use client'

import {useEffect, useState} from "react";
import {libraryService, Book} from "@/services/libraryService";

export default function EmprestimoPage() {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [borrowerName, setBorrowerName] = useState("");
  const [selectedBook, setSelectedBook] = useState<string>('');
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    loadBooks();
  }, []);

  async function loadBooks() {
    try {
      const data = await libraryService.getAllBooks();
      setBooks(data.filter(book => book.status === 'DISPONIVEL'));
    } catch  {
      alert("Erro ao carregar livros");
    } finally {
      setLoading(false);
    }
 }

 async function handleSubmit(event: React.FormEvent) {
    event.preventDefault();

    if (!selectedBook || !borrowerName.trim()) {
      alert("Por favor, selecione um livro e informe o nome do responsável.");
      return;
    }

    setSubmitting(true);
    try {
      await libraryService.borrowBook(selectedBook, { borrowerName: borrowerName.trim() });
      alert("Livro emprestado com sucesso!");
      setBorrowerName("");
      setSelectedBook("");
      loadBooks();
    } catch (error) {
      alert("Erro ao emprestar livro");
    } finally {
      setSubmitting(false);
    }
 }

 if (loading) {
    return <p className="text-center text-gray-500">Carregando livros disponíveis...</p>;
  }

 return (
    <div className="max-w-3xl mx-auto">
        <div className="mb-8 flex items-center gap-4">
            <div className="bg-gradient-to-br from-pink-400 to-pink-600 p-3 rounded-xl text-white text-2xl">↪️</div>
            <div>
              <h1 className="text-3xl font-bold text-gray-900">Registrar Empréstimo</h1>
              <p className="text-gray-600 text-sm mt-1">Registre um novo empréstimo de livro</p>
            </div>
        </div>

        {books.length === 0 ? (
          <div className="text-center py-16 bg-white rounded-2xl shadow-md border border-gray-100">
            <p className="text-gray-500 text-lg font-semibold">📭 Nenhum livro disponível</p>
            <p className="text-gray-400 mt-2">Todos os livros estão emprestados ou não há novos livros disponíveis.</p>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="bg-white p-8 rounded-2xl shadow-lg space-y-6 border border-gray-100">
            <div>
              <label htmlFor="book" className="block text-sm font-semibold text-gray-700 mb-2">
                Selecione um Livro *
              </label>
              <select
                id="book"
                value={selectedBook}
                onChange={(e) => setSelectedBook(e.target.value)}
                className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl focus:outline-none focus:border-pink-500 focus:ring-2 focus:ring-pink-200 transition-colors cursor-pointer bg-white font-medium"
                required
                >
                <option value="" className="text-gray-500">📚 Escolha um livro disponível...</option>
                {books.map((book) => (
                  <option key={book.code} value={book.code}>
                    {book.code} — {book.title} — {book.author}
                  </option>
                ))}
                </select>
            </div>

            <div>
              <label htmlFor="borrowerName" className="block text-sm font-semibold text-gray-700 mb-2">
                Nome do Responsável *
              </label>
              <input
                type="text"                
                value={borrowerName}
                id="borrowerName"
                placeholder="Ex: João da Silva"
                onChange={(e) => setBorrowerName(e.target.value)}
                className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl focus:outline-none focus:border-pink-500 focus:ring-2 focus:ring-pink-200 transition-colors font-medium"
                required
              />
            </div>

            <button
              type="submit"
              disabled={submitting}
              className="w-full bg-gradient-to-r from-pink-500 to-pink-600 text-white py-3 px-6 rounded-xl hover:from-pink-600 hover:to-pink-700 focus:outline-none focus:ring-2 focus:ring-pink-500 focus:ring-offset-2 disabled:opacity-50 font-bold transition-all shadow-lg hover:shadow-xl transform hover:-translate-y-0.5"
            >
              {submitting ? '⏳ Registrando...' : '✓ Registrar Empréstimo'}
            </button>
          </form>
        )}
    </div>
    );
}
