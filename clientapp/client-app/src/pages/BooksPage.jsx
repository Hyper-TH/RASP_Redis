import { useEffect, useState } from 'react';
import Books from '../components/Books.jsx';

const BooksPage = () => {
    const [booksData, setBooksData] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch('http://localhost:5275/api/books');
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                const data = await response.json();
                setBooksData(data);
            } catch (err) {
                console.error('Error fetching books data:', err);
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
        </>
    )
}

export default BooksPage;