CREATE TABLE [dbo].[Venue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
 CONSTRAINT [PK_Venues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))