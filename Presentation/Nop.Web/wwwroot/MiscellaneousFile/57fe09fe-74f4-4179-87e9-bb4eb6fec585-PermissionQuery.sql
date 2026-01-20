IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'ManageSections')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Sections','ManageSections', 'GradeManagement')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'ManageSections'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END