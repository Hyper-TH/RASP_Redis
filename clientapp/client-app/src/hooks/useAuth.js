import { useState, useEffect } from 'react';
import axios from 'axios';
const API = import.meta.env.VITE_API;

export const useAuth = () => {
    const [token, setToken] = useState(localStorage.getItem('token') || '');
    const [uID, setUID] = useState(localStorage.getItem('uID') || '');
    const [username, setUsername] = useState(localStorage.getItem('username') || '');
    const [location, setLocation] = useState(localStorage.getItem('location') || '');

    useEffect(() => {
        const savedToken = localStorage.getItem('token');
        const savedUID = localStorage.getItem('uID');
        const savedUsername = localStorage.getItem('username');
        const savedLocation = localStorage.getItem('location');

        if (savedToken && savedToken !== token) setToken(savedToken);
        if (savedUID && savedUID !== uID) setUID(savedUID);
        if (savedUsername && savedUsername !== username) setUsername(savedUsername);
        if (savedLocation && savedLocation !== location) setLocation(savedLocation);
    }, [token, uID, username, location]);

    const login = async (username, password) => {
        try {
            console.log('Attempting login...');
            const response = await axios.post(`${API}/auth/login`, { username, password });
            console.log('Login response:', response.data);

            const { Token, UID, Username, Location } = response.data;

            setToken(Token);
            setUID(UID);
            setUsername(Username);
            setLocation(Location);

            localStorage.setItem('token', Token);
            localStorage.setItem('uID', UID);
            localStorage.setItem('username', Username);
            localStorage.setItem('location', Location);

            console.log('Login successful. Data saved:', { Token, UID, Username, Location });

            return { Token, UID, Username, Location };
        } catch (error) {
            console.error('Login failed:', error);
            throw error; 
        }
    };

    const register = async (username, password) => {
        try {
            await axios.post(`${API}/auth/register`, { username, password });
        } catch (error) {
            console.error(`Registration failed`, error);
        }
    };

    const logout = () => {
        setToken('');
        setUID('');
        setUsername('');
        setLocation('');
        localStorage.removeItem('token');
        localStorage.removeItem('uID');
        localStorage.removeItem('username');
        localStorage.removeItem('location');
    };

    return { token, uID, username, location, login, register, logout };
};