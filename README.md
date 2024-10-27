# Poll Clash

An easy way to generated online poll on real time.

https://github.com/user-attachments/assets/bd9b255b-569c-4a1e-a5fc-775e51c3eb92

Poll Clash is an easy-to-use, real-time polling platform designed for scalability. Built with Redis and MySQL, the platform supports vertical scalability, ensuring a smooth experience even as usage grows.
By utilizing reverse proxies like Nginx, Poll Clash can handle multiple instances efficiently, distributing the load across the machine's resources.

## To-Do List

- [X] Real-time Polling: Instant updates and results for participants.
- [X] Easy Poll Creation: (In progress)
- [ ] Just one vote per connection (easy way is to control sockets. But it will be easy surpass.) (i dont want to implement auth)
- [ ] Better UI 🤣

## How to Use Poll Clash

Poll Clash offers a real-time polling experience with high scalability using Docker, NGINX, Redis, and MySQL. Here’s how to set it up and get started:

### Prerequisites

1. **Docker and Docker Compose**: Make sure you have Docker and Docker Compose installed on your machine.

### Setup

1. Clone the Poll Clash repository to your local machine.
2. Review the `docker-compose.yml` and `nginx.conf` files to ensure all configurations meet your environment's requirements.

### Running the Application

1. Open your terminal, navigate to the project directory, and run:

   ```bash
   docker-compose up --build
   ```

2. Once Docker Compose finishes building and starting all services, access the application at `http://localhost` in your web browser.

### Services Overview

- **Redis**: In-memory storage for fast data access and caching.
- **MySQL**: Persistent database storage.
- **Backend (API & WebSocket)**: ASP.NET Core backends (two instances) to handle API requests and WebSocket traffic.
- **Client**: React-based frontend for poll creation and participation.
- **NGINX**: Reverse proxy to load balance requests across backend instances and client.

**Note**: Ensure that all network ports in `docker-compose.yml` are available on your system, as conflicts can cause startup issues.

### Docker Compose Configuration

The `docker-compose.yml` file is set to spin up all necessary services. NGINX routes WebSocket and HTTP traffic separately, load balancing between two backend instances.

```yaml
version: "3.8"

services:
  redis:
    image: "redis:7.0"
    container_name: redis
    ports:
      - "6379:6379"
    networks:
      - app-network

  backend1:
    build: ./Server
    container_name: backend1
    ports:
      - "8081:8081"
    depends_on:
      - redis
      - mysql
    networks:
      - app-network

  backend2:
    build: ./Server
    container_name: backend2
    ports:
      - "8082:8081"
    depends_on:
      - redis
      - mysql
    networks:
      - app-network

  mysql:
    image: "mysql:8.0"
    container_name: mysql
    environment:
      MYSQL_ROOT_PASSWORD: root
      MYSQL_DATABASE: poolClash
    ports:
      - "3306:3306"
    volumes:
      - ./Server/schema.sql:/docker-entrypoint-initdb.d/schema.sql
    networks:
      - app-network

  client:
    build:
      args: 
        - VITE_APP_BASE_URL=http://localhost/api
        - VITE_WEBSOCKET_URL=ws://localhost/ws
      context: ./Client
    container_name: client
    ports:
      - "3000:3000"
    networks:
      - app-network
    env_file: ./Client/.env
    environment:
      - VITE_APP_BASE_URL=http://localhost/api
      - VITE_WEBSOCKET_URL=ws://localhost/ws

  nginx:
    image: nginx:latest
    container_name: nginx
    ports:
      - "80:80"
    depends_on:
      - backend1
      - backend2
      - client
    networks:
      - app-network
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf:ro

networks:
  app-network:
    driver: bridge
```

### NGINX Configuration

The `nginx.conf` file is designed to balance HTTP and WebSocket connections effectively:

```nginx
worker_processes auto;

events {
    worker_connections 1024;
}

http {
    map $http_upgrade $connection_upgrade {
        default upgrade;
        '' close;
    }

    
    upstream websocket_backend {
        least_conn;
        server backend1:8081;
        server backend2:8081;
    }

    upstream http_backend {
        least_conn;
        server backend1:8080;
        server backend2:8080;
    }

    upstream http_client {
        least_conn;
        server client:3000;
    }

    server {
        listen 80;

        # Route HTTP traffic
        location /api/ {
            proxy_pass http://http_backend;
            proxy_http_version 1.1;
            proxy_set_header Host $host;
            proxy_set_header X-Real-IP $remote_addr;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
        }

        # Route WebSocket traffic with load balancing
        location /ws {
            proxy_pass http://websocket_backend;
            proxy_http_version 1.1;
            proxy_set_header Upgrade $http_upgrade;
            proxy_set_header Connection "Upgrade";
            proxy_set_header Host $host;
        }

        location / {
            proxy_pass http://http_client;
            proxy_http_version 1.1;
        }
    }
}
```

Now you’re all set! Visit `http://localhost` to experience real-time polling with Poll Clash!
