import { useEffect, useState } from "react";
import VoteCard from "./components/Vote/VoteCard";
import VoteEvent from "./Services/VoteEvent";
import { Loader2 } from "lucide-react";

function App() {
  const [socket, setSocket] = useState<WebSocket>();

  const [currentVote, setCurrentVote] = useState<APP.Vote>();
  // const [port, setport] = useState("8081");

  useEffect(() => {
    // const a = (Math.random() + 1).toFixed(0);
    // setport(`ws://192.168.1.40:808${a}`);
    const ws = new WebSocket(`ws://192.168.1.40:8081`);

    ws.onmessage = (event) => {
      const message: APP.Vote = JSON.parse(event.data);
      setCurrentVote(message);
    };

    setSocket(ws);
  }, []);

  const handleVote = (id: number) => {
    if (!socket) return;

    VoteEvent(socket, id);
  };

  return (
    <>
      {currentVote ? (
        <VoteCard vote={currentVote} onVote={handleVote} />
      ) : (
        <>
          <div className="w-full h-full absolute items-center justify-center flex flex-col">
            <Loader2 className="mr-2 h-16 w-16 animate-spin" />
          </div>
        </>
      )}
    </>
  );
}

export default App;
