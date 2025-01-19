CREATE TABLE user (
    user_id UUID DEFAULT gen_random_uuid() PRIMARY KEY, 
    username VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL 
);

CREATE TABLE trip (
    trip_id UUID DEFAULT gen_random_uuid() PRIMARY KEY,
    trip_name VARCHAR(255) NOT NULL,
    trip_date DATE NOT NULL
);

CREATE TABLE trip_participants (
    trip_id UUID,
    user_id UUID,
    PRIMARY KEY (trip_id, user_id),
    FOREIGN KEY (trip_id) REFERENCES trip(trip_id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES user(user_id) ON DELETE CASCADE
);

CREATE TABLE expense (
    expense_id UUID DEFAULT gen_random_uuid() PRIMARY KEY,
    trip_id UUID NULL,
    paid_user UUID NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    comment TEXT NULL,
    category VARCHAR(255) NOT NULL,
    FOREIGN KEY (trip_id) REFERENCES trip(trip_id) ON DELETE CASCADE,
    FOREIGN KEY (paid_user) REFERENCES user(user_id) ON DELETE CASCADE
);

CREATE TABLE debt (
    trip_id UUID,
    debtor_id UUID NOT NULL,
    creditor_id UUID NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    status BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY (trip_id, debtor_id, creditor_id),
    FOREIGN KEY (trip_id) REFERENCES trip(trip_id) ON DELETE CASCADE,
    FOREIGN KEY (debtor_id) REFERENCES user(user_id) ON DELETE CASCADE,
    FOREIGN KEY (creditor_id) REFERENCES user(user_id) ON DELETE CASCADE
);

#This Migration failed because User is a keyword in Postgres. But Error is not reflected in the logs. Message returns as Migration successful but the table is not created in the database.

 