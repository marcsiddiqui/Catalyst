IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'ManageAcadamicYearTerms')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage AcadamicYearTerms','ManageAcadamicYearTerms','AcadamicYears')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'ManageAcadamicYearTerms'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END