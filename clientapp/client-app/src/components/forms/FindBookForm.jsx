import { useState } from "react";
import Error from '../props/Error.jsx';
import Books from '../props/Books.jsx'
import { fetchOneBook } from '../../services/booksService.js';

const FindBookForm = () => {
    const [isbn, setISBN] = useState("");
    const [book, setBookData] = useState({});
    const [error, setError] = useState(null);

    const handleChange = (e) => {
        setISBN(e.target.value);
    };

    const searchBook = async (e) => {
        e.preventDefault();

        try {
            const data = await fetchOneBook(isbn);

            setBookData(data);
        } catch (err) {
            setError(err);
        }
    };

    return (
        <>
            <div className='sub_container'>
                <div className='search_book_form'>
                    <div className='search_book'>
                        <h1 className='search_book_title'>
                            Search for a book using ISBN
                        </h1>

                        <form className='form'>
                            <div>
                                <label>ISBN</label>
                                <input
                                    type="text"
                                    name="isbn"
                                    placeholder="Input ISBN here"
                                    value={isbn}
                                    onChange={handleChange}

                                    required
                                />
                            </div>

                            <button className='submit' onClick={searchBook}>Search</button>
                        </form>
                    </div>
                </div>

                {book?.Id &&
                    <Books
                        key={book.Id}
                        id={book.Id}
                        name={book.Name}
                        price={book.Price}
                        category={book.Category}
                        author={book.Author}
                    />
                }
            </div>

            {error &&
                <Error
                    error={error}
                />
            }
        </>
    )
};

export default FindBookForm;