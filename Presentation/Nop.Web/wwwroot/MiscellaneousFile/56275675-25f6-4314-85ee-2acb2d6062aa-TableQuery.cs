IF OBJECT_ID('Holiday') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Holiday](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AcademicYearId] [int] NOT NULL,
	[Name] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DateFromUtc] [datetime] NOT NULL,
	[DateToUtc] [datetime] NOT NULL,
	[StoreId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NOT NULL,
	[LimitedToStores] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[Holiday] ADD  DEFAULT ((0)) FOR [AcademicYearId]
ALTER TABLE [dbo].[Holiday] ADD  DEFAULT ((0)) FOR [StoreId]
ALTER TABLE [dbo].[Holiday] ADD  DEFAULT ((0)) FOR [CreatedBy]
ALTER TABLE [dbo].[Holiday] ADD  DEFAULT ((0)) FOR [UpdatedBy]
ALTER TABLE [dbo].[Holiday] ADD  DEFAULT ((0)) FOR [Deleted]
ALTER TABLE [dbo].[Holiday] ADD  DEFAULT ((0)) FOR [LimitedToStores]
END
