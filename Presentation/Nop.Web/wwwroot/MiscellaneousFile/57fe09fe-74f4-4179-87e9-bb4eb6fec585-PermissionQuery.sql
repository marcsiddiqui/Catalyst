IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'GradeManagement.ManageSections')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Sections','GradeManagement.ManageSections', 'GradeManagement')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'GradeManagement.ManageSections'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END