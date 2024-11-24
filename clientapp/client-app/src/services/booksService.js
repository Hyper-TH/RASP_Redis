import { fetchAllBooks, fetchBook, postBook} from "./fetchData";

export const fetchBooks = async () => {
    return fetchAllBooks(`/books`);
};

export const fetchOneBook = async (isbn) => {
    return fetchBook(`/books/${isbn}`);
};

export const addBook = async (bookData) => {
    return postBook(`/books`, bookData);
};