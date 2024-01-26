import React, { useCallback, useState, useEffect } from 'react';
import {jwtDecode} from "jwt-decode";
import Cookies from "js-cookie";
import { axiosInstance } from "../Components/axios_server.js";
import * as signalR from "@microsoft/signalr";
import { useNavigate, useLocation } from "react-router-dom";
import ServerURL from '../Components/server_url';

const GamePage = () => {
    const navigate = useNavigate();
    const token = Cookies.get('token');
    const location = useLocation();
    const uid = jwtDecode(token).Id;
    const [sign, setSign] = useState('');

    useEffect(() => {
        if (!token){
            navigate("/login");
        }
    }, [navigate])

    useEffect(() => {
        callbackSignalR(location.pathname.split('/game/'[1]));
    })

    const callbackSignalR = useCallback((roomData) => {

        let connection = new signalR.HubConnectionBuilder().withUrl(`${ServerURL}/ChatHub`).build();
        connection.on("ReceiveGameMessage", function (user, message){
            console.log("normal chat recieved");
            var elem = document.createElement("div");
            var author = document.createElement("span");
            var content = document.createElement("span");
            author.textContent = user + ":";

            content.className = "message-text";
            content.textContent = message;

            elem.appendChild(author);
            elem.appendChild(content);

            document.getElementById("messagesList").appendChild(elem);

        });


        connection.start().then(res => {connection.invoke("ConnectToRoom", `${roomData}`)
        .catch(function (err) {
            return console.error(err.toString());
        })});

        const buttons = document.getElementsByName('gameButton');
        buttons.forEach(button => {
            button.addEventListener("click", function (event) {
                setSign(button.value);
                
                connection.invoke("SendGameMessage", `${roomData.senderName}`, sign, `${roomData.roomName}`).catch(function(err){
                    return console.error(err.toString());
                });
                event.preventDefault();
            });
        })
        // document.getElementById("sendButton").addEventListener("click", function (event) { 
        //     var message = document.getElementById("messageInput").value;
        //     document.getElementById("messageInput").value ='';
        //     connection.invoke("SendGameMessage", `${roomData.senderName}`, message, `${roomData.receiverName}`, `${roomData.roomName}`).catch(function (err) { 
        //         return console.error(err.toString());
        //     });
        //     event.preventDefault();
        // });
    }, [])
    

    return(<>
            <button name = 'gameButton' value = "Rock">
                Rock
            </button>
            <button name = 'gameButton' value = "Paper">
                Paper
            </button>
            <button name = 'gameButton' value = "Scissors">
                Scissors
            </button>

            
    </>)
}

export default GamePage;