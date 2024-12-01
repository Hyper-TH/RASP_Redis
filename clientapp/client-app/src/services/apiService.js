import * as Data from './fetchData';

export const fetchBooks = async () => {
    return Data.fetchAllBooks(`/books`);
};

export const fetchOneBook = async (isbn) => {
    return Data.fetchBook(`/books/${isbn}`);
};

export const addBook = async (bookData) => {
    return Data.postBook(`/books`, bookData);
};

export const deleteBook = async (isbn) => {
    return Data.deleteBook(`/books/${isbn}`);
};

export const addMeeting = async (meetingData, token) => {
    return Data.postMeeting(`/ProjectA/meeting`, meetingData, token);
}