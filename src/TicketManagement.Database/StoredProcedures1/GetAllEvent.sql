 --- GetAllEvents procedure
CREATE PROCEDURE [dbo].[sp_GetAllEvents]
AS
SELECT [Id], [Name], [Description], [LayoutId], [DateTimeStart], [DateTimeEnd], [ImageUrl]
FROM [Event]

GO