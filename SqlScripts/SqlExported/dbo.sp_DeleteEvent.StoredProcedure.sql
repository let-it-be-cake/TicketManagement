USE [TicketManagement]
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteEvent]    Script Date: 26.02.2021 0:54:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteEvent]
	@id INT
AS
DELETE FROM [Event] WHERE [Id] = @id;
GO
