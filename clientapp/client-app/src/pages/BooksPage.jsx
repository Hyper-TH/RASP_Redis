import { useEffect, useState } from 'react';
import Books from '../components/props/Books.jsx';
import Error from '../components/props/Error.jsx';
import { fetchBooks } from '../services/booksService.js';
import BookForm from '../components/BookForm.jsx';

const BooksPage = () => {
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

    console.log(booksData);

    return (
        <>
            <h1 className="main_title">
                List of books
            </h1>

            {booksData.map((book) => {
                return (
                    <Books
                        key={book.Id}
                        id={book.Id}
                        name={book.Name}
                        price={book.Price}
                        category={book.Category}
                        author={book.Author}
                    />
                )
            })}

            <BookForm />

            {error &&
                <Error
                    error={error}
                />
            }
        </>
    )
}

export default BooksPage;