IF OBJECT_ID('Student') IS NULL
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Student](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[GrNumber] [int] NOT NULL,
	[FirstName] [nvarchar](400) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MiddleName] [nvarchar](400) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LastName] [nvarchar](400) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PreviousSchoolId] [int] NOT NULL,
	[IdentificationMark] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BirthCity] [int] NOT NULL,
	[Allergies] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MedicalNotes] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[GuardianTypeId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
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

ALTER TABLE [dbo].[Student] ADD  DEFAULT ((0)) FOR [GrNumber]
ALTER TABLE [dbo].[Student] ADD  DEFAULT ((0)) FOR [PreviousSchoolId]
ALTER TABLE [dbo].[Student] ADD  DEFAULT ((0)) FOR [BirthCity]
ALTER TABLE [dbo].[Student] ADD  DEFAULT ((0)) FOR [GuardianTypeId]
ALTER TABLE [dbo].[Student] ADD  DEFAULT ((1)) FOR [IsActive]
ALTER TABLE [dbo].[Student] ADD  DEFAULT ((0)) FOR [Deleted]
ALTER TABLE [dbo].[Student] ADD  DEFAULT ((0)) FOR [CreatedBy]
ALTER TABLE [dbo].[Student] ADD  DEFAULT ((0)) FOR [UpdatedBy]
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
END
