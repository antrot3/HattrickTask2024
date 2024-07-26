INSERT INTO sports (name) VALUES ('Basketball'), ('Tennis'), ('Football');

-- Insert into users
INSERT INTO users (name, walletbalance) VALUES ('John Doe', 400);

-- Insert into matches
INSERT INTO matches (sportid, teamhome, teamaway, date) VALUES
(1, 'Bulls', 'Lakers', '2024-08-01 20:00:00'),
(1, 'Split', 'Cibona', '2024-08-02 18:00:00'),
(1, 'New york', 'Florida', '2024-08-03 15:00:00'),
(2, 'Djokovic', 'Federer', '2024-08-01 20:00:00'),
(2, 'Nadal', 'Federer', '2024-08-02 18:00:00'),
(2, 'Sampras', 'Federer', '2024-08-03 15:00:00'),
(3, 'Hajduk', 'Barcelona', '2024-08-01 20:00:00'),
(3, 'Arsenal', 'PSG', '2024-08-02 18:00:00'),
(3, 'Dinamo', 'Solin', '2024-08-03 15:00:00');

-- Insert into wallet_transactions
INSERT INTO wallettransactions (userid, amount, transactiontype, date) VALUES
(1, 50, 'deposit', '2024-07-01 10:00:00'),
(1, 50, 'deposit', '2024-07-01 11:00:00'),
(1, 20, 'withdrawal', '2024-07-02 09:00:00'),
(1, 20, 'bet', '2024-07-02 09:00:00');

-- Insert into bet_types
INSERT INTO bettypes (name, description) VALUES
('1', 'Home Win'),
('2', 'Away Win'),
('X', 'Draw'),
('1X', 'Home Win or Draw'),
('X2', 'Draw or Away Win'),
('12', 'Home or Away Win');

-- Insert into coeficient
INSERT INTO coeficient (matchid, bettypeid, oddvalue) VALUES
(1, 1, 1.5),
(1, 2, 2.5),
(1, 3, 3.0),
(1, 4, 1.43),
(1, 5, 1.25),
(1, 6, 1.37),
(2, 1, 1.8),
(2, 2, 2.2),
(2, 3, 2.8),
(3, 1, 1.6),
(3, 2, 2.4),
(3, 3, 3.1);

-- Insert into top_offers
INSERT INTO topoffers (matchid, conditiondescription, oddvalue) VALUES
(1, 'Special condition for match 1', 3.57),
(2, 'Special condition for match 2', 2.39);


-- Insert into tickets
INSERT INTO tickets (userid, totalodd, stake, potentialwinning, date) VALUES
(1, 2.5, 10, 25, '2024-07-03 12:00:00'),
(1, 3.0, 20, 60, '2024-07-03 13:00:00');

-- Insert into ticket_bets
INSERT INTO ticketbets (ticketid, matchid, bettypeid, odd) VALUES
(1, 1, 1, 1.5),
(1, 2, 1, 1.8),
(2, 1, 2, 2.5),
(2, 3, 2, 2.4);