import { Routes, Route } from 'react-router-dom';
import HeaderApp from './Components/header';
import LoginPage from './Pages/loginPage';
import RegisterPage from './Pages/registerPage';
import GamesPage from './Pages/games';
import './assets/css/App.css';
import RatingPage from './Pages/ratingPage'

function App() {
  return (
    <>
      <HeaderApp />
      <Routes>
        <Route path='/login' element={<LoginPage />} />
        <Route path='/register' element= {<RegisterPage />}/>
        <Route path='/games' element= {<GamesPage />}/>
        <Route path='/rating' element= {<RatingPage />}/>
        <Route path = '*' element={<LoginPage />} 
        />
      </Routes>
    </>
  );
}

export default App;
