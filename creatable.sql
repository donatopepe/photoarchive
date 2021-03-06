USE [PhotoCoilsDb]
GO

/****** Object:  Table [dbo].[PhotoArchives]    Script Date: 24/10/2019 15:41:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PhotoArchives](
	[Id] [uniqueidentifier] ROWGUIDCOL NOT NULL UNIQUE, 
	[Cam] [nvarchar](max) NULL,
	[File] [varbinary](max) FILESTREAM NULL,
	[ContentType] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[Parent] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedTimestamp] [datetime2](7) NOT NULL,
	[UpdatedTimestamp] [datetime2](7) NOT NULL,	
 CONSTRAINT [PK_PhotoArchives] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO