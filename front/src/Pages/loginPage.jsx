import {useState, useEffect} from "react"; 
import Cookies from 'js-cookie'; 
import {axiosInstance} from "../Components/axios_server"; 
import TokenName from "../Components/token_constant_name";
import { useNavigate } from 'react-router-dom';
const LoginPage = () => {
    const [userName, setUserName] = useState('') 
    const [password, setPassword] = useState('') 
    const [passError, setPassError] = useState('');
    const [nicknameError, setNicknameError] = useState('');
    const navigate = useNavigate()

    useEffect(() => {
        if (Cookies.get(TokenName)){
            navigate('/games');
        }
    }, [navigate]);

    const onSubmit = (e) => { 
        e.preventDefault(); 

        if(userName === '')
        {
            setNicknameError('Логин не может быть пустой');
            return;
        }

        if(password === '')
        {
            setPassError('Пароль не может быть пустой');
            return;
        }
 
        axiosInstance.post('/login', { 
            userName: userName, 
            password: password, 
        }) 
        .then((res) => { 
            if (!res.data.successful){ 
                alert("Неправильный логин или пароль");
            } 
            else{ 
                Cookies.set(TokenName, res.data.message, {expires: 1}); 
                document.location.replace(`/main`);
            } 
        }) 
        .catch((err) => { 
            alert("Ошибка при входе");
        }); 
    }; 

    const handleUserNameChange = (e) => {
        setNicknameError('');
        setUserName(e.target.value);
    }

    const handlePasswordChange = (e) => {
        setPassError('');
        setPassword(e.target.value);
    }

return(
    <form>
        <div className="form-group"> 
            <label >Nickname</label> 
            <input type="text" 
                className="my-form-control" 
                name="UserName" 
                onChange={(e) => handleUserNameChange(e)} 
                placeholder="Enter Your Nickname" /> 
            <span>{nicknameError}</span>
        </div> 
        <div className="form-group"> 
            <label >Password</label> 
            <input type="password" 
                className="my-form-control" 
                name="Password" 
                onChange={(e) => handlePasswordChange(e)} 
                placeholder="Enter Your Password" /> 
            <span>{passError}</span>
        </div>

        <div className="text-center"> 
            <button type="submit" className="default-btn" onClick={onSubmit}><span>Sign IN</span></button> 
        </div>            
    </form>
    )
}

export default LoginPage;