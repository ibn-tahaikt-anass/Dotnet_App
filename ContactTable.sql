-- Create the Contacts table
CREATE TABLE Contacts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Subject NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL
);

-- Optional: Insert some sample data
INSERT INTO Contacts (Subject, Email, Message)
VALUES 
    ('General Inquiry', 'john@example.com', 'I would like more information about your products.'),
    ('Support Request', 'sarah@example.com', 'I am having trouble with my order #12345.'),
    ('Feedback', 'mike@example.com', 'Great service! Very satisfied with the quality.');

-- View all contacts
SELECT * FROM Contacts;

