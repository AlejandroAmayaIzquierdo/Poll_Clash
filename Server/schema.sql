-- Create table for Polls
CREATE TABLE Polls (
    PollId VARCHAR(255) PRIMARY KEY,
    Text VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create table for Options associated with Polls
CREATE TABLE Options (
    OptionId INT AUTO_INCREMENT PRIMARY KEY,
    PollId VARCHAR(255) NOT NULL,
    Text VARCHAR(255) NOT NULL,
    Votes INT DEFAULT 0,
    FOREIGN KEY (PollId) REFERENCES Polls(PollId) ON DELETE CASCADE
);

-- Index to improve performance for finding winnable options
CREATE INDEX idx_poll_options_votes ON Options (PollId, Votes DESC);

-- Table for tracking user votes (optional, for more detailed voting management)
CREATE TABLE UserVotes (
    UserVoteId INT AUTO_INCREMENT PRIMARY KEY,
    PollId VARCHAR(255) NOT NULL,
    OptionId INT NOT NULL,
    UserId INT NOT NULL, -- Assuming there's a user system
    VotedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (PollId) REFERENCES Polls(PollId) ON DELETE CASCADE,
    FOREIGN KEY (OptionId) REFERENCES Options(OptionId) ON DELETE CASCADE
);


-- Insert mock data into Polls table
INSERT INTO Polls (PollId, Text) VALUES
('poll1', 'Que prefires?'),
('poll2', 'Which language do you prefer?'),
('poll3', 'Favorite programming paradigm?');

-- Insert mock data into Options table associated with the first poll
INSERT INTO Options (PollId, Text) VALUES
('poll1', 'Esto'),       
('poll1', 'Nah esto'),   
('poll2', 'C#'),         
('poll2', 'Python'),     
('poll3', 'Object-Oriented'),  
('poll3', 'Functional'),       
('poll3', 'Procedural');

-- Insert mock data into UserVotes table
INSERT INTO UserVotes (PollId, OptionId, UserId) VALUES
('poll1', 1, 101),  -- User 101 votes for "Esto"
('poll1', 2, 102),  -- User 102 votes for "Nah esto"
('poll2', 1, 103),  -- User 103 votes for "C#"
('poll2', 2, 101),  -- User 101 votes for "Python"
('poll3', 1, 104),  -- User 104 votes for "Object-Oriented"
('poll3', 2, 105),  -- User 105 votes for "Functional"
('poll3', 3, 101);  -- User 101 votes for "Procedural"