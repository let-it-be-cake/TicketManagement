 --- CreateEvent procedure
CREATE PROCEDURE [dbo].[sp_CreateEvent]
	@name NVARCHAR(120),
	@description NVARCHAR(MAX),
	@layoutId INT,
	@dateTimeStart DATETIME2,
	@dateTimeEnd DATETIME2,
	@imageUrl NVARCHAR(MAX),
	@eventId INT OUTPUT
AS
BEGIN
	INSERT INTO [dbo].[Event]([Name], [Description], [LayoutId], [DateTimeStart], [DateTimeEnd], [ImageUrl]) 
	VALUES(@name, @description, @layoutId, @dateTimeStart, @dateTimeEnd, @imageUrl)
	SET @eventId = (SELECT SCOPE_IDENTITY())
END
GO