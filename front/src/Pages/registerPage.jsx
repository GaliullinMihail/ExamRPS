import {useState, useEffect} from "react"; 
import Cookies from 'js-cookie'; 
import {axiosInstance} from "../Components/axios_server"; 
import TokenName from "../Components/token_constant_name";
import { useNavigate } from 'react-router-dom';
const RegisterPage = () => {
    const [userName, setUserName] = useState('') 
    const [password, setPassword] = useState('') 
    const [confPass, setConfPass] = useState('');
    const [passError, setPassError] = useState('');
    const [nicknameError, setNicknameError] = useState('');
    const navigate = useNavigate()

    useEffect(() => {
        if (Cookies.get('token')){
            navigate('/games');
        }
    }, [navigate]);

    const ValidatePass = (e) => {
        setConfPass(e.target.value)
        if (e.target.value !== password){
            setPassError("Пароли не совпадают!")
        } else {
            setPassError('')
            setConfPass(e.target.value)
        }
    } 
    
    const handleUserNameChange = (e) => {
        setNicknameError('');
        setUserName(e.target.value);
    }


    const onSubmit = (e) => { 
        e.preventDefault(); 
        if (password !== confPass){
            setPassError('Пароли не совпадают!');
            return;
        }
        if (password === ''){
            setPassError('Пароль не может быть пустой');
            return;
        }
        if (userName === '')
        {
            setNicknameError('Логин не указан');
            return;
        }

 
        axiosInstance.post('/registration', { 
            userName: userName, 
            password: password, 
        }) 
        .then((res) => { 
            console.log(res);
            if (!res.data.successful){ 
                console.log(res)
            } 
            else{ 
                document.location.replace(`/login`);
            } 
        }) 
        .catch((err) => { 
            console.log(err);
            alert("Ошибка при регистрации");
        }); 
    }; 

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
                onChange={(e) => setPassword(e.target.value)} 
                placeholder="Enter Your Password" /> 
        </div>
        <div className="form-group"> 
            <label >Confirm Password</label> 
            <input type="password" 
                className="my-form-control" 
                name="Confirm Password" 
                onChange={(e) => ValidatePass(e)} 
                placeholder="Confirm Your Password" /> 
            <span>{passError}</span>
        </div>

        <div className="text-center"> 
            <button type="submit" className="default-btn" onClick={onSubmit}><span>Sign UP</span></button> 
        </div>            
    </form>
    )
}

export default RegisterPage;