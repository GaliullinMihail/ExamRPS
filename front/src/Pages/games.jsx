import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Cookies from "js-cookie";
import { axiosInstance } from "../Components/axios_server";




const GamesPage = () => {
    const navigate = useNavigate();
    const token = Cookies.get('token');
    const [games, setGames] = useState([]);

    // useEffect(() => {
    //     setGames();
    // }, games)

    useEffect(() => {
        if (!token){
            navigate("/login");
        }
    }, [navigate, token])

}

export default GamesPage;