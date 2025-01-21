-- Insert sample users
INSERT INTO users (user_id, username, password) VALUES
('c56a4180-65aa-42ec-a945-5fd21dec0538', 'Alice', 'password123'),
('c56a4180-65aa-42ec-a945-5fd21dec0539', 'Bob', 'password123'),
('c56a4180-65aa-42ec-a945-5fd21dec0540', 'Charlie', 'password123'),
('c56a4180-65aa-42ec-a945-5fd21dec0541', 'David', 'password123'),
('c56a4180-65aa-42ec-a945-5fd21dec0542', 'Emma', 'password123');

-- Insert a sample trip
INSERT INTO trip (trip_id, trip_name, trip_date) VALUES
('148b272f-7a26-45c2-9e33-c9ac99ba8b8d', 'Beach Vacation', '2025-02-01');

-- Insert trip participants
INSERT INTO trip_participants (trip_id, user_id) VALUES
('148b272f-7a26-45c2-9e33-c9ac99ba8b8d', 'c56a4180-65aa-42ec-a945-5fd21dec0538'),
('148b272f-7a26-45c2-9e33-c9ac99ba8b8d', 'c56a4180-65aa-42ec-a945-5fd21dec0539'),
('148b272f-7a26-45c2-9e33-c9ac99ba8b8d', 'c56a4180-65aa-42ec-a945-5fd21dec0540'),
('148b272f-7a26-45c2-9e33-c9ac99ba8b8d', 'c56a4180-65aa-42ec-a945-5fd21dec0541'),
('148b272f-7a26-45c2-9e33-c9ac99ba8b8d', 'c56a4180-65aa-42ec-a945-5fd21dec0542');

-- Insert expenses
INSERT INTO expense (expense_id, trip_id, paid_user, amount, comment, category) VALUES
('a1b2c3d4-65aa-42ec-a945-5fd21dec0001', '148b272f-7a26-45c2-9e33-c9ac99ba8b8d', 'c56a4180-65aa-42ec-a945-5fd21dec0538', 100.00, 'Lunch for everyone', 'Food'),
('a1b2c3d4-65aa-42ec-a945-5fd21dec0002', '148b272f-7a26-45c2-9e33-c9ac99ba8b8d', 'c56a4180-65aa-42ec-a945-5fd21dec0539', 50.00, 'Snacks and drinks', 'Food'),
('a1b2c3d4-65aa-42ec-a945-5fd21dec0003', '148b272f-7a26-45c2-9e33-c9ac99ba8b8d', 'c56a4180-65aa-42ec-a945-5fd21dec0540', 150.00, 'Beach cabana rental', 'Accommodation'),
('a1b2c3d4-65aa-42ec-a945-5fd21dec0004', '148b272f-7a26-45c2-9e33-c9ac99ba8b8d', 'c56a4180-65aa-42ec-a945-5fd21dec0541', 200.00, 'Dinner', 'Food'),
('a1b2c3d4-65aa-42ec-a945-5fd21dec0005', '148b272f-7a26-45c2-9e33-c9ac99ba8b8d', 'c56a4180-65aa-42ec-a945-5fd21dec0542', 300.00, 'Hotel', 'Accommodation');



