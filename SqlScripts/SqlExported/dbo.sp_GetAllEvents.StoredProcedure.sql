USE [TicketManagementTests]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllEvents]    Script Date: 06.03.2021 15:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 --- GetAllEvents procedure
 ALTER PROCEDURE [dbo].[sp_GetAllEvents]
AS
SELECT [Id], [Name], [Description], [LayoutId]
FROM [Event]
