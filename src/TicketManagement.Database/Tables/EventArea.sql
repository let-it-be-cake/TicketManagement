CREATE TABLE [dbo].[EventArea](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CoordX] [int] NOT NULL,
	[CoordY] [int] NOT NULL,
	[Price] [decimal](18, 2) NULL,
 CONSTRAINT [PK_EventAreas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))