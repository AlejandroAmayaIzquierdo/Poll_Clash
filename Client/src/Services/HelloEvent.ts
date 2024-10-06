const HelloEventService = (ws: WebSocket, name: string) => {
  const data: API.WebSocketEvent = {
    eventType: "HelloEvent",
    name,
  };

  ws.send(JSON.stringify(data));
};

export default HelloEventService;
