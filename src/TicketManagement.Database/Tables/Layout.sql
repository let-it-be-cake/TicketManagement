CREATE TABLE [dbo].[Layout](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VenueId] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Layouts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))