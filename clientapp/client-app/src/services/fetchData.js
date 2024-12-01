const API = import.meta.env.VITE_API;

export const fetchAllBooks = async (endpoint, options = {}) => {
    try {
        const response = await fetch(`${API}${endpoint}`, options);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        return await response.json();
    } catch (err) {
        console.error(`Error fetching data from ${API}${endpoint}`, err);

        throw err;
    }
};

export const fetchBook = async (endpoint, options = {}) => {
    try {
        const response = await fetch(`${API}${endpoint}`, options);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        return await response.json();
    } catch (err) {
        console.error(`Error fetching data from ${API}${endpoint}`, err);

        throw err;
    }
};

export const postBook = async (endpoint, bookData, options = {}) => {
    try {
        const response = await fetch(`${API}${endpoint}`, {
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
        console.error(`Error posting data to ${API}${endpoint}`, err);

        throw err;
    }
}

export const deleteBook = async (endpoint, isbn, options = {}) => {
    try {
        const response = await fetch(`${API}${endpoint}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(isbn),
            ...options
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        return await response.json();
    } catch (err) {
        console.error(`Error deleting data to ${API}${endpoint}`, err);

        throw err;
    }
};

export const postMeeting = async (endpoint, meetingData, token, options = {}) => {
    try {
        const response = await fetch(`${API}${endpoint}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify(meetingData),
            ...options
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        return await response.json();

    } catch (err) {
        console.error(`Error posting data to ${API}${endpoint}`, err);

        throw err;
    }
}