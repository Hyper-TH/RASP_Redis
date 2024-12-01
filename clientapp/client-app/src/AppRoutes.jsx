import { Routes, Route } from 'react-router-dom';
import * as Pages from './RouteImports.js';
import ProtectedRoute from './components/auth/ProtectedRoute.jsx';

const AppRoutes = () => (
    <Routes>
        <Route
            path="/"
            element={<Pages.LoginPage />}
        />

        <Route
            path="/signup"
            element={<Pages.SignUpPage />}
        />

        <Route
            path="/home"
            element={<ProtectedRoute element={<Pages.HomePage />} />}
        />

        <Route
            path="/books"
            element={<ProtectedRoute element={<Pages.BooksPage backTo="/home" />} />}
        />

        <Route
            path="/addBook"
            element={<ProtectedRoute element={<Pages.AddBookPage backTo="/home" />} />}
        />

        <Route
            path="/calendar"
            element={<ProtectedRoute element={<Pages.CalendarPage backTo="/home" />} />}
        />

        <Route
            path="/allMeetings"
            element={<ProtectedRoute element={<Pages.MeetingsPage backTo="/home" />} />}
        />

        <Route
            path="/addMeeting"
            element={<ProtectedRoute element={<Pages.AddMeetingPage backTo="/home" />} />}
        />
    </Routes>
);

export default AppRoutes;