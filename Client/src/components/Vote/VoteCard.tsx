// interface VoteCardProps {
//     options?:

import { useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "../ui/card";
import { Button } from "../ui/button";
import { Share2 } from "lucide-react";
import { Progress } from "../ui/progress";

interface VoteCardProps {
  vote: APP.Vote;
  onVote?: (id: number) => void;
}

const VoteCard: React.FC<VoteCardProps> = ({ vote, onVote }) => {
  const { Options, Text } = vote;
  const [userVote] = useState(null);

  const totalVotes = Options
    ? Options.reduce((sum, option) => sum + option.Votes, 0)
    : 0;

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-2xl font-bold text-center">
          {Text ?? "Daily Vote"}
        </CardTitle>
      </CardHeader>
      <CardContent>
        {Options?.map((option) => (
          <div key={option.Id} className="mb-4">
            <div className="flex justify-between items-center mb-2">
              <span className="font-semibold">{option.Text}</span>
              <span className="text-sm text-gray-500">
                {option.Votes} votes
              </span>
            </div>
            <div className="flex items-center space-x-2">
              <Progress
                value={(option.Votes / totalVotes) * 100}
                className="flex-grow"
              />
              <Button
                onClick={() => {
                  if (onVote) onVote(option.Id);
                }}
                variant={userVote === option.Id ? "secondary" : "default"}
              >
                {userVote === option.Id ? "Voted" : "Vote"}
              </Button>
              {userVote === option.Id && (
                <Button variant="outline" size="icon">
                  <Share2 className="h-4 w-4" />
                </Button>
              )}
            </div>
          </div>
        ))}
      </CardContent>
    </Card>
  );
};

export default VoteCard;
