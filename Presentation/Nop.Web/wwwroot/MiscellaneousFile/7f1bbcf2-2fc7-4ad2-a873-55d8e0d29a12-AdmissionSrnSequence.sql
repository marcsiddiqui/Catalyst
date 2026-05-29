IF NOT EXISTS (
    SELECT 1
    FROM sys.sequences
    WHERE name = N'AdmissionSrnSequence'
        AND SCHEMA_NAME(schema_id) = N'dbo'
)
BEGIN
    DECLARE @startWith INT = 100000;

    IF OBJECT_ID(N'[dbo].[Admission]', N'U') IS NOT NULL
    BEGIN
        SELECT @startWith = CASE
            WHEN ISNULL(MAX([SRN]), 0) + 1 > @startWith THEN ISNULL(MAX([SRN]), 0) + 1
            ELSE @startWith
        END
        FROM [dbo].[Admission];
    END

    DECLARE @sql NVARCHAR(MAX) = N'CREATE SEQUENCE [dbo].[AdmissionSrnSequence] AS INT START WITH '
        + CAST(@startWith AS NVARCHAR(20))
        + N' INCREMENT BY 1;';

    EXEC sp_executesql @sql;
END
