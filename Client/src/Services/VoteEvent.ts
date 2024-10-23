const VoteEvent = (ws: WebSocket, id: string, option: number) => {
  const data: API.WebSocketEvent = {
    eventType: "VoteEvent",
    id,
    option,
  };

  console.log(data);

  ws.send(JSON.stringify(data));
};

export default VoteEvent;
