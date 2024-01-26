import React, { useState } from 'react';
import '../assets/css/popup.css';
import Cookies from "js-cookie";
import { axiosInstance } from "../Components/axios_server";
import { useNavigate } from 'react-router-dom';

const Popup = ({ onClose }) => {
  const navigate = useNavigate();
  const [inputValue, setInputValue] = useState('');
  const token = Cookies.get('token');

  const handleInputChange = (e) => {
    setInputValue(e.target.value);
  };

  const handleSubmit = () => {
    if (isNaN(parseInt(inputValue, 10)) || !/^\d+$/.test(inputValue))
    {
      setInputValue(0);
      alert("value should be int");
      return;
    }

    axiosInstance.post('/createGame', { 
      MaxRating: parseInt(inputValue,10) 
  }, {
      headers:{
          Authorization: `Bearer ${token}`,
          Accept : "application/json"
      }
  })
  .then(res => {
    console.log(res);
    navigate("/game/" + res.data.roomId)
      
  })
  .catch();

  };

  return (
    <div className="popup-container">
      <div className="popup">
        <input
          type="text"
          value={inputValue}
          onChange={handleInputChange}
          placeholder="Введите значение"
        />
        <button onClick={handleSubmit}>Создать</button>
        <button onClick={onClose}>Закрыть</button>
      </div>
    </div>
  );
};

export default Popup;