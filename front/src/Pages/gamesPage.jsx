import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Cookies from "js-cookie";
import { axiosInstance } from "../Components/axios_server";
import "../assets/css/gamesPage.css"
import Popup from "../Components/Popup";


const GamesPage = () => {
    const navigate = useNavigate();
    const token = Cookies.get('token');
    const [numberOfFetches, setNumberOfFetches] = useState(0)
    const [isPopupOpen, setPopupOpen] = useState(false);
    const [games, setGames] = useState([])

    const fetchGames = (e) => {
        console.log(e.currentTarget.scrollTop)
        if (e.currentTarget.scrollTop >= -1) {
            // if (true) {

            axiosInstance.get(`/getAllRooms/` + numberOfFetches,
        {
           headers:{
               Authorization: `Bearer ${token}`,
               Accept : "application/json"
           }
        }) 
        .then(response => {
            console.log(response.data.value);
            setGames(prev => [ ...prev, ...response.data.value])
            setNumberOfFetches(numberOfFetches + 1)
        })
        .catch()
            
        }
    }

    useEffect(() => {
        axiosInstance.get(`/getAllRooms/` + numberOfFetches,
        {
           headers:{
               Authorization: `Bearer ${token}`,
               Accept : "application/json"
           }
        }) 
        .then(response => {
            console.log(response.data.value);
            setGames(prev => [ ...prev, ...response.data.value])
            setNumberOfFetches(numberOfFetches + 1)
        })
        .catch()
            
        }, [])

    useEffect(() => {
        if (!token){
            navigate("/login");
        }
    }, [navigate, token])

    function CreateGame(){

    }

    const togglePopup = () => {
        setPopupOpen(!isPopupOpen);
      };

    return (
        <div className='main-content' onScroll={e => fetchGames(e) }>
            <div className='buttons'>
                <button onClick={(e) => setPopupOpen(true)}>Create game</button>
                {isPopupOpen && <Popup onClose={togglePopup} />}
            </div>
            <div className='items-container'>
                <div className='items-container-title'>Available games</div>
                <div>
                    {
                        games.map(game => (
                            <div key={ game.id } className='item'>
                                <div>{ game.id }</div>
                                <div>{ game.username }</div>
                                <div>{ game.createdAt }</div>
                                <div className='enter-message'>{ game.enterMessage }</div>
                            </div>
                        ))
                    }
                </div>
            </div>
        </div>
    )

}

export default GamesPage;