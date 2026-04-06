IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'AcademicYears.ManageAcademicYearTerms')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage AcademicYearTerms','AcademicYears.ManageAcademicYearTerms','AcademicYears')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'AcademicYears.ManageAcademicYearTerms'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END