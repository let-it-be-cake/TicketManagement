USE [TicketManagement];
GO
 --- DeleteEvent procedure
CREATE PROCEDURE [dbo].[sp_DeleteEvent]
	@id INT
AS
DELETE FROM [Event] WHERE [Id] = @id;