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
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE
);

CREATE TABLE expense (
    expense_id UUID DEFAULT gen_random_uuid() PRIMARY KEY,
    trip_id UUID NULL,
    paid_user UUID NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    comment TEXT NULL,
    category VARCHAR(255) NOT NULL,
    FOREIGN KEY (trip_id) REFERENCES trip(trip_id) ON DELETE CASCADE,
    FOREIGN KEY (paid_user) REFERENCES users(user_id) ON DELETE CASCADE
);

CREATE TABLE debt (
    trip_id UUID,
    debtor_id UUID NOT NULL,
    creditor_id UUID NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    status BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY (trip_id, debtor_id, creditor_id),
    FOREIGN KEY (trip_id) REFERENCES trip(trip_id) ON DELETE CASCADE,
    FOREIGN KEY (debtor_id) REFERENCES users(user_id) ON DELETE CASCADE,
    FOREIGN KEY (creditor_id) REFERENCES users(user_id) ON DELETE CASCADE
);
