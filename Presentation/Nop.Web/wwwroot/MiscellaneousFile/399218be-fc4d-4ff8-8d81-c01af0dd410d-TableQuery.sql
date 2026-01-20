IF OBJECT_ID('FeePayments') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[FeePayments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FeeId] [int] NOT NULL,
	[PaidAmount] [decimal](18, 0) NOT NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[UpdatedOnUtc] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[FeePayments] ADD  DEFAULT ((0)) FOR [FeeId]
ALTER TABLE [dbo].[FeePayments] ADD  DEFAULT ((0)) FOR [PaidAmount]
ALTER TABLE [dbo].[FeePayments] ADD  DEFAULT ((0)) FOR [StatusId]
ALTER TABLE [dbo].[FeePayments] ADD  DEFAULT ((0)) FOR [CreatedBy]
ALTER TABLE [dbo].[FeePayments] ADD  DEFAULT ((0)) FOR [UpdatedBy]
END
