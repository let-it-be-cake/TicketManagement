USE [TicketManagement]
GO
/****** Object:  Table [dbo].[Layout]    Script Date: 26.02.2021 0:54:43 ******/
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
SET IDENTITY_INSERT [dbo].[Layout] ON 

INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (1, 1, N'First layout')
INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (2, 1, N'Second layout')
INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (3, 2, N'First layout to second venue')
INSERT [dbo].[Layout] ([Id], [VenueId], [Description]) VALUES (4, 2, N'Second layout to second venue')
SET IDENTITY_INSERT [dbo].[Layout] OFF
GO
ALTER TABLE [dbo].[Layout]  WITH CHECK ADD  CONSTRAINT [FK_Venue_Layout] FOREIGN KEY([VenueId])
REFERENCES [dbo].[Venue] ([Id])
GO
ALTER TABLE [dbo].[Layout] CHECK CONSTRAINT [FK_Venue_Layout]
GO
