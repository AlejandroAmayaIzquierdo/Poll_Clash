const VoteEvent = (ws: WebSocket, option: number) => {
  const data: API.WebSocketEvent = {
    eventType: "VoteEvent",
    option,
  };

  ws.send(JSON.stringify(data));
};

export default VoteEvent;
