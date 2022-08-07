 --- Roles
SET IDENTITY_INSERT [dbo].[AspNetRoles] ON 

INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (1, N'Admin', N'ADMIN', N'f6cf8083-ea42-41f1-8dd1-f9bc841dee7e')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (2, N'Manager', N'MANAGER', N'1a99bfd1-ea93-49b6-888a-d78bf74e2d1c')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (3, N'User', N'USER', N'c182f0e0-b96b-40be-98b4-eff1e8822fed')
SET IDENTITY_INSERT [dbo].[AspNetRoles] OFF
GO

 --- Users
SET IDENTITY_INSERT [dbo].[AspNetUsers] ON 

INSERT [dbo].[AspNetUsers] ([Id], [TimeZoneId], [Language], [FirstName], [Surname], [Money], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsBlocked]) VALUES (1, N'UTC', N'ru', N'Admin', N'Admin', CAST(0.00 AS Decimal(20, 2)), N'Admin', N'ADMIN', N'admin@admin.com', N'ADMIN@ADMIN.COM', 0, N'AQAAAAEAACcQAAAAEO1sGT8uGuB711JWDsKC82SAd53ioqz+A5HOjdw4P038ml5HdDaOFhI9vrpcHbqmHw==', N'74JOVLDHBEBBEOJKVAN2GIKVDIJTKYRZ', N'0a6c19be-6728-45a6-94e5-20d24c2d9909', NULL, 0, 0, NULL, 1, 0, 0)
INSERT [dbo].[AspNetUsers] ([Id], [TimeZoneId], [Language], [FirstName], [Surname], [Money], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsBlocked]) VALUES (2, N'UTC', N'ru', N'Manager', N'Manager', CAST(0.00 AS Decimal(20, 2)), N'Manager', N'MANAGER', N'manager@gmail.com', N'MANAGER@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEFkn5rqKqVv3Rbdiq00yNMKqPSbSIT86baNK9Yo9bkVs+BPyqDKF1Ib0/GwRmTg5Rw==', N'YHGFG74QZPLPTLBSKPVXY3JZ2D6CJ4A4', N'546fb803-665c-43ab-b2a0-b86c40bf3097', NULL, 0, 0, NULL, 1, 0, 0)
INSERT [dbo].[AspNetUsers] ([Id], [TimeZoneId], [Language], [FirstName], [Surname], [Money], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsBlocked]) VALUES (3, N'Belarus Standard Time', N'en', N'User', N'Userov', CAST(10000.00 AS Decimal(20, 2)), N'user10@gmail.com', N'USER10@GMAIL.COM', N'user10@gmail.com', N'USER10@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEHiL+/sIUOhHr+JFnpF/ahQjKPsaWlYEFTzE7BWb74ZxFFfzi5QsW4wOqnqyIjywBA==', N'3F6OIMFLRNRPICWF45LE5QPQKGCJB54O', N'e4eafa16-ba07-4147-9b9f-8dc3c291c182', NULL, 0, 0, NULL, 1, 0, 0)
SET IDENTITY_INSERT [dbo].[AspNetUsers] OFF
GO

 --- User roles
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (1, 1)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (2, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (3, 3)
GO

--- Venue
SET IDENTITY_INSERT [dbo].[Venue] ON 

INSERT [dbo].[Venue] ([Id], [Description], [Address], [Phone]) VALUES (1, N'First venue', N'First venue Address', N'123 45 678 90 12')
INSERT [dbo].[Venue] ([Id], [Description], [Address], [Phone]) VALUES (5, N'Second venue', N'Second venue Address', N'234 56 789 01 23')
INSERT [dbo].[Venue] ([Id], [Description], [Address], [Phone]) VALUES (6, N'Third venue', N'Third venue Address', N'345 67 890 12 34')
SET IDENTITY_INSERT [dbo].[Venue] OFF
GO

--- Layout
SET IDENTITY_INSERT [dbo].[Layout] ON 

INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (1, 1, N'First layout')
INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (5, 1, N'Second layout')
INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (6, 5, N'First layout to second venue')
INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (9, 5, N'Second layout to second venue')
SET IDENTITY_INSERT [dbo].[Layout] OFF
GO

--- Area
SET IDENTITY_INSERT [dbo].[Area] ON 

INSERT [dbo].[Area] ([Id], [LayoutId], [Description], [CoordX], [CoordY]) VALUES (5, 1, N'First area of first layout', 1, 1)
INSERT [dbo].[Area] ([Id], [LayoutId], [Description], [CoordX], [CoordY]) VALUES (6, 1, N'Second area of first layout', 2, 2)
INSERT [dbo].[Area] ([Id], [LayoutId], [Description], [CoordX], [CoordY]) VALUES (7, 5, N'First area of second layout', 3, 3)
SET IDENTITY_INSERT [dbo].[Area] OFF
GO

--- Seat
SET IDENTITY_INSERT [dbo].[Seat] ON 

INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (5, 5, 1, 1)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (6, 5, 1, 2)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (7, 5, 1, 3)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (8, 6, 2, 2)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (9, 6, 1, 1)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (10, 6, 2, 1)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (11, 7, 1, 1)
SET IDENTITY_INSERT [dbo].[Seat] OFF
GO

 --- Event
SET IDENTITY_INSERT [dbo].[Event] ON 

INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId], [DateTimeStart], [DateTimeEnd], [ImageUrl]) VALUES (6, N'Birthday', N'Birthday of Ramz Ahm (I apologize in advance...)', 1, CAST(N'2023-04-19T17:10:00.0000000' AS DateTime2), CAST(N'2023-04-19T19:10:00.0000000' AS DateTime2), N'1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg')
INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId], [DateTimeStart], [DateTimeEnd], [ImageUrl]) VALUES (10, N'Send-off to jail', N'Send-off to jail for bad apologies', 5, CAST(N'2022-05-18T15:00:00.0000000' AS DateTime2), CAST(N'2022-05-18T19:00:00.0000000' AS DateTime2), N'1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg')
INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId], [DateTimeStart], [DateTimeEnd], [ImageUrl]) VALUES (11, N'Release from prison', N'Release from prison after bad apology', 6, CAST(N'2022-09-09T10:00:00.0000000' AS DateTime2), CAST(N'2022-09-09T20:00:00.0000000' AS DateTime2), N'1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg')
INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId], [DateTimeStart], [DateTimeEnd], [ImageUrl]) VALUES (14, N'Name', N'Description', 9, CAST(N'2022-08-07T19:00:00.0000000' AS DateTime2), CAST(N'2022-08-08T05:00:00.0000000' AS DateTime2), N'1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg')
SET IDENTITY_INSERT [dbo].[Event] OFF
GO

 --- EventArea
SET IDENTITY_INSERT [dbo].[EventArea] ON 

INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) VALUES (6, 6, N'In honor of such a person, you need to arrange a big holiday', 1, 1, CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) VALUES (7, 10, N'Apparently the holiday was not big enough (or too expensive)', 3, 12, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) VALUES (8, 11, N'Our boss is out of prison, and we need to have a big celebration in honor of this', 1, 1, CAST(10000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[EventArea] OFF
GO

 --- Tickets
SET IDENTITY_INSERT [dbo].[Ticket] ON

INSERT [dbo].[Ticket] ([Id], [UserId], [Price], [Name], [Description], [StartEventDate], [EndEventDate])
VALUES (3, 3, CAST(0.00 AS Decimal(18, 2)), N'Birthday', N'Birthday of Ramz Ahm (I apologize in advance...)', CAST(N'2023-04-19T17:10:00.0000000' AS DateTime2), CAST(N'2023-04-19T19:10:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Ticket] OFF
GO

 --- EventSeat
SET IDENTITY_INSERT [dbo].[EventSeat] ON 

INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [TicketId], [Row], [Number], [State]) VALUES (8, 6, 3, 1, 1, 2)
INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [TicketId], [Row], [Number], [State]) VALUES (9, 7, 3, 1, 2, 2)
INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [TicketId], [Row], [Number], [State]) VALUES (10, 8, NULL, 1, 3, 1)
SET IDENTITY_INSERT [dbo].[EventSeat] OFF
GO