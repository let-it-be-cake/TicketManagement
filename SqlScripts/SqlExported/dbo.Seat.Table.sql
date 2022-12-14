USE [TicketManagement]
GO
/****** Object:  Table [dbo].[Seat]    Script Date: 26.02.2021 0:54:43 ******/
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
SET IDENTITY_INSERT [dbo].[Seat] ON 

INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (1, 1, 1, 1)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (2, 1, 1, 2)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (3, 1, 1, 3)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (4, 1, 2, 2)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (5, 2, 1, 1)
INSERT [dbo].[Seat] ([Id], [AreaId], [Row], [Number]) VALUES (6, 1, 2, 1)
SET IDENTITY_INSERT [dbo].[Seat] OFF
GO
ALTER TABLE [dbo].[Seat]  WITH CHECK ADD  CONSTRAINT [FK_Area_Seat] FOREIGN KEY([AreaId])
REFERENCES [dbo].[Area] ([Id])
GO
ALTER TABLE [dbo].[Seat] CHECK CONSTRAINT [FK_Area_Seat]
GO
