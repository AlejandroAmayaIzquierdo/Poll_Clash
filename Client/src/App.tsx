import { useEffect, useState } from "react";
import { Button } from "./components/ui/button";
import HelloEventService from "./Services/HelloEvent";

function App() {
  const [socket, setSocket] = useState<WebSocket>();

  useEffect(() => {
    const ws = new WebSocket("ws://localhost:81");

    ws.onopen = () => {
      console.log("Connected");
    };
    ws.onmessage = (event) => {
      console.log(event.data);
    };

    setSocket(ws);
  }, []);

  return (
    <Button
      onClick={() => {
        if (socket) HelloEventService(socket, "Nova");
      }}
    >
      Click me
    </Button>
  );
}

export default App;
