import { useState } from 'react';
import Error from './props/Error.jsx';
import { addBook } from '../services/booksService.js';

const AddBookForm = () => {
    const [bookData, setBookData] = useState({
        ISBN: '',
        Name: '',
        Price: 0.0,
        Category: '',
        Author: ''
    });
    const [error, setError] = useState(null);

    const handleChange = (e) => {
        const { name, value } = e.target;

        setBookData((prevBook) => ({
            ...prevBook,
            [name]: value
        }))
    };

    const putData = async () => {
        try {
            await addBook(bookData);
        } catch (err) {
            console.error('Error:', err);
            setError(err);

            alert('Error adding book.');
        }
    };

    return (
        <>
            <div className='sub_container'>
                <div className='add_book_form'>
                    <div className='add_book'>
                        <h1 className='add_book_title'>
                            Add New book
                        </h1>

                        <form className='form'>
                            <div>
                                <label>ISBN</label>
                                <input
                                    type="text"
                                    name="ISBN"
                                    placeholder="Input unique ISBN here"
                                    value={bookData.ISBN}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div>
                                <label>Title</label>
                                <input
                                    type="text"
                                    name="name"
                                    placeholder="Input title here"
                                    value={bookData.Name}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div>
                                <label>Price</label>
                                <input
                                    type="number"
                                    name="price"
                                    placeholder="Input price here"
                                    value={bookData.price}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div>
                                <label>Category</label>
                                <input
                                    type="text"
                                    name="Category"
                                    placeholder="Input category here"
                                    value={bookData.Category}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div>
                                <label>Author</label>
                                <input
                                    type="text"
                                    name="name"
                                    placeholder="Input author here"
                                    value={bookData.Author}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <button className='submit' onClick={putData}>Submit</button>
                        </form>
                    </div>
                </div>

                {error && <Error error={error} />}
            </div>
        </>
    )
};

export default AddBookForm;