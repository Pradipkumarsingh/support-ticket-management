CREATE TABLE [Users] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(200) NOT NULL,
    [Role] NVARCHAR(50) NOT NULL
);

CREATE TABLE [Tickets] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(2000) NOT NULL,
    [Priority] INT NOT NULL,
    [Status] INT NOT NULL,
    [AssignedToUserId] INT NULL,
    [CreatedByUserId] INT NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL,
    [UpdatedAt] DATETIME2 NOT NULL,
    CONSTRAINT [FK_Tickets_AssignedTo] FOREIGN KEY ([AssignedToUserId]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_Tickets_CreatedBy] FOREIGN KEY ([CreatedByUserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [Comments] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [TicketId] INT NOT NULL,
    [Message] NVARCHAR(2000) NOT NULL,
    [CreatedByUserId] INT NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL,
    CONSTRAINT [FK_Comments_Ticket] FOREIGN KEY ([TicketId]) REFERENCES [Tickets] ([Id]),
    CONSTRAINT [FK_Comments_CreatedBy] FOREIGN KEY ([CreatedByUserId]) REFERENCES [Users] ([Id])
);

