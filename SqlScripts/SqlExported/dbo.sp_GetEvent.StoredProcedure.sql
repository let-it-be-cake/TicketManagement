USE [TicketManagementTests]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetEvent]    Script Date: 06.03.2021 15:36:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 --- GetEvent proceudre
 ALTER PROCEDURE [dbo].[sp_GetEvent]
	@id INT
AS
SELECT [Id], [Name], [Description], [LayoutId]
FROM [Event] 
WHERE [Event].Id = @id
