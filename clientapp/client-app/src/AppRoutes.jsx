import { Routes, Route } from 'react-router-dom';
import * as Pages from './RouteImports.js';

const AppRoutes = () => (
    <Routes>
        <Route path="/" element={<Pages.HomePage />} />
        <Route path="/books" element={<Pages.BooksPage />} />
    </Routes>
);

export default AppRoutes;