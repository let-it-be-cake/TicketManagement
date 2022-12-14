USE [TicketManagement]
GO
/****** Object:  Table [dbo].[Venue]    Script Date: 26.02.2021 0:54:43 ******/
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
SET IDENTITY_INSERT [dbo].[Venue] ON 

INSERT [dbo].[Venue] ([Id], [Description], [Address], [Phone]) VALUES (1, N'First venue', N'First venue address', N'123 45 678 90 12')
INSERT [dbo].[Venue] ([Id], [Description], [Address], [Phone]) VALUES (2, N'Second venue', N'Second venue Address', N'234 56 789 01 23')
INSERT [dbo].[Venue] ([Id], [Description], [Address], [Phone]) VALUES (3, N'Third venue', N'Third venue address', N'345 67 890 12 34')
SET IDENTITY_INSERT [dbo].[Venue] OFF
GO
