CREATE PROCEDURE [dbo].[sp_AddTestingData]
AS
BEGIN
--- Create Event Test Data
	DELETE FROM [dbo].[Event] WHERE [Id] >= 100;
    DBCC CHECKIDENT ('Event',RESEED, 20);

    SET IDENTITY_INSERT [dbo].[Event] ON
    INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId], [DateTimeStart], [DateTimeEnd], [ImageUrl]) 
        VALUES (100, N'SetUp Test Name', N'SetUp Test Description', 1, CAST(N'2023-04-19T20:30:00' AS DateTime2), CAST(N'2023-04-19T22:30:00' AS DateTime2), N'1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg')
    INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId], [DateTimeStart], [DateTimeEnd], [ImageUrl]) 
        VALUES (101, N'SetUp Test Name 2', N'SetUp Test Description 2', 1, CAST(N'2023-04-19T10:30:00' AS DateTime2), CAST(N'2023-04-19T12:30:00' AS DateTime2), N'1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg')
    SET IDENTITY_INSERT [dbo].[Event] OFF
	
 --- Create EventArea Test Data
    DELETE FROM [dbo].[EventArea] WHERE [Id] >= 100;
    DBCC CHECKIDENT('EventArea',RESEED,20);

    SET IDENTITY_INSERT [dbo].[EventArea] ON
    INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) 
        VALUES (100, 100, N'First area of first layout', 1, 1, 200)
    INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) 
        VALUES (101, 100, N'Second area of first layout', 2, 2, 400)
    INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) 
        VALUES (102, 101, N'Alternate First area of first layout', 2, 2, Null)
    SET IDENTITY_INSERT [dbo].[EventArea] OFF
	
 --- Create Ticket Test Data
	DELETE FROM [dbo].[Ticket] WHERE [Id] >= 100;
    DBCC CHECKIDENT ('Ticket',RESEED, 10);

    SET IDENTITY_INSERT [dbo].[Ticket] ON
    INSERT [dbo].[Ticket] ([Id], [Name], [Description], [UserId], [StartEventDate], [EndEventDate], [Price]) 
        VALUES (100, N'SetUp Test Name', N'SetUp Test Description', 3, CAST(N'2023-04-19T20:30:00' AS DateTime2), CAST(N'2023-04-19T22:30:00' AS DateTime2), 0)
    SET IDENTITY_INSERT [dbo].[Ticket] OFF

 -- Create EventSeat Test Data
    DELETE FROM [dbo].[EventSeat] WHERE [Id] >= 100;
    DBCC CHECKIDENT ('EventSeat',RESEED,20);

    SET IDENTITY_INSERT [dbo].[EventSeat] ON
    INSERT [dbo].[EventSeat] ([Id], [TicketId], [EventAreaId], [Row], [Number], [State]) 
        VALUES (100, 100, 100, 1, 1, 0)
    INSERT [dbo].[EventSeat] ([Id], [TicketId], [EventAreaId], [Row], [Number], [State]) 
        VALUES (101, 100, 100, 1, 2, 0)
    INSERT [dbo].[EventSeat] ([Id], [TicketId], [EventAreaId], [Row], [Number], [State]) 
        VALUES (102, 100, 100, 1, 3, 0)

    INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [Row], [Number], [State])
        VALUES (103, 101, 2, 2, 0)
    INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [Row], [Number], [State]) 
        VALUES (104, 101, 1, 1, 0)
    INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [Row], [Number], [State]) 
        VALUES (105, 101, 2, 1, 0)
    SET IDENTITY_INSERT [dbo].[EventSeat] OFF

--- Create Area Test Data
	DELETE FROM [dbo].[Area] WHERE [Id] >= 100;
    DBCC CHECKIDENT ('Area',RESEED,4);
    SET IDENTITY_INSERT [dbo].[Area] ON
    INSERT [dbo].[Area] ([Id], [LayoutId], [Description], [CoordX], [CoordY]) 
        VALUES (100, 1, N'SetUp Test Description', 1, 1)
    SET IDENTITY_INSERT [dbo].[Area] OFF

--- Create Layout Test Data
	DELETE FROM [dbo].[Layout] WHERE [Id] >= 100;
    DBCC CHECKIDENT ('Layout',RESEED,4);
    SET IDENTITY_INSERT [dbo].[Layout] ON
    INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) 
        VALUES (100, 5, N'SetUp Test Description')
    SET IDENTITY_INSERT [dbo].[Layout] OFF

--- Create Seat Test Data
	DELETE FROM [dbo].[Seat] WHERE [Id] >= 100;
    DBCC CHECKIDENT ('Seat',RESEED,20);
    SET IDENTITY_INSERT [dbo].[Seat] ON
    INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) 
        VALUES (100, 6, 15, 20)
    SET IDENTITY_INSERT [dbo].[Seat] OFF

--- Create Venue Test Data
	DELETE FROM [dbo].[Venue] WHERE [Id] >= 100;
    DBCC CHECKIDENT ('Venue',RESEED,4);
    SET IDENTITY_INSERT [dbo].[Venue] ON
    INSERT [dbo].[Venue] ([Id], [Description], [Address], [Phone])
        VALUES (100, N'SetUp Test Description', N'SetUp Test Address', N'456 78 901 23 45')
    SET IDENTITY_INSERT [dbo].[Venue] OFF
END

GO