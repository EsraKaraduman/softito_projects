USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'HotelDb')
BEGIN
    ALTER DATABASE HotelDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE HotelDb;
END
GO

CREATE DATABASE HotelDb;
GO

USE HotelDb;
GO

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL
);
GO

CREATE TABLE Rooms (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoomNumber NVARCHAR(10) NOT NULL UNIQUE,
    RoomType NVARCHAR(50) NOT NULL,
    PricePerNight DECIMAL(18,2) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,
    Description NVARCHAR(500) NULL,
    ImageUrl NVARCHAR(500) NULL
);
GO

CREATE TABLE Bookings (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoomId INT FOREIGN KEY REFERENCES Rooms(Id) ON DELETE CASCADE,
    GuestName NVARCHAR(100) NOT NULL,
    GuestEmail NVARCHAR(100) NOT NULL,
    GuestPhone NVARCHAR(20) NOT NULL,
    CheckInDate DATE NOT NULL,
    CheckOutDate DATE NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,
    BookingStatus NVARCHAR(20) NOT NULL DEFAULT 'Confirmed'
);
GO

CREATE PROCEDURE sp_RegisterUser
    @Username NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255),
    @Role NVARCHAR(20)
AS
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, Role)
    VALUES (@Username, @Email, @PasswordHash, @Role);
END;
GO

CREATE PROCEDURE sp_GetUserByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT Id, Username, Email, PasswordHash, Role
    FROM Users
    WHERE Email = @Email;
END;
GO

CREATE PROCEDURE sp_GetAllRooms
AS
BEGIN
    SELECT Id, RoomNumber, RoomType, PricePerNight, IsAvailable, Description, ImageUrl
    FROM Rooms;
END;
GO

CREATE PROCEDURE sp_GetRoomById
    @Id INT
AS
BEGIN
    SELECT Id, RoomNumber, RoomType, PricePerNight, IsAvailable, Description, ImageUrl
    FROM Rooms
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE sp_InsertRoom
    @RoomNumber NVARCHAR(10),
    @RoomType NVARCHAR(50),
    @PricePerNight DECIMAL(18,2),
    @IsAvailable BIT,
    @Description NVARCHAR(500),
    @ImageUrl NVARCHAR(500)
AS
BEGIN
    INSERT INTO Rooms (RoomNumber, RoomType, PricePerNight, IsAvailable, Description, ImageUrl)
    VALUES (@RoomNumber, @RoomType, @PricePerNight, @IsAvailable, @Description, @ImageUrl);
END;
GO

CREATE PROCEDURE sp_UpdateRoom
    @Id INT,
    @RoomNumber NVARCHAR(10),
    @RoomType NVARCHAR(50),
    @PricePerNight DECIMAL(18,2),
    @IsAvailable BIT,
    @Description NVARCHAR(500),
    @ImageUrl NVARCHAR(500)
AS
BEGIN
    UPDATE Rooms
    SET RoomNumber = @RoomNumber,
        RoomType = @RoomType,
        PricePerNight = @PricePerNight,
        IsAvailable = @IsAvailable,
        Description = @Description,
        ImageUrl = @ImageUrl
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE sp_DeleteRoom
    @Id INT
AS
BEGIN
    DELETE FROM Rooms WHERE Id = @Id;
END;
GO

CREATE PROCEDURE sp_SearchRooms
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT Id, RoomNumber, RoomType, PricePerNight, IsAvailable, Description, ImageUrl
    FROM Rooms
    WHERE RoomNumber LIKE '%' + @SearchTerm + '%'
       OR RoomType LIKE '%' + @SearchTerm + '%'
       OR Description LIKE '%' + @SearchTerm + '%';
END;
GO

CREATE PROCEDURE sp_GetAllBookings
AS
BEGIN
    SELECT b.Id, b.RoomId, b.GuestName, b.GuestEmail, b.GuestPhone, b.CheckInDate, b.CheckOutDate, b.TotalPrice, b.BookingStatus,
           r.RoomNumber, r.RoomType
    FROM Bookings b
    INNER JOIN Rooms r ON b.RoomId = r.Id;
END;
GO

CREATE PROCEDURE sp_GetBookingById
    @Id INT
AS
BEGIN
    SELECT b.Id, b.RoomId, b.GuestName, b.GuestEmail, b.GuestPhone, b.CheckInDate, b.CheckOutDate, b.TotalPrice, b.BookingStatus,
           r.RoomNumber, r.RoomType
    FROM Bookings b
    INNER JOIN Rooms r ON b.RoomId = r.Id
    WHERE b.Id = @Id;
END;
GO

CREATE PROCEDURE sp_InsertBooking
    @RoomId INT,
    @GuestName NVARCHAR(100),
    @GuestEmail NVARCHAR(100),
    @GuestPhone NVARCHAR(20),
    @CheckInDate DATE,
    @CheckOutDate DATE,
    @TotalPrice DECIMAL(18,2),
    @BookingStatus NVARCHAR(20)
AS
BEGIN
    INSERT INTO Bookings (RoomId, GuestName, GuestEmail, GuestPhone, CheckInDate, CheckOutDate, TotalPrice, BookingStatus)
    VALUES (@RoomId, @GuestName, @GuestEmail, @GuestPhone, @CheckInDate, @CheckOutDate, @TotalPrice, @BookingStatus);
    
    UPDATE Rooms
    SET IsAvailable = 0
    WHERE Id = @RoomId;
END;
GO

CREATE PROCEDURE sp_UpdateBooking
    @Id INT,
    @RoomId INT,
    @GuestName NVARCHAR(100),
    @GuestEmail NVARCHAR(100),
    @GuestPhone NVARCHAR(20),
    @CheckInDate DATE,
    @CheckOutDate DATE,
    @TotalPrice DECIMAL(18,2),
    @BookingStatus NVARCHAR(20)
AS
BEGIN
    DECLARE @OldRoomId INT;
    SELECT @OldRoomId = RoomId FROM Bookings WHERE Id = @Id;

    UPDATE Bookings
    SET RoomId = @RoomId,
        GuestName = @GuestName,
        GuestEmail = @GuestEmail,
        GuestPhone = @GuestPhone,
        CheckInDate = @CheckInDate,
        CheckOutDate = @CheckOutDate,
        TotalPrice = @TotalPrice,
        BookingStatus = @BookingStatus
    WHERE Id = @Id;

    IF @OldRoomId <> @RoomId
    BEGIN
        UPDATE Rooms SET IsAvailable = 1 WHERE Id = @OldRoomId;
        UPDATE Rooms SET IsAvailable = 0 WHERE Id = @RoomId;
    END;
END;
GO

CREATE PROCEDURE sp_DeleteBooking
    @Id INT
AS
BEGIN
    DECLARE @RoomId INT;
    SELECT @RoomId = RoomId FROM Bookings WHERE Id = @Id;

    DELETE FROM Bookings WHERE Id = @Id;

    UPDATE Rooms SET IsAvailable = 1 WHERE Id = @RoomId;
END;
GO

CREATE PROCEDURE sp_SearchBookings
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT b.Id, b.RoomId, b.GuestName, b.GuestEmail, b.GuestPhone, b.CheckInDate, b.CheckOutDate, b.TotalPrice, b.BookingStatus,
           r.RoomNumber, r.RoomType
    FROM Bookings b
    INNER JOIN Rooms r ON b.RoomId = r.Id
    WHERE b.GuestName LIKE '%' + @SearchTerm + '%'
       OR b.GuestEmail LIKE '%' + @SearchTerm + '%'
       OR r.RoomNumber LIKE '%' + @SearchTerm + '%'
       OR r.RoomType LIKE '%' + @SearchTerm + '%';
END;
GO
