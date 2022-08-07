 --- UpdateEvent procedure
CREATE PROCEDURE [dbo].[sp_UpdateEvent]
	@id INT,
	@name NVARCHAR(120),
	@description NVARCHAR(MAX),
	@layoutId INT,
	@dateTimeStart DATETIME,
	@dateTimeEnd DATETIME,
	@imageUrl NVARCHAR(MAX)
AS
UPDATE Event
SET [Name] = @name, [Description] = @description, [LayoutId] = @layoutId, [DateTimeStart] = @dateTimeStart, [DateTimeEnd] = @dateTimeEnd, [ImageUrl] = @imageUrl
WHERE [Id] = @id;

GO