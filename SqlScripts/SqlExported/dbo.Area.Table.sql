USE [TicketManagement]
GO
/****** Object:  Table [dbo].[Area]    Script Date: 26.02.2021 0:54:43 ******/
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
SET IDENTITY_INSERT [dbo].[Area] ON 

INSERT [dbo].[Area] ([Id], [LayoutId], [Description], [CoordX], [CoordY]) VALUES (1, 1, N'First area of first layout', 1, 1)
INSERT [dbo].[Area] ([Id], [LayoutId], [Description], [CoordX], [CoordY]) VALUES (2, 1, N'Second area of first layout', 1, 1)
INSERT [dbo].[Area] ([Id], [LayoutId], [Description], [CoordX], [CoordY]) VALUES (3, 2, N'First area of second layout', 4, 4)
SET IDENTITY_INSERT [dbo].[Area] OFF
GO
ALTER TABLE [dbo].[Area]  WITH CHECK ADD  CONSTRAINT [FK_Layout_Area] FOREIGN KEY([LayoutId])
REFERENCES [dbo].[Layout] ([Id])
GO
ALTER TABLE [dbo].[Area] CHECK CONSTRAINT [FK_Layout_Area]
GO
