import VoteCard from "@/components/Vote/VoteCard";
import { VITE_WEBSOCKET_URL } from "@/constants";
import VoteEvent from "@/Services/VoteEvent";
import { Loader2 } from "lucide-react";
import { useEffect, useState } from "react";
import { useParams } from "react-router";

// interface PollPageProps {}
const PollPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();

  const [socket, setSocket] = useState<WebSocket>();

  const [currentPoll, setCurrentPoll] = useState<APP.Poll>();

  useEffect(() => {
    const ws = new WebSocket(VITE_WEBSOCKET_URL);

    ws.onopen = () => {
      ws.send(JSON.stringify({ eventType: "ConnectToPollEvent", id }));
    };

    ws.onmessage = (event) => {
      const message: APP.Poll = JSON.parse(event.data);
      setCurrentPoll(message);
    };

    ws.onclose = () => {
      setCurrentPoll(undefined);
    };

    ws.onerror = () => {
      setCurrentPoll(undefined);
    };

    setSocket(ws);
  }, []);

  const handleVote = (voteID: number) => {
    if (!socket || !id) return;

    VoteEvent(socket, id, voteID);
  };

  return (
    <>
      {currentPoll ? (
        <VoteCard vote={currentPoll} onVote={(a) => handleVote(a)} />
      ) : (
        <>
          <div className="w-full h-full absolute items-center justify-center flex flex-col">
            <Loader2 className="mr-2 h-16 w-16 animate-spin" />
          </div>
        </>
      )}
    </>
  );
};

export default PollPage;
