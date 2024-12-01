import { Link } from "react-router-dom";
import { useEffect, useState } from 'react';
import Books from '../components/props/Books.jsx';
import Error from '../components/props/Error.jsx';
import { fetchBooks, deleteBook } from '../services/apiService.js';
import FindBookForm from '../components/forms/FindBookForm.jsx';
import PropTypes from "prop-types";
import ReturnButton from "../components/ReturnButton.jsx";

const BooksPage = ({ backTo }) => {
    const [booksData, setBooksData] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await fetchBooks();

                setBooksData(data);
            } catch (err) {
                setError(err);
            }
        };

        fetchData();
    }, []);

    const removeBook = async (ISBN) => {
        try {
            await deleteBook(ISBN);

            const updatedBooks = await fetchBooks();
            setBooksData(updatedBooks); 

            console.log(`Book with ISBN ${ISBN} removed successfully.`);
        } catch (err) {
            console.error('Error:', err);
            setError(err);

            alert('Error removing book.');
        }

    };
    return (
        <>
            <Link to={backTo}>
                <ReturnButton />
            </Link>

            <h1 className="main_title">
                List of books
            </h1>

            {booksData.map((book) => {
                return (
                    <Books
                        key={book.Id}
                        id={book.Id}
                        isbn={book.ISBN}
                        name={book.Name}
                        price={book.Price}
                        category={book.Category}
                        author={book.Author}
                        deleteBook={removeBook}
                    />
                )
            })}

            <FindBookForm />

            {error && <Error error={error} />}
        </>
    )
}

BooksPage.propTypes = {
    backTo: PropTypes.string.isRequired,  // backTo must be a string and is required
};

export default BooksPage;