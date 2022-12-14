USE [TicketManagement]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 26.02.2021 0:54:43 ******/
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
SET IDENTITY_INSERT [dbo].[Event] ON 

INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId]) VALUES (1, N'Birthday', N'Birthday of Ramz Ahm (I apologize in advance...)', 1)
INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId]) VALUES (2, N'Send-off to jail', N'Send-off to jail for bad apologies', 2)
INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId]) VALUES (3, N'Release from prison', N'Release from prison after bad apology', 1)
INSERT [dbo].[Event] ([Id], [Name], [Description], [LayoutId]) VALUES (4, N'Name', N'Description', 1)
SET IDENTITY_INSERT [dbo].[Event] OFF
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Layout_Event] FOREIGN KEY([LayoutId])
REFERENCES [dbo].[Layout] ([Id])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Layout_Event]
GO
