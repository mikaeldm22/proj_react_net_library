import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5180/api',
});

export interface Book {

  code: string;
  title: string;
  author: string;
  publisher: string;
  year: number;
  genre: string;
  status: string;
  createdAt: string;
  borrowedAt: string | null;
  returnedAt: string | null;
  borrowerName: string | null;
}

export interface CreateBookData {
  code: string;
  title: string;
  author: string;
  publisher: string;
  year: number;
  genre: string;
}

export interface BorrowBookData {
  borrowerName: string;
}

export interface BookStats {
  totalBooks: number;
  borrowedBooks: number;
  availableBooks: number;
}

export const libraryService = {

  getAllBooks: async (): Promise<Book[]> => {
    const response = await api.get<Book[]>('/Library');
    return response.data;
  },

  getBookByCode: async (code: string): Promise<Book> => {
    const response = await api.get<Book>(`/Library/${code}`);
    return response.data;
  },

  createBook: async (data: CreateBookData): Promise<Book> => {
    const response = await api.post<Book>('/Library', data);
    return response.data;
  },

  deleteBook: async (code: string): Promise<void> => {
    await api.delete(`/Library/${code}`);
  },

  borrowBook: async (code: string, data: BorrowBookData): Promise<Book> => {
    const response = await api.post<Book>(`/Library/${code}/borrow`, data);
    return response.data;
  },

  returnBook: async (code: string): Promise<Book> => {
    const response = await api.post<Book>(`/Library/${code}/return`);
    return response.data;
  },

  getStats: async (): Promise<BookStats> => {
    const response = await api.get<BookStats>('/Library/stats');
    return response.data;
  },
};
  