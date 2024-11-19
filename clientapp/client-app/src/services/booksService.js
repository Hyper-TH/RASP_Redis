import { fetchAllBooks, fetchBook } from "./fetchData";

export const fetchBooks = async () => {
    return fetchAllBooks(`/books`);
};

export const fetchOneBook = async (isbn) => {
    return fetchBook(`/books/${isbn}`);
};