INSERT INTO [Users] ([Name], [Email], [Role]) VALUES
('Alice Admin', 'alice@example.com', 'Admin'),
('Bob Support', 'bob@example.com', 'Support'),
('Charlie User', 'charlie@example.com', 'User'),
('Diana Support', 'diana@example.com', 'Support'),
('Ethan Analyst', 'ethan@example.com', 'User');

INSERT INTO [Tickets] ([Title], [Description], [Priority], [Status], [AssignedToUserId], [CreatedByUserId], [CreatedAt], [UpdatedAt]) VALUES
('Cannot log in', 'User reports being unable to log in with correct credentials.', 2, 0, 2, 3, SYSUTCDATETIME(), SYSUTCDATETIME()),
('Feature request: Dark mode', 'User requested a dark mode for the dashboard.', 1, 1, 2, 3, SYSUTCDATETIME(), SYSUTCDATETIME());

INSERT INTO [Comments] ([TicketId], [Message], [CreatedByUserId], [CreatedAt]) VALUES
(1, 'We are investigating this issue.', 2, SYSUTCDATETIME());

