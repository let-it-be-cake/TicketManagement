CREATE TABLE [dbo].[Area](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LayoutId] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CoordX] [int] NOT NULL,
	[CoordY] [int] NOT NULL,
 CONSTRAINT [PK_Areas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))