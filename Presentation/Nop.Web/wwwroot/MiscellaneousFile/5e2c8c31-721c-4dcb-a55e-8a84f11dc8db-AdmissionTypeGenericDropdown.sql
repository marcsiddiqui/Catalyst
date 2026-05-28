;WITH CTE AS
(
	SELECT 14 EntityId, 'B-Form' [Text], 1 [Value], 1 OrderBy, NULL Color, 1 IsSystemOption, GETUTCDATE() CreatedOnUtc	UNION
	SELECT 14, 'StudentBirthCertificate',		2, 2, NULL, 1, GETUTCDATE()	UNION
	SELECT 14, 'FatherCNIC-Front',				3, 3, NULL, 1, GETUTCDATE()	UNION
	SELECT 14, 'FatherCNIC-Back',				4, 4, NULL, 1, GETUTCDATE()	UNION
	SELECT 14, 'MotherCNIC-Front',				5, 5, NULL, 1, GETUTCDATE()	UNION
	SELECT 14, 'MotherCNIC-Back',				6, 6, NULL, 1, GETUTCDATE()	UNION
	SELECT 14, 'LastSchoolResultCard',			7, 7, NULL, 1, GETUTCDATE()	UNION
	SELECT 14, 'LastSchoolLeavingCertificate',	8, 8, NULL, 1, GETUTCDATE()	UNION
	SELECT 14, 'FatherDeathCertificate',		9, 9, NULL, 1, GETUTCDATE()	UNION
	SELECT 14, 'MotherDeathCerfiticate',		10, 10, NULL, 1, GETUTCDATE() UNION
	SELECT 14, 'GuardianCNIC-Front',			11, 11, NULL, 1, GETUTCDATE()	UNION
	SELECT 14, 'GuardianCNIC-Back',				12, 12, NULL, 1, GETUTCDATE()	UNION
	SELECT 14, 'GuardianAuthorityLetter',		13, 13, NULL, 1, GETUTCDATE()
)
INSERT INTO GenericDropDownOption
	SELECT * FROM CTE C
	WHERE NOT EXISTS
	(
		SELECT * FROM GenericDropDownOption G WHERE G.EntityId = C.EntityId AND G.[Text] = C.[Text]
	)