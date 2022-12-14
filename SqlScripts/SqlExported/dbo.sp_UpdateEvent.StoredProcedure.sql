USE [TicketManagement]
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateEvent]    Script Date: 26.02.2021 0:54:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateEvent]
	@id INT,
	@name NVARCHAR(120),
	@description NVARCHAR(MAX),
	@layoutId INT
AS
UPDATE Event
SET [Name] = @name, [Description] = @description, [LayoutId] = @description
WHERE [Id] = @id;
GO
