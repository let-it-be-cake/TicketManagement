CREATE TABLE [dbo].[Event](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[LayoutId] [int] NOT NULL,
	[DateTimeStart] [datetime2](7) NOT NULL,
	[DateTimeEnd] [datetime2](7) NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))