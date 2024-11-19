﻿const booksAPI = import.meta.env.VITE_BOOKS_API;

export const fetchAllBooks = async (endpoint, options = {}) => {
    try {
        const response = await fetch(`${booksAPI}${endpoint}`, options);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        return await response.json();
    } catch (err) {
        console.error(`Error fetching data from ${booksAPI}${endpoint}`, err);

        throw err;
    }
};

export const fetchBook = async (endpoint, options = {}) => {
    try {
        const response = await fetch(`${booksAPI}${endpoint}`, options);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        return await response.json();
    } catch (err) {
        console.error(`Error fetching data from ${booksAPI}${endpoint}`, err);

        throw err;
    }
};
