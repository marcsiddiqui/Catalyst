IF OBJECT_ID('Fee') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Fee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AcademicYearGradeSectionMappingId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[Discount] [decimal](18, 0) NOT NULL,
	[FeeTypeId] [int] NOT NULL,
	[FeeDate] [datetime] NOT NULL,
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

ALTER TABLE [dbo].[Fee] ADD  DEFAULT ((0)) FOR [AcademicYearGradeSectionMappingId]
ALTER TABLE [dbo].[Fee] ADD  DEFAULT ((0)) FOR [CustomerId]
ALTER TABLE [dbo].[Fee] ADD  DEFAULT ((0)) FOR [Amount]
ALTER TABLE [dbo].[Fee] ADD  DEFAULT ((0)) FOR [Discount]
ALTER TABLE [dbo].[Fee] ADD  DEFAULT ((0)) FOR [FeeTypeId]
ALTER TABLE [dbo].[Fee] ADD  DEFAULT ((0)) FOR [Deleted]
ALTER TABLE [dbo].[Fee] ADD  DEFAULT ((0)) FOR [CreatedBy]
ALTER TABLE [dbo].[Fee] ADD  DEFAULT ((0)) FOR [UpdatedBy]
END
