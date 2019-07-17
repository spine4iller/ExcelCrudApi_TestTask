USE [ExcelDb]
GO

/****** Object:  Table [dbo].[ExcelDataTable]    Script Date: 17.07.2019 22:00:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ExcelDataTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[dateCreate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[ExcelDataTable] ADD  DEFAULT (getdate()) FOR [dateCreate]
GO

