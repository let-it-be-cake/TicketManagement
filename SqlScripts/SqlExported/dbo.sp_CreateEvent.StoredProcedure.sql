USE [TicketManagement]
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateEvent]    Script Date: 26.02.2021 0:54:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateEvent]
	@name NVARCHAR(120),
	@description NVARCHAR(MAX),
	@layoutId INT
AS
INSERT INTO [Event]([Name], [Description], [LayoutId]) 
VALUES(@name, @description, @layoutId)
GO
