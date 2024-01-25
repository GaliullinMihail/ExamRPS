import {useState, useEffect} from "react"; 
import Cookies from 'js-cookie'; 
import {axiosInstance} from "../Components/axios_server"; 
import TokenName from "../Components/token_constant_name";
import { useNavigate } from 'react-router-dom';
import "../assets/css/ratingPage.css"

const RatingPage = () => {
    
    const [ratings, setRating] = useState([]);
    const token = Cookies.get(TokenName);

    useEffect(() => { 
        axiosInstance.get(`/all`,
        {
           headers:{
               Authorization: `Bearer ${token}`,
               Accept : "application/json"
           }
        }) 
        .then(response => {
            setRating(response.data.value);
        })
        .catch(); 
    }, [token])
    

    const navigate = useNavigate()
    useEffect(() => {
        if (!token){
            navigate("/login");
        }
    }, [navigate])


        return (
            <>
            {ratings.map((user, index) => (
                <div className={index % 2 == 0? "userEven" : "userOdd"}>
                    <span className="index">{index + 1}</span>
                    <span className="login">{user.key}</span>
                    <span className="rating">{user.rating}</span>
                </div>

            ))}
            </>

        )

}

export default RatingPage;