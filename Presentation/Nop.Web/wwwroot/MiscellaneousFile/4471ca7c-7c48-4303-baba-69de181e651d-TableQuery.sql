IF OBJECT_ID('AcademicYearGradeSectionMapping') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[AcademicYearGradeSectionMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AcademicYearId] [int] NOT NULL,
	[GradeId] [int] NOT NULL,
	[SectionId] [int] NOT NULL,
	[ExamTermCount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[AcademicYearGradeSectionMapping] ADD  DEFAULT ((0)) FOR [AcademicYearId]
ALTER TABLE [dbo].[AcademicYearGradeSectionMapping] ADD  DEFAULT ((0)) FOR [GradeId]
ALTER TABLE [dbo].[AcademicYearGradeSectionMapping] ADD  DEFAULT ((0)) FOR [SectionId]
ALTER TABLE [dbo].[AcademicYearGradeSectionMapping] ADD  DEFAULT ((0)) FOR [ExamTermCount]
END
