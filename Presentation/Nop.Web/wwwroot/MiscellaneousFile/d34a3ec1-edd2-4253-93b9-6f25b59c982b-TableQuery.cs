IF OBJECT_ID('AcademicYearGradeSectionStudentMapping') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[AcademicYearGradeSectionStudentMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AcademicYearGradeSectionMappingId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[TotalMarks] [decimal](18, 0) NOT NULL,
	[ObtainedMarks] [decimal](18, 0) NOT NULL,
	[Grade] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Discount] [decimal](18, 0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[AcademicYearGradeSectionStudentMapping] ADD  DEFAULT ((0)) FOR [AcademicYearGradeSectionMappingId]
ALTER TABLE [dbo].[AcademicYearGradeSectionStudentMapping] ADD  DEFAULT ((0)) FOR [CustomerId]
ALTER TABLE [dbo].[AcademicYearGradeSectionStudentMapping] ADD  DEFAULT ((0)) FOR [StatusId]
ALTER TABLE [dbo].[AcademicYearGradeSectionStudentMapping] ADD  DEFAULT ((0)) FOR [TotalMarks]
ALTER TABLE [dbo].[AcademicYearGradeSectionStudentMapping] ADD  DEFAULT ((0)) FOR [ObtainedMarks]
ALTER TABLE [dbo].[AcademicYearGradeSectionStudentMapping] ADD  DEFAULT ((0)) FOR [Discount]
END
