IF OBJECT_ID('SubjectGradeMapping') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[SubjectGradeMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GradeId] [int] NOT NULL,
	[SubjectId] [int] NOT NULL,
	[LabFee] [decimal](18, 0) NOT NULL,
	[SectionId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[SubjectGradeMapping] ADD  DEFAULT ((0)) FOR [GradeId]
ALTER TABLE [dbo].[SubjectGradeMapping] ADD  DEFAULT ((0)) FOR [SubjectId]
ALTER TABLE [dbo].[SubjectGradeMapping] ADD  DEFAULT ((0)) FOR [LabFee]
END
