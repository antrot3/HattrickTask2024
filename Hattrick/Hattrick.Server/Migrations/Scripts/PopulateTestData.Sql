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
(1, 50, 'Add to wallet', '2024-07-01 10:00:00'),
(1, 50, 'Add to wallet', '2024-07-01 11:00:00'),
(1, 20, 'Deposit to bank', '2024-07-02 09:00:00'),
(1, 20, 'Bet', '2024-07-02 09:00:00');

-- Insert into bet_types
INSERT INTO bettypes (name, description) VALUES
('1', 'Home Win'),
('2', 'Away Win'),
('X', 'Draw'),
('1X', 'Home Win or Draw'),
('X2', 'Draw or Away Win'),
('12', 'Home or Away Win');

-- Insert into coefficient
INSERT INTO coefficient (matchid, bettypeid, oddvalue) VALUES
(1, 1, 1.5),
(1, 2, 2.5),
(1, 3, 3.0),
(1, 4, 1.43),
(1, 5, 1.25),
(1, 6, 1.37),
(2, 1, 1.8),
(2, 2, 2.2),
(2, 3, 2.8),
(2, 4, 1.33),
(2, 5, 1.25),
(2, 6, 1.47),
(3, 1, 1.6),
(3, 2, 2.4),
(3, 3, 3.1),
(3, 3, 2.8),
(3, 4, 1.33),
(3, 5, 1.25),
(3, 6, 1.47),

(4, 1, 1.5),
(4, 2, 2.5),
(5, 1, 3.0),
(5, 2, 1.43),
(6, 1, 1.25),
(6, 2, 1.37),
(7, 1, 1.8),
(7, 2, 2.2),
(8, 1, 2.8),
(8, 2, 1.33),
(9, 1, 1.05),
(9, 2, 1.47);

-- Insert into top_offers
INSERT INTO topoffers (matchid, oddmultiplier) VALUES
(1, 1.4),
(2, 1.5);

-- Insert into tickets
INSERT INTO tickets (userid, totalodd, stake, potentialwinning, date, isbetplayed, didbetwin) VALUES
(1, 2.5, 10, 25, '2024-07-03 12:00:00',true,false),
(1, 3.0, 20, 60, '2024-07-03 13:00:00',true,true);

-- Insert into ticket_bets
INSERT INTO ticketbets (ticketid, matchid, bettypeid, odd) VALUES
(1, 1, 1, 1.5),
(1, 2, 1, 1.8),
(2, 1, 2, 2.5),
(2, 3, 2, 2.4);