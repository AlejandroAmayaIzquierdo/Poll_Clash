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