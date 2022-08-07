ALTER TABLE [dbo].[EventSeat]
ADD CONSTRAINT [FK_EventSeat_Ticket] FOREIGN KEY([TicketId])
	REFERENCES [dbo].[Ticket] ([Id])