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
    const [roomId, setRoomId] = useState(null);
    const [winner, setWinner] = useState('');
    const [winnerChoice, setWinnerChoice] = useState('');
    const [looser, setLooser] = useState('');
    const [connection, setConnection] = useState(null);
    const [seconds, setSeconds] = useState(0);

    useEffect(() => {
        if (!token){
            navigate("/login");
        }
    }, [navigate])

    useEffect(() => {
        setRoomId(location.pathname.split('/game/'[1]));
        callbackSignalR(location.pathname.split('/game/'[1]));
    },[])

    const callbackSignalR = useCallback((roomData) => {
        
        let newConnection = new signalR.HubConnectionBuilder().withUrl(`${ServerURL}/ChatHub`).build();

        newConnection.on("ReceiveGameMessage", function (senderUserId, sign){
            setOpponentChoice(sign);
            if (playerChoice !== '')
                calculateResult();
        });

        newConnection.on("StartGame", function (){
            RefreshGame();
        });

        newConnection.on("ReceiveResultMessage", function (firstPlayerName, secondPlayerName, result){
            if (result === 1) {
                setWinner(firstPlayerName);
                setWinnerChoice(playerChoice);
                setLooser(secondPlayerName);
            } else if (result === 0) {
                setIsDraw(true);
            } else {
                setWinner(secondPlayerName);
                setWinnerChoice(opponentChoice);
                setLooser(firstPlayerName);
            }
        });

        newConnection.start().then(res => {newConnection.invoke("ConnectToRoom", `${roomData}`)
        .catch(function (err) {
            return console.error(err.toString());
        })});

        const buttons = document.getElementsByName('gameButton');
        buttons.forEach(button => {
            button.addEventListener("click", function (event) {
                setPlayerChoice(button.value);
        
                newConnection.invoke("SendGameMessage", `${roomData.senderName}`, `${button.value}`, `${roomData.roomName}`)
                    .catch(function(err){
                        return console.error(err.toString());
                    });

                if (opponentChoice !== '')
                    calculateResult();

                event.preventDefault();
            });
        })
        setConnection(newConnection);
        // document.getElementById("sendButton").addEventListener("click", function (event) { 
        //     var message = document.getElementById("messageInput").value;
        //     document.getElementById("messageInput").value ='';
        //     connection.invoke("SendGameMessage", `${roomData.senderName}`, message, `${roomData.receiverName}`, `${roomData.roomName}`).catch(function (err) { 
        //         return console.error(err.toString());
        //     });
        //     event.preventDefault();
        // });
    }, [])
    
    const calculateResult = () => {
        let room = axiosInstance.get(`/games/${roomId}`,
        {
            headers:{
                Authorization: `Bearer ${token}`,
                Accept : "application/json"
            }
         });

        if (uid === room.value.firstPlayerId)
        {
            
            let result = 0;
            if (playerChoice === "Rock" && opponentChoice === "Scissors") {
                result = 1;
            } else if (playerChoice === "Scissors" && opponentChoice === "Paper") {
                result = 1;
            } else if (playerChoice === "Paper" && opponentChoice === "Rock") {
                result = 1;
            } else if (playerChoice === opponentChoice) {
                result = 0;
            } else {
                result = -1;
            }

            connection.invoke("SendResultMessage", result, `${roomId}`);
        }
        
        setIsWaitingForAny(false);
        
        const timeout = setTimeout(() => { }, 6000);
        
        RefreshGame();
      };

    const RefreshGame = () => {
        setWinner('');
        setLooser('');
        setIsWaitingForAny(true);
        setIsDraw(false);
        setSeconds(0);
    }

    useEffect(() => {
        const interval = setInterval(() => {
          setSeconds(prevSeconds => prevSeconds + 1);
        }, 1000);
    
        if (seconds === 10 && (playerChoice === '' || opponentChoice === '')) {
            if (playerChoice === '')
            {
                setPlayerChoice('Scissors');
            }
            if (opponentChoice === '')
            {
                setOpponentChoice('Scissors');
            }

        }
    
        return () => clearInterval(interval);
      }, [seconds]);

    return(
        <>
            <div>
                <button name = 'gameButton' value = "Rock">
                    Rock
                </button>
                <button name = 'gameButton' value = "Paper">
                    Paper
                </button>
                <button name = 'gameButton' value = "Scissors">
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
        </>
    )
}

export default GamePage;