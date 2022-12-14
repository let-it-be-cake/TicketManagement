USE [master]
GO
/****** Object:  Database [TicketManagementTests]    Script Date: 06.03.2021 15:38:45 ******/
CREATE DATABASE [TicketManagementTests]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TicketManagementTests', FILENAME = N'D:\Programs\SQLServer2019\MSSQL15.AZX\MSSQL\DATA\TicketManagementTests_Primary.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TicketManagementTests_log', FILENAME = N'D:\Programs\SQLServer2019\MSSQL15.AZX\MSSQL\DATA\TicketManagementTests_Primary.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [TicketManagementTests] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TicketManagementTests].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TicketManagementTests] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [TicketManagementTests] SET ANSI_NULLS ON 
GO
ALTER DATABASE [TicketManagementTests] SET ANSI_PADDING ON 
GO
ALTER DATABASE [TicketManagementTests] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [TicketManagementTests] SET ARITHABORT ON 
GO
ALTER DATABASE [TicketManagementTests] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TicketManagementTests] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TicketManagementTests] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TicketManagementTests] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TicketManagementTests] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [TicketManagementTests] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [TicketManagementTests] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TicketManagementTests] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [TicketManagementTests] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TicketManagementTests] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TicketManagementTests] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TicketManagementTests] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TicketManagementTests] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TicketManagementTests] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TicketManagementTests] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TicketManagementTests] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TicketManagementTests] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TicketManagementTests] SET RECOVERY FULL 
GO
ALTER DATABASE [TicketManagementTests] SET  MULTI_USER 
GO
ALTER DATABASE [TicketManagementTests] SET PAGE_VERIFY NONE  
GO
ALTER DATABASE [TicketManagementTests] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TicketManagementTests] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TicketManagementTests] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [TicketManagementTests] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TicketManagementTests', N'ON'
GO
ALTER DATABASE [TicketManagementTests] SET QUERY_STORE = OFF
GO
USE [TicketManagementTests]
GO
/****** Object:  Table [dbo].[Area]    Script Date: 06.03.2021 15:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Area](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LayoutId] [int] NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[CoordX] [int] NOT NULL,
	[CoordY] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 06.03.2021 15:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](120) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[LayoutId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventArea]    Script Date: 06.03.2021 15:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventArea](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [int] NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[CoordX] [int] NOT NULL,
	[CoordY] [int] NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventSeat]    Script Date: 06.03.2021 15:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventSeat](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventAreaId] [int] NOT NULL,
	[Row] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[State] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Layout]    Script Date: 06.03.2021 15:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Layout](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VenueId] [int] NOT NULL,
	[Description] [nvarchar](120) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Seat]    Script Date: 06.03.2021 15:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Seat](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AreaId] [int] NOT NULL,
	[Row] [int] NOT NULL,
	[Number] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Venue]    Script Date: 06.03.2021 15:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Venue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](120) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Phone] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Area] ON 

INSERT [dbo].[Area] ([Id], [LayoutId], [Description], [CoordX], [CoordY]) VALUES (1, 1, N'First area of first layout', 1, 1)
INSERT [dbo].[Area] ([Id], [LayoutId], [Description], [CoordX], [CoordY]) VALUES (2, 1, N'Second area of first layout', 2, 2)
INSERT [dbo].[Area] ([Id], [LayoutId], [Description], [CoordX], [CoordY]) VALUES (3, 2, N'First area of second layout', 3, 3)
SET IDENTITY_INSERT [dbo].[Area] OFF
GO
SET IDENTITY_INSERT [dbo].[Event] ON 

INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId]) VALUES (1, N'Birthday', N'Birthday of Ramz Ahm (I apologize in advance...)', 1)
INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId]) VALUES (2, N'Send-off to jail', N'Send-off to jail for bad apologies', 2)
INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId]) VALUES (3, N'Release from prison', N'Release from prison after bad apology', 1)
INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId]) VALUES (4, N'Name', N'Description', 1)
SET IDENTITY_INSERT [dbo].[Event] OFF
GO
SET IDENTITY_INSERT [dbo].[EventArea] ON 

INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) VALUES (1, 1, N'In honor of such a person, you need to arrange a big holiday', 1, 1, CAST(10000 AS Decimal(18, 0)))
INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) VALUES (2, 2, N'Apparently the holiday was not big enough (or too expensive)', 3, 12, CAST(10 AS Decimal(18, 0)))
INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) VALUES (3, 3, N'Our boss is out of prison, and we need to have a big celebration in honor of this', 1, 1, CAST(10000 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[EventArea] OFF
GO
SET IDENTITY_INSERT [dbo].[EventSeat] ON 

INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [Row], [Number], [State]) VALUES (1, 1, 1, 1, 1)
INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [Row], [Number], [State]) VALUES (2, 2, 1, 2, 2)
INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [Row], [Number], [State]) VALUES (3, 3, 1, 3, 1)
SET IDENTITY_INSERT [dbo].[EventSeat] OFF
GO
SET IDENTITY_INSERT [dbo].[Layout] ON 

INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (1, 1, N'First layout')
INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (2, 1, N'Second layout')
INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (3, 2, N'First layout to second venue')
INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (4, 2, N'Second layout to second venue')
SET IDENTITY_INSERT [dbo].[Layout] OFF
GO
SET IDENTITY_INSERT [dbo].[Seat] ON 

INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (1, 1, 1, 1)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (2, 1, 1, 2)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (3, 1, 1, 3)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (4, 2, 2, 2)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (5, 2, 1, 1)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (6, 2, 2, 1)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (7, 3, 2, 4)
SET IDENTITY_INSERT [dbo].[Seat] OFF
GO
SET IDENTITY_INSERT [dbo].[Venue] ON 

INSERT [dbo].[Venue] ([Id], [Description], [Address], [Phone]) VALUES (1, N'First venue', N'First venue Address', N'123 45 678 90 12')
INSERT [dbo].[Venue] ([Id], [Description], [Address], [Phone]) VALUES (2, N'Second venue', N'Second venue Address', N'234 56 789 01 23')
INSERT [dbo].[Venue] ([Id], [Description], [Address], [Phone]) VALUES (3, N'Third venue', N'Third venue Address', N'345 67 890 12 34')
SET IDENTITY_INSERT [dbo].[Venue] OFF
GO
ALTER TABLE [dbo].[Area]  WITH CHECK ADD  CONSTRAINT [FK_Layout_Area] FOREIGN KEY([LayoutId])
REFERENCES [dbo].[Layout] ([Id])
GO
ALTER TABLE [dbo].[Area] CHECK CONSTRAINT [FK_Layout_Area]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Layout_Event] FOREIGN KEY([LayoutId])
REFERENCES [dbo].[Layout] ([Id])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Layout_Event]
GO
ALTER TABLE [dbo].[EventArea]  WITH CHECK ADD  CONSTRAINT [FK_Event_EventArea] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventArea] CHECK CONSTRAINT [FK_Event_EventArea]
GO
ALTER TABLE [dbo].[EventSeat]  WITH CHECK ADD  CONSTRAINT [FK_Area_EventSeat] FOREIGN KEY([EventAreaId])
REFERENCES [dbo].[EventArea] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventSeat] CHECK CONSTRAINT [FK_Area_EventSeat]
GO
ALTER TABLE [dbo].[Layout]  WITH CHECK ADD  CONSTRAINT [FK_Venue_Layout] FOREIGN KEY([VenueId])
REFERENCES [dbo].[Venue] ([Id])
GO
ALTER TABLE [dbo].[Layout] CHECK CONSTRAINT [FK_Venue_Layout]
GO
ALTER TABLE [dbo].[Seat]  WITH CHECK ADD  CONSTRAINT [FK_Area_Seat] FOREIGN KEY([AreaId])
REFERENCES [dbo].[Area] ([Id])
GO
ALTER TABLE [dbo].[Seat] CHECK CONSTRAINT [FK_Area_Seat]
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateEvent]    Script Date: 06.03.2021 15:38:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 --- CreateEvent procedure
CREATE PROCEDURE [dbo].[sp_CreateEvent]
	@name NVARCHAR(120),
	@description NVARCHAR(MAX),
	@layoutId INT
AS
INSERT INTO [Event]([Name], [Description], [LayoutId]) 
VALUES(@name, @description, @layoutId) SELECT SCOPE_IDENTITY()
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteEvent]    Script Date: 06.03.2021 15:38:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 --- DeleteEvent procedure
CREATE PROCEDURE [dbo].[sp_DeleteEvent]
	@id INT
AS
DELETE FROM [Event] WHERE [Id] = @id;

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllEvents]    Script Date: 06.03.2021 15:38:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 --- GetAllEvents procedure
 CREATE PROCEDURE [dbo].[sp_GetAllEvents]
AS
SELECT [Id], [Name], [Description], [LayoutId]
FROM [Event]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetEvent]    Script Date: 06.03.2021 15:38:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 --- GetEvent proceudre
 CREATE PROCEDURE [dbo].[sp_GetEvent]
	@id INT
AS
SELECT [Id], [Name], [Description], [LayoutId]
FROM [Event] 
WHERE [Event].Id = @id
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateEvent]    Script Date: 06.03.2021 15:38:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 --- UpdateEvent procedure
 CREATE PROCEDURE [dbo].[sp_UpdateEvent]
	@id INT,
	@name NVARCHAR(120),
	@description NVARCHAR(MAX),
	@layoutId INT
AS
UPDATE Event
SET [Name] = @name, [Description] = @description, [LayoutId] = @layoutId
WHERE [Id] = @id;

GO
USE [master]
GO
ALTER DATABASE [TicketManagementTests] SET  READ_WRITE 
GO
