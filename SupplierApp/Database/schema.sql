-- Microsoft Access / Jet 4.0. Выполнять отдельные операторы через OleDb.
-- Готовая база уже включена в проект; эти команды нужны только для воссоздания.
CREATE TABLE [Users] (
    [UserId] COUNTER CONSTRAINT [PK_Users] PRIMARY KEY,
    [UserName] TEXT(50) NOT NULL,
    [FullName] TEXT(100) NOT NULL,
    [PasswordSalt] TEXT(24) NOT NULL,
    [PasswordHash] TEXT(44) NOT NULL,
    [HashIterations] LONG NOT NULL,
    [CreatedAt] DATETIME NOT NULL
);
CREATE UNIQUE INDEX [UX_Users_UserName] ON [Users] ([UserName]);
CREATE TABLE [Products] (
    [ProductId] COUNTER CONSTRAINT [PK_Products] PRIMARY KEY,
    [ProductName] TEXT(100) NOT NULL,
    [Unit] TEXT(10) NOT NULL,
    [UnitPrice] CURRENCY NOT NULL,
    [IsActive] YESNO NOT NULL
);
CREATE TABLE [Orders] (
    [OrderId] COUNTER CONSTRAINT [PK_Orders] PRIMARY KEY,
    [UserId] LONG NOT NULL,
    [ProductId] LONG NOT NULL,
    [Quantity] DECIMAL(18,3) NOT NULL,
    [UnitPrice] CURRENCY NOT NULL,
    [DeliveryAddress] TEXT(255) NOT NULL,
    [CreatedAt] DATETIME NOT NULL,
    [OrderStatus] TEXT(20) NOT NULL,
    CONSTRAINT [FK_Orders_Users] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]),
    CONSTRAINT [FK_Orders_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId])
);
