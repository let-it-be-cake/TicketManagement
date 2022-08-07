CREATE TABLE [dbo].[EventSeat](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventAreaId] [int] NOT NULL,
	[TicketId] [int] NULL,
	[Row] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_EventSeats] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))