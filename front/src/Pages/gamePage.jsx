import React, { useCallback, useState, useEffect } from 'react';
import {jwtDecode} from "jwt-decode";
import Cookies from "js-cookie";
import { axiosInstance } from "../Components/axios_server.js";
import * as signalR from "@microsoft/signalr";
import { useNavigate, useLocation } from "react-router-dom";
import ServerURL from '../Components/server_url';
import './../assets/css/gamePage.css'
import { clear } from '@testing-library/user-event/dist/clear.js';

const GamePage = () => {
    const navigate = useNavigate();
    const token = Cookies.get('token');
    const location = useLocation();
    const uid = jwtDecode(token).Id;
    const [isWaitingForAny, setIsWaitingForAny] = useState(false);
    const [isDraw, setIsDraw] = useState(false);
    const [playerChoice, setPlayerChoice] = useState('');
    const [opponentChoice, setOpponentChoice] = useState('');
    const [winner, setWinner] = useState('');
    const [winnerChoice, setWinnerChoice] = useState('');
    const [looser, setLooser] = useState('');
    const [connection, setConnection] = useState(null);
    const [seconds, setSeconds] = useState(0);
    const [oponnentExists, setOponnentExists] = useState(false);
    const [room, setRoom] = useState(null);
    const [isWatcher, setIsWatcher] = useState(true);

    useEffect(() => {
        if (!token){
            navigate("/login");
        }
    }, [navigate, token])

    const calculateResult = (myChoice, enemyChoice) => {
        if (uid === room.firstPlayerId)
        {
            let result = 0;
            if (myChoice === "Rock" && enemyChoice === "Scissors") {
                result = 1;
            } else if (myChoice === "Scissors" && enemyChoice === "Paper") {
                result = 1;
            } else if (myChoice === "Paper" && enemyChoice === "Rock") {
                result = 1;
            } else if (myChoice === enemyChoice) {
                result = 0;
            } else {
                result = -1;
            }

            connection.invoke("SendResultMessage", result, `${room.id}`);
        }
      };

    const callbackSignalR = useCallback((roomData) => {
        let newConnection = connection;

        if(!connection || connection.state === signalR.HubConnectionState.Disconnected)
        {
            newConnection = new signalR.HubConnectionBuilder().withUrl(`${ServerURL}/ChatHub`).build();
            newConnection.start().then(res => {newConnection.invoke("ConnectToRoom", `${roomData}`)
            .catch(function (err) {
            return console.error(err.toString());
        })})
        }

        newConnection.off('ReceiveGameMessage')
        newConnection.off('StartGame')
        newConnection.off('ReceiveResultMessage')
        newConnection.off('EndGame')
        
        newConnection.on('EndGame', function(){
            navigate('/games');
        })

        newConnection.on("ReceiveGameMessage", function (senderUserId, sign){

            if (isWatcher)
            {
                if (senderUserId !== room.firstPlayerId)
                {
                    setPlayerChoice(sign);
                }
                else
                {
                    setOpponentChoice(sign);
                }
            } 
            else if (senderUserId !== uid)
            {
                setOpponentChoice(sign);
                if (playerChoice !== '')
                    calculateResult(playerChoice, sign);
            }
        });

        newConnection.on("StartGame", function (){
            RefreshGame();
        });

        newConnection.on("ReceiveResultMessage", function (firstPlayerName, secondPlayerName, result){
            if(isWatcher && !oponnentExists)
            {
                console.log("refresh");
                RefreshGame();
            }
            const isOwner = uid === room.firstPlayerId;
            if (result === 1) {
                setWinner(firstPlayerName);
                setWinnerChoice(isOwner ? playerChoice : opponentChoice);
                setLooser(secondPlayerName);
            } else if (result === 0) {
                setIsDraw(true);
            } else {
                setWinner(secondPlayerName);
                setWinnerChoice(isOwner ? opponentChoice : playerChoice);
                setLooser(firstPlayerName);
            }

            setIsWaitingForAny(false);

            setTimeout(() => {
                RefreshGame();
              }, 3000);
        });

        setConnection(newConnection);
    }, [uid, opponentChoice, playerChoice, oponnentExists])

    useEffect(() => {
        callbackSignalR(location.pathname.split('/game/'[0])[2]);
        axiosInstance.get(`/games/`+location.pathname.split('/game/'[0])[2],
        {
            headers:{
                Authorization: `Bearer ${token}`,
                Accept : "application/json"
            }
         }).then(res => {
            setRoom(res.data.value);
            setIsWatcher(uid !== res.data.value.firstPlayerId && uid !== res.data.value.secondPlayerId)
            try{
            window.addEventListener('beforeunload', () => {
                
                connection.invoke('LeaveFromRoom', `${uid}`, `${location.pathname.split('/game/'[0])[2]}`).catch()
            })
            }
            catch{
                
            }
         });
    },[location.pathname, token, callbackSignalR])

    

    const SendGameMessage = (value) => {
        setPlayerChoice(value);
        connection.invoke("SendGameMessage", `${uid}`, `${value}`, `${room.id}`);
        if (opponentChoice !== '')
            calculateResult(value, opponentChoice);
    }
    
    

    const RefreshGame = () => {
        setIsWaitingForAny(true);
        setWinner('');
        setLooser('');
        setIsDraw(false);
        setSeconds(0);
        setOponnentExists(true);
        setPlayerChoice('');
        setOpponentChoice('');
        setWinnerChoice('');
    }

    return(
        <>
            {oponnentExists? 
            <div>
                {isWaitingForAny
                    ? <div>
                            {isWatcher || playerChoice != ''? <></>
                            :
                            <div> 
                            <button name = 'gameButton' onClick={() => SendGameMessage("Rock")} value = "Rock">
                                Rock
                            </button>
                            <button name = 'gameButton' onClick={() => SendGameMessage("Paper")} value = "Paper">
                                Paper
                            </button>
                            <button name = 'gameButton' onClick={() => SendGameMessage("Scissors")} value = "Scissors">
                                Scissors
                            </button>                       
                            </div>}
                            <p>Waiting for players to make a choice...</p>
                        </div>
                    : isDraw 
                        ?
                        <>
                            <p className='gray'> It's Draw (+1)</p>
                        </>
                        :
                        <>
                            <p className='green'> {winner} is Winner! (+3), choice: {winnerChoice}</p>
                            <p className='red'> {looser} is Looser! (-1), choice: {winnerChoice === playerChoice ? opponentChoice: playerChoice}</p>
                        </>
                }
            </div>
            : 
            <div>
                <p>Waiting for oponnent</p>
            </div>
            }
            
        </>
    )
}

export default GamePage;