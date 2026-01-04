IF COL_LENGTH('PermissionRecord', 'IsEnabledOnInstall') IS NULL
BEGIN
	ALTER TABLE PermissionRecord
	ADD IsEnabledOnInstall BIT NOT NULL DEFAULT 0
END