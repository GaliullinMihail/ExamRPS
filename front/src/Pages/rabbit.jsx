import React, { useEffect, useState } from "react";
import Stomp from "stompjs";

export default function Messages() {
    var client = Stomp.client('ws://127.0.0.1:15674/ws'); // Обратите внимание, что порт для STOMP может отличаться от обычного WebSocket порта
    
    client.connect("rabbitsa", "mypa55w0rd!", function (frame) {
      console.log('Успешное соединение: ' + frame);
      // Далее можно выполнять действия при успешном соединении
    }, function (error) {
      console.log('Ошибка подключения: ' + error);
    });
}