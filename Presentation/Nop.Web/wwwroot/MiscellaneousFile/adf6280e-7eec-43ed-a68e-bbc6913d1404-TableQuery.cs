IF OBJECT_ID('AcadamicYearTerm') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[AcadamicYearTerm](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AcademicYearGradeSectionMappingId] [int] NOT NULL,
	[ConductanceOrder] [int] NOT NULL,
	[Name] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Weitage] [decimal](18, 0) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[AcadamicYearTerm] ADD  DEFAULT ((0)) FOR [AcademicYearGradeSectionMappingId]
ALTER TABLE [dbo].[AcadamicYearTerm] ADD  DEFAULT ((0)) FOR [ConductanceOrder]
ALTER TABLE [dbo].[AcadamicYearTerm] ADD  DEFAULT ((0)) FOR [Weitage]
ALTER TABLE [dbo].[AcadamicYearTerm] ADD  DEFAULT ((0)) FOR [CreatedBy]
ALTER TABLE [dbo].[AcadamicYearTerm] ADD  DEFAULT ((0)) FOR [UpdatedBy]
ALTER TABLE [dbo].[AcadamicYearTerm] ADD  DEFAULT ((0)) FOR [Deleted]
END
