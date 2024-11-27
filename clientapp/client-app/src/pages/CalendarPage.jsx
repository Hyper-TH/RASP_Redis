import { useState } from 'react';
import { Link } from "react-router-dom";
import PropTypes from "prop-types";
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import ReturnButton from '../components/ReturnButton';

const CalendarPage = ({ backTo }) => {
    const [date, setDate] = useState(new Date());
    const [darkMode, setDarkMode] = useState(false);

    const onChange = (newDate) => {
        setDate(newDate);
    };

    const toggle = () => {
        setDarkMode(!darkMode);
    };

    return (
        <>
            <Link to={backTo}>
                <ReturnButton />
            </Link>

            <div className={darkMode ? 'dark-mode' : ''}>
                <button onClick={toggle}>
                    Toggle {darkMode ? 'Light' : 'Dark'} Mode
                </button>

                <Calendar onChange={onChange} value={date} />
            </div>

            <div>
                <p>Selected date: {date?.toDateString()}</p>
            </div>
        </>
    )
};

CalendarPage.propTypes = {
    backTo: PropTypes.string.isRequired,  // backTo must be a string and is required
};

export default CalendarPage;