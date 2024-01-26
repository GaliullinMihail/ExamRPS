import React, { useCallback, useState, useEffect } from 'react';
import {jwtDecode} from "jwt-decode";
import Cookies from "js-cookie";
import { axiosInstance } from "../Components/axios_server.js";
import * as signalR from "@microsoft/signalr";
import { useNavigate, useLocation } from "react-router-dom";
import ServerURL from '../Components/server_url';
import './../assets/css/gamePage.css'

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
        
        setIsWaitingForAny(false);
        
        // RefreshGame();
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

        newConnection.on("ReceiveGameMessage", function (senderUserId, sign){
            
            if (senderUserId !== uid)
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
            const isOwner = uid === room.firstPlayerId;
            console.log("Я ПОЛУЧИЛ РЕЗ","result",isOwner,firstPlayerName, secondPlayerName, result)
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
        });

        setConnection(newConnection);
        // document.getElementById("sendButton").addEventListener("click", function (event) { 
        //     var message = document.getElementById("messageInput").value;
        //     document.getElementById("messageInput").value ='';
        //     connection.invoke("SendGameMessage", `${roomData.senderName}`, message, `${roomData.receiverName}`, `${roomData.roomName}`).catch(function (err) { 
        //         return console.error(err.toString());
        //     });
        //     event.preventDefault();
        // });
    }, [uid, opponentChoice, playerChoice])

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
         });
    },[location.pathname, token, callbackSignalR])

    

    const SendGameMessage = (value) => {
        setPlayerChoice(value);
        connection.invoke("SendGameMessage", `${uid}`, `${value}`, `${room.id}`);
        if (opponentChoice !== '')
            calculateResult(value, opponentChoice);
    }
    
    

    const RefreshGame = () => {
        setWinner('');
        setLooser('');
        setIsWaitingForAny(true);
        setIsDraw(false);
        setSeconds(0);
        setOponnentExists(true);
    }

    // useEffect(() => {
    //     const interval = setInterval(() => {
    //       setSeconds(prevSeconds => prevSeconds + 1);
    //     }, 1000);
    
    //     if (seconds === 10 && (playerChoice === '' || opponentChoice === '')) {
    //         if (playerChoice === '')
    //         {
    //             setPlayerChoice('Scissors');
    //         }
    //         if (opponentChoice === '')
    //         {
    //             setOpponentChoice('Scissors');
    //         }

    //     }
    
    //     return () => clearInterval(interval);
    //   }, [seconds, opponentChoice, playerChoice]);

    return(
        <>
            {oponnentExists? 
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
                {isWaitingForAny
                    ? <p>Waiting for players to make a choice...</p>
                    : isDraw 
                        ?
                        <>
                            <p className='gray'> It's Draw (+1)</p>
                        </>
                        :
                        <>
                            <p className='green'> {winner} is Winner! (+3), choice: {winnerChoice}</p>
                            <p className='red'> {looser} is Looser! (-1), choise: {winnerChoice === playerChoice ? opponentChoice: playerChoice}</p>
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