CREATE TABLE [dbo].[Seat](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AreaId] [int] NOT NULL,
	[Row] [int] NOT NULL,
	[Number] [int] NOT NULL,
 CONSTRAINT [PK_Seats] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))