import { Routes, Route } from 'react-router-dom';
import * as Pages from './RouteImports.js';

const AppRoutes = () => (
    <Routes>
        <Route
            path="/"
            element={<Pages.HomePage />}
        />

        <Route
            path="/books"
            element={<Pages.BooksPage backTo="/" />}
        />

        <Route
            path="/addBook"
            element={<Pages.AddBookPage backTo="/" />}
        />

        <Route
            path="/calendar"
            element={<Pages.CalendarPage backTo="/" />}
        />
    </Routes>
);

export default AppRoutes;