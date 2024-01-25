import React, { useState, useEffect } from 'react';

const RabbitMQQueueList = () => {
  const [queueList, setQueueList] = useState([]);

  useEffect(() => {
    const fetchQueueList = async () => {
      try {
        const response = await fetch('http://your-rabbitmq-server/api/queues', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            // Добавьте любые необходимые заголовки аутентификации или токены здесь
          },
        });

        if (response.ok) {
          const data = await response.json();
          setQueueList(data);
        } else {
          console.error('Failed to fetch queue list');
        }
      } catch (error) {
        console.error('Error fetching queue list:', error);
      }
    };

    fetchQueueList();
  }, []);

  return (
    <div>
      <h1>Queue List</h1>
      <ul>
        {queueList.map((queue) => (
          <li key={queue.name}>{queue.name}</li>
        ))}
      </ul>
    </div>
  );
};

export default RabbitMQQueueList;
