import { useEffect, useState } from 'react';
import Error from '../components/props/Error.jsx';
import { addBook } from '../services/booksService.js';
import { AddBookForm } from '../components/AddBookForm.jsx';

const AddBookPage = () => {
    return (
        <>
            <AddBookForm />
        </>
    )
}

export default AddBookPage;