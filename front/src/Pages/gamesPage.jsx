import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Cookies from "js-cookie";
import { axiosInstance } from "../Components/axios_server";
import "../assets/css/gamesPage.css"
import Popup from "../Components/Popup";
import {jwtDecode} from "jwt-decode";


const GamesPage = () => {
    const navigate = useNavigate();
    const token = Cookies.get('token');
    const [numberOfFetches, setNumberOfFetches] = useState(0)
    const [isPopupOpen, setPopupOpen] = useState(false);
    const [games, setGames] = useState([])
    const [loading, setLoading] = useState(false);
    const uName = token? jwtDecode(token).Name: '';
    const uId = token? jwtDecode(token).Id : '';

    useEffect(() => {
        if (!token){
            navigate("/login");
        }
    }, [navigate, token])

    useEffect(() => {
        if(!token)
            return;
        setLoading(true);
        axiosInstance.get(`/games/getAllRooms/` + numberOfFetches,
        {
           headers:{
               Authorization: `Bearer ${token}`,
               Accept : "application/json"
           }
        }) 
        .then(response => {
            setGames(prev => [ ...prev, ...response.data.value])
            setLoading(false);
        })
        .catch()
            
        }, [numberOfFetches])


    const togglePopup = () => {
        setPopupOpen(!isPopupOpen);
      };

    function ParseTime(time){
        const hours = time.getHours();
        const minutes = time.getMinutes();
        const second = time.getSeconds();
        return hours + ':'+ minutes + ':' + second;
    }

    const handleScroll= () => {
        const bottom = Math.ceil(window.innerHeight + window.scrollY) >= document.documentElement.scrollHeight;

        if(bottom && !loading){
            setNumberOfFetches(prev => prev +1);
        }
    }

    useEffect(() => {
        window.addEventListener('scroll', handleScroll);
        return() => {
            window.removeEventListener('scroll', handleScroll);
        }
    }, [handleScroll])

    const JoinGame = (id) =>  {
        axiosInstance.post('games/joinGame/' + id, {},
        {
            headers:{
                Authorization: `Bearer ${token}`,
                Accept : "application/json"
            }
        }).then(response => {
            if(!response.data.successful)
            {
                navigate('/game/' + id)
            }
            else
            {
                alert("вы не можете присоединиться к комнате")
            }
            
        })
    }

    return (    
        <div className='main-content'>
            <div className='buttons'>
                <button className="button_create_game" onClick={(e) => setPopupOpen(true)}>Create game</button>
                {isPopupOpen && <Popup onClose={togglePopup} />}
            </div>
            <div className='games_container'>
                <div className='games_container_title'>Available games</div>
                <div>
                    {
                        games.map(game => (
                            <div>
                            {uName !== game.owner && uId !== game.secondPlayerId &&
                            <div className="game" key = {game.id}> 
                                <div className="game_id">id : { game.id } </div>
                                <div className="game_owner">owner : { game.owner } </div>
                                <div className="game_time">creation time : {ParseTime(new Date(game.creationTime))} </div>
                                <div className="game_rating">max rating : { game.maxRating } </div>
                                <div className="game_oponnent">oponnent : {game.secondPlayerId? "Yes" : "No"} </div>
                                <button onClick={() => JoinGame(game.id)}> Join game </button>
                            </div>
                            }
                            </div>
                        ))
                    }
                </div>
                {loading && <p>Loading ...</p>}
            </div>
        </div>
    )

}

export default GamesPage;