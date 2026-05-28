IF OBJECT_ID('AdmissionGradeDocumentRequirement') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[AdmissionGradeDocumentRequirement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GradeId] [int] NOT NULL,
	[AdmissionDocumentTypeId] [int] NOT NULL,
	[IsRequired] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOnUtc] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[AdmissionGradeDocumentRequirement] ADD  DEFAULT ((0)) FOR [GradeId]
ALTER TABLE [dbo].[AdmissionGradeDocumentRequirement] ADD  DEFAULT ((0)) FOR [AdmissionDocumentTypeId]
ALTER TABLE [dbo].[AdmissionGradeDocumentRequirement] ADD  DEFAULT ((1)) FOR [IsRequired]
ALTER TABLE [dbo].[AdmissionGradeDocumentRequirement] ADD  DEFAULT ((0)) FOR [Deleted]
ALTER TABLE [dbo].[AdmissionGradeDocumentRequirement] ADD  DEFAULT ((0)) FOR [CreatedBy]
ALTER TABLE [dbo].[AdmissionGradeDocumentRequirement] ADD  DEFAULT ((0)) FOR [UpdatedBy]
END
