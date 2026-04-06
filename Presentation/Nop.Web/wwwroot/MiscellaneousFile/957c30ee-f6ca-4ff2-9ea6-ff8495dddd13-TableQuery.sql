IF OBJECT_ID('AcademicYear') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[AcademicYear](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[AcademicYear] ADD  DEFAULT ((0)) FOR [CreatedBy]
ALTER TABLE [dbo].[AcademicYear] ADD  DEFAULT ((0)) FOR [UpdatedBy]
ALTER TABLE [dbo].[AcademicYear] ADD  DEFAULT ((0)) FOR [Deleted]
END
