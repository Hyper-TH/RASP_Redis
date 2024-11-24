const booksAPI = import.meta.env.VITE_BOOKS_API;

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

export const postBook = async (endpoint, bookData, options = {}) => {
    try {
        const response = await fetch(`${booksAPI}${endpoint}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(bookData),
            ...options
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        return await response.json();
    } catch (err) {
        console.error(`Error posting data to ${booksAPI}${endpoint}`, err);

        throw err;
    }
}

export const deleteBook = async (endpoint, isbn, options = {}) => {
    try {
        ;
    } catch (err) {
        console.error(`Error deleting data to ${booksAPI}${endpoint}`, err);

        throw err;
    }
}