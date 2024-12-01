import { useState, useEffect } from "react";
import { useAuth } from '../../context/AuthContext';
import Error from '../props/Error';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import { addMeeting } from '../../services/apiService.js';

const AddMeetingForm = () => {
    const { uID, token } = useAuth();
    const [error, setError] = useState(null);
    const [date, setDate] = useState(new Date());
    const [darkMode, setDarkMode] = useState(false);
    const [meetingData, setMeetingData] = useState({
        Name: '',
        Organizer: '',
        Description: '',
        Date: '',
        Timezone: '',
    });

    useEffect(() => {
        if (uID && token) {
            setMeetingData((prevData) => ({
                ...prevData,
                Organizer: uID,
            }));
        }
    }, [uID, token]);


    const handleChange = async (e) => {
        const { name, value } = e.target;

        setMeetingData((prevMeeting) => ({
            ...prevMeeting,
            [name]: value
        }));
    };

    const onChange = (newDate) => {
        setDate(newDate);
        setMeetingData((prevMeeting) => ({
            ...prevMeeting,
            Date: newDate.toISOString(), // Use ISO string for backend compatibility
        }));
    };

    const toggle = () => {
        setDarkMode(!darkMode);
    };

    const putData = async (e) => {
        e.preventDefault();

        try {
            const data = await addMeeting(meetingData);

            console.log(data);
        } catch (err) {
            console.error('Error:', err);
            setError(err);

            alert('Error adding meeting.');
        }
    };

    return (
        <>
            <div className='sub_container'>
                <div className='add_meeting_form'>
                    <div className='add_meeting'>
                        <h1 className='add_meeting_title'>
                            Add New Meeting
                        </h1>

                        <form className='form'>
                            <div>
                                <label>Name</label>
                                <input
                                    type="text"
                                    name="Name"
                                    placeholder="Name of meeting"
                                    value={meetingData.Name}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div>
                                <label>Description</label>
                                <input
                                    type="text"
                                    name="Description"
                                    placeholder="Description of meeting"
                                    value={meetingData.Description}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div>
                                <label>Timezone</label>
                                <input
                                    type="text"
                                    name="Timezone"
                                    placeholder="Input Timezone here"
                                    value={meetingData.Timezone}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div className={darkMode ? 'dark-mode' : ''}>
                                <button onClick={toggle}>
                                    Toggle {darkMode ? 'Light' : 'Dark'} Mode
                                </button>

                                <Calendar onChange={onChange} value={date} />
                            </div>

                            <div>
                                <p>Selected date: {date?.toDateString()}</p>
                            </div>

                            <button className="submit" onClick={putData}>Submit</button>
                        </form>
                    </div>
                </div>

                {error && <Error error={error} />}
            </div>
        </>
    );
};

export default AddMeetingForm;