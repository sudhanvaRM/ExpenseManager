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