import { useState, useEffect } from 'react';
import axios from 'axios';
const API = import.meta.env.VITE_BOOKS_API;

export const useAuth = () => {
    const [token, setToken] = useState(localStorage.getItem('token') || '');

    useEffect(() => {
        const savedToken = localStorage.getItem('token');
        if (savedToken && savedToken !== token) {
            setToken(savedToken);
        }

    }, [token]);

    const login = async (username, password) => {
        try {
            console.log('Attempting login...');
            const response = await axios.post(`${API}/auth/login`, { username, password });
            console.log('Login response:', response.data);

            const { Token } = response.data;
            setToken(Token);
            localStorage.setItem('token', Token);

            console.log('Token set successfully:', Token);
            return Token; // Return the token after setting it
        } catch (error) {
            console.error('Login failed:', error);
            throw error; // Re-throw the error to be caught in SignIn.jsx
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
        localStorage.removeItem('token');
    };

    return { token, login, register, logout };
};