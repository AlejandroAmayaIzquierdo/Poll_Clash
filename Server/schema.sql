-- Create table for Polls
CREATE TABLE Polls (
    PollId INT AUTO_INCREMENT PRIMARY KEY,
    Text VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create table for Options associated with Polls
CREATE TABLE Options (
    OptionId INT AUTO_INCREMENT PRIMARY KEY,
    PollId INT NOT NULL,
    Text VARCHAR(255) NOT NULL,
    Votes INT DEFAULT 0,
    FOREIGN KEY (PollId) REFERENCES Polls(PollId) ON DELETE CASCADE
);

-- Index to improve performance for finding winnable options
CREATE INDEX idx_poll_options_votes ON Options (PollId, Votes DESC);

-- Table for tracking user votes (optional, for more detailed voting management)
CREATE TABLE UserVotes (
    UserVoteId INT AUTO_INCREMENT PRIMARY KEY,
    PollId INT NOT NULL,
    OptionId INT NOT NULL,
    UserId INT NOT NULL, -- Assuming there's a user system
    VotedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (PollId) REFERENCES Polls(PollId) ON DELETE CASCADE,
    FOREIGN KEY (OptionId) REFERENCES Options(OptionId) ON DELETE CASCADE
);


-- Insert mock data into Polls table
INSERT INTO Polls (Text) VALUES
('Que prefires?'),
('Which language do you prefer?'),
('Favorite programming paradigm?');

-- Insert mock data into Options table associated with the first poll
INSERT INTO Options (PollId, Text) VALUES
(1, 'Esto'),        -- Associated with "Que prefires?"
(1, 'Nah esto'),    -- Associated with "Que prefires?"
(2, 'C#'),          -- Associated with "Which language do you prefer?"
(2, 'Python'),      -- Associated with "Which language do you prefer?"
(3, 'Object-Oriented'),  -- Associated with "Favorite programming paradigm?"
(3, 'Functional'),       -- Associated with "Favorite programming paradigm?"
(3, 'Procedural');       -- Associated with "Favorite programming paradigm?"

-- Insert mock data into UserVotes table
INSERT INTO UserVotes (PollId, OptionId, UserId) VALUES
(1, 1, 101),  -- User 101 votes for "Esto"
(1, 2, 102),  -- User 102 votes for "Nah esto"
(2, 1, 103),  -- User 103 votes for "C#"
(2, 2, 101),  -- User 101 votes for "Python"
(3, 1, 104),  -- User 104 votes for "Object-Oriented"
(3, 2, 105),  -- User 105 votes for "Functional"
(3, 3, 101);  -- User 101 votes for "Procedural"