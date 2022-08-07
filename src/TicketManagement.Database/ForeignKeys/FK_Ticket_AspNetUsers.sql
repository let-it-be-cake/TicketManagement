ALTER TABLE [dbo].[Ticket]
ADD  CONSTRAINT [FK_Ticket_AspNetUsers] FOREIGN KEY([UserId])
	REFERENCES [dbo].[AspNetUsers] ([Id])