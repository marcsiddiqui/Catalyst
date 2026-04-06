IF OBJECT_ID('AcademicYearGradeSectionEventMapping') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[AcademicYearGradeSectionEventMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [int] NOT NULL,
	[AcademicYearGradeSectionMappingId] [int] NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[AcademicYearGradeSectionEventMapping] ADD  DEFAULT ((0)) FOR [EventId]
ALTER TABLE [dbo].[AcademicYearGradeSectionEventMapping] ADD  DEFAULT ((0)) FOR [AcademicYearGradeSectionMappingId]
ALTER TABLE [dbo].[AcademicYearGradeSectionEventMapping] ADD  DEFAULT ((0)) FOR [Amount]
END
