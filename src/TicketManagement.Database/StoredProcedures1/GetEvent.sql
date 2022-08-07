 --- GetEvent proceudre
CREATE PROCEDURE [dbo].[sp_GetEvent]
	@id INT
AS
SELECT [Id], [Name], [Description], [LayoutId], [DateTimeStart], [DateTimeEnd], [ImageUrl]
FROM [Event] 
WHERE [Event].Id = @id

GO