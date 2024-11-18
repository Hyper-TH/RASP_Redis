import { fetchData } from "./fetchData";

export const fetchBooks = async () => {
    return fetchData(`/books`);
};