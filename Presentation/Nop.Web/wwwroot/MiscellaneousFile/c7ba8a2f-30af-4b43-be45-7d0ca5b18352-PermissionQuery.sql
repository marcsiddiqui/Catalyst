IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'ManageStudents')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Students','Students.ManageStudents','Students')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'ManageStudents'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END