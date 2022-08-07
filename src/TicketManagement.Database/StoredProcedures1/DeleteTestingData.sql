CREATE PROCEDURE [dbo].[sp_DeleteTestingData]
AS
BEGIN
--- Delete Event Test Data
	DELETE FROM [dbo].[Event] WHERE [Id] >= 100;
    DBCC CHECKIDENT ('Event',RESEED,20);

--- Delete EventArea Test Data
    DELETE FROM [dbo].[EventArea] WHERE[Id] >= 100;
    DBCC CHECKIDENT('EventArea',RESEED,20);

--- Delete Ticket Test Data
	DELETE FROM [dbo].[Ticket] WHERE [Id] >= 100;
    DBCC CHECKIDENT ('Ticket',RESEED, 10);

--- Delete EventSeat Test Data
    DELETE FROM [dbo].[EventSeat] WHERE [Id] >= 100;
    DBCC CHECKIDENT ('EventSeat',RESEED,20);

--- Delete Area Test Data
	DELETE FROM [dbo].[Area] WHERE [Id] >= 100;
	DBCC CHECKIDENT ('Area',RESEED,20);

--- Delete Layout Test Data
	DELETE FROM [dbo].[Layout] WHERE [Id] >= 100;
	DBCC CHECKIDENT ('Layout',RESEED,4);

--- Delete Seat Test Data
	DELETE FROM [dbo].[Seat] WHERE [Id] >= 100;
	DBCC CHECKIDENT ('Seat',RESEED,10);

--- Delete Venue Test Data
	DELETE FROM [dbo].[Venue] WHERE [Id] >= 100;
	DBCC CHECKIDENT ('Venue',RESEED,4);
END