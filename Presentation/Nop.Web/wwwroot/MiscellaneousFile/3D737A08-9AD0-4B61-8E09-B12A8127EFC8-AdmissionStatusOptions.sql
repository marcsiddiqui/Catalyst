;WITH OptionsToInsert AS
(
    SELECT *
    FROM (VALUES
        (10, 'Draft',               1, 1, NULL, 1),
        (10, 'Submitted',           2, 2, NULL, 1),
        (10, 'InterviewScheduled',  3, 3, NULL, 1),
        (10, 'InterviewInProgress', 4, 4, NULL, 1),
        (10, 'InterviewPassed',     5, 5, NULL, 1),
        (10, 'InterviewFailed',     6, 6, NULL, 1),
        (10, 'RetakeRequired',      7, 7, NULL, 1),
        (10, 'Approved',            8, 8, NULL, 1),
        (10, 'Rejected',            9, 9, NULL, 1),
        (10, 'Cancelled',          10, 10, NULL, 1)
    ) AS v(EntityId, [Text], [Value], OrderBy, Color, IsSystemOption)
)
INSERT INTO GenericDropDownOption
(
    EntityId,
    [Text],
    [Value],
    OrderBy,
    Color,
    IsSystemOption,
    CreatedOnUtc
)
SELECT
    o.EntityId,
    o.[Text],
    o.[Value],
    o.OrderBy,
    o.Color,
    o.IsSystemOption,
    GETUTCDATE()
FROM OptionsToInsert o
WHERE NOT EXISTS
(
    SELECT 1
    FROM GenericDropDownOption g
    WHERE g.EntityId = o.EntityId
      AND g.[Text] = o.[Text]
);
