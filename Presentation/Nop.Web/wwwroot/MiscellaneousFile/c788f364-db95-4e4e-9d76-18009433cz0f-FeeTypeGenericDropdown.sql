;WITH CTE AS
(
	SELECT 9 EntityId, 'Registration' [Text], 1 [Value], 1 OrderBy, NULL Color, 1 IsSystemOption, GETUTCDATE() CreatedOnUtc	UNION
	SELECT 9, 'Admission',		 2, 2, NULL, 1, GETUTCDATE()	UNION
	SELECT 9, 'Monthly',		 3, 3, NULL, 1, GETUTCDATE()	UNION
	SELECT 9, 'Examination',	 4, 4, NULL, 1, GETUTCDATE()	UNION
	SELECT 9, 'ScienceLab',		 5, 5, NULL, 1, GETUTCDATE()	UNION
	SELECT 9, 'ComputerLab',	 6, 6, NULL, 1, GETUTCDATE()	UNION
	SELECT 9, 'Library',		 7, 7, NULL, 1, GETUTCDATE()	UNION
	SELECT 9, 'Stationery',		 8, 8, NULL, 1, GETUTCDATE()	UNION
	SELECT 9, 'Picnic',			 9, 9, NULL, 1, GETUTCDATE()	UNION
	SELECT 9, 'Other',			 10, 10, NULL, 1, GETUTCDATE()
)
INSERT INTO GenericDropDownOption
	SELECT * FROM CTE C
	WHERE NOT EXISTS
	(
		SELECT * FROM GenericDropDownOption G WHERE G.EntityId = C.EntityId AND G.[Text] = C.[Text]
	)