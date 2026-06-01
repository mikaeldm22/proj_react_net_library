'use client'

import {useEffect, useState} from "react";
import {useRouter} from "next/navigation";
import { libraryService, CreateBookData} from "@/services/libraryService";

export default function CadastroPage() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [form, setForm] = useState<CreateBookData>({
    title: '',
    author: '',
    publisher: '',
    year: new Date().getFullYear(),
    genre: '',
    code: '',
  });

  async function handleSubmit(event: React.FormEvent) {
    event.preventDefault();

    if (!form.title.trim() || !form.author.trim()) {
      alert('Título e autor são obrigatórios.');
      return;
    }

    setLoading(true);
    try {
      await libraryService.createBook(form);
      alert('Livro cadastrado com sucesso!');
      router.push('/acervo');
    } catch (error) {
      alert('Erro ao cadastrar livro.');
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="max-w-3xl mx-auto">
      <div className="mb-8 flex items-center gap-4">
        <div className="bg-gradient-to-br from-purple-400 to-purple-600 p-3 rounded-xl text-white text-2xl">➕</div>
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Cadastrar Novo Livro</h1>
          <p className="text-gray-600 text-sm mt-1">Adicione um novo livro ao acervo da biblioteca</p>
        </div>
      </div>

      <form onSubmit={handleSubmit} className="bg-white rounded-2xl shadow-lg p-8 space-y-6 border border-gray-100">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label htmlFor="title" className="block text-sm font-semibold text-gray-700 mb-2">
              Título *
            </label>
            <input
              type="text"
              id="title"
              value={form.title}
              onChange={(e) => setForm({...form, title: e.target.value})}
              className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl shadow-sm hover:border-purple-300 focus:outline-none focus:border-purple-500 focus:ring-2 focus:ring-purple-200 transition-colors"
              placeholder="Ex: O Senhor dos Anéis"
              required
            />
          </div>

          <div>
            <label htmlFor="code" className="block text-sm font-semibold text-gray-700 mb-2">
              Código *
            </label>
            <input
              type="text"
              id="code"
              value={form.code}
              onChange={(e) => setForm({...form, code: e.target.value})}
              className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl shadow-sm hover:border-purple-300 focus:outline-none focus:border-purple-500 focus:ring-2 focus:ring-purple-200 transition-colors"
              placeholder="Ex: LIV-001"
              required
            />
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label htmlFor="author" className="block text-sm font-semibold text-gray-700 mb-2">
              Autor *
            </label>
            <input
              type="text"
              id="author"
              value={form.author}
              onChange={(e) => setForm({...form, author: e.target.value})}
              className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl shadow-sm hover:border-purple-300 focus:outline-none focus:border-purple-500 focus:ring-2 focus:ring-purple-200 transition-colors"
              placeholder="Ex: J.R.R. Tolkien"
              required
            />
          </div>

          <div>
            <label htmlFor="publisher" className="block text-sm font-semibold text-gray-700 mb-2">
              Editora
            </label>
            <input
              type="text"
              id="publisher"
              value={form.publisher}
              onChange={(e) => setForm({...form, publisher: e.target.value})}
              className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl shadow-sm hover:border-purple-300 focus:outline-none focus:border-purple-500 focus:ring-2 focus:ring-purple-200 transition-colors"
              placeholder="Ex: Editora Globo"
            />
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label htmlFor="year" className="block text-sm font-semibold text-gray-700 mb-2">
              Ano de Publicação
            </label>
            <input
              type="number"
              id="year"
              value={form.year || ''}
              onChange={(e) => setForm({...form, year: e.target.value ? parseInt(e.target.value) : '' as unknown as number})}
              className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl shadow-sm hover:border-purple-300 focus:outline-none focus:border-purple-500 focus:ring-2 focus:ring-purple-200 transition-colors"
              placeholder="Ex: 1954"
            />
          </div>

          <div>
            <label htmlFor="genre" className="block text-sm font-semibold text-gray-700 mb-2">
              Gênero
            </label>
            <input
              type="text"
              id="genre"
              value={form.genre}
              onChange={(e) => setForm({...form, genre: e.target.value})}
              className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl shadow-sm hover:border-purple-300 focus:outline-none focus:border-purple-500 focus:ring-2 focus:ring-purple-200 transition-colors"
              placeholder="Ex: Ficção Científica"
            />
          </div>
        </div>

        <div className="flex gap-4 pt-4">
          <button
            type="submit"
            disabled={loading}
            className="flex-1 bg-gradient-to-r from-purple-500 to-purple-600 text-white px-6 py-3 rounded-xl hover:from-purple-600 hover:to-purple-700 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:ring-offset-2 disabled:opacity-50 font-semibold transition-all shadow-lg hover:shadow-xl transform hover:-translate-y-0.5"
          >
            {loading ? '⏳ Cadastrando...' : '✓ Cadastrar Livro'}
          </button>
          <button
            type="button"
            onClick={() => router.push('/acervo')}
            className="flex-1 bg-gray-100 text-gray-700 px-6 py-3 rounded-xl hover:bg-gray-200 focus:outline-none focus:ring-2 focus:ring-gray-400 focus:ring-offset-2 font-semibold transition-all"
          >
            Cancelar
          </button>
        </div>
      </form>
    </div>
  );
}
