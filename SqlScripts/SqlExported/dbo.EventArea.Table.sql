USE [TicketManagement]
GO
/****** Object:  Table [dbo].[EventArea]    Script Date: 26.02.2021 0:54:43 ******/
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
SET IDENTITY_INSERT [dbo].[EventArea] ON 

INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) VALUES (1, 1, N'In honor of such a person, you need to arrange a big holiday', 1, 1, CAST(10000 AS Decimal(18, 0)))
INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) VALUES (2, 2, N'Apparently the holiday was not big enough (or too expensive)', 3, 12, CAST(10 AS Decimal(18, 0)))
INSERT [dbo].[EventArea] ([Id], [EventId], [Description], [CoordX], [CoordY], [Price]) VALUES (3, 3, N'Our boss is out of prison, and we need to have a big celebration in honor of this', 1, 1, CAST(10000 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[EventArea] OFF
GO
ALTER TABLE [dbo].[EventArea]  WITH CHECK ADD  CONSTRAINT [FK_Event_EventArea] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventArea] CHECK CONSTRAINT [FK_Event_EventArea]
GO
