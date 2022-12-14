USE [TicketManagement]
GO
/****** Object:  Table [dbo].[EventSeat]    Script Date: 26.02.2021 0:54:43 ******/
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
SET IDENTITY_INSERT [dbo].[EventSeat] ON 

INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [Row], [Number], [State]) VALUES (1, 1, 1, 200, 1)
INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [Row], [Number], [State]) VALUES (2, 2, 12, 15, 2)
INSERT [dbo].[EventSeat] ([Id], [EventAreaId], [Row], [Number], [State]) VALUES (3, 3, 1, 150, 1)
SET IDENTITY_INSERT [dbo].[EventSeat] OFF
GO
ALTER TABLE [dbo].[EventSeat]  WITH CHECK ADD  CONSTRAINT [FK_Area_EventSeat] FOREIGN KEY([EventAreaId])
REFERENCES [dbo].[EventArea] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventSeat] CHECK CONSTRAINT [FK_Area_EventSeat]
GO
