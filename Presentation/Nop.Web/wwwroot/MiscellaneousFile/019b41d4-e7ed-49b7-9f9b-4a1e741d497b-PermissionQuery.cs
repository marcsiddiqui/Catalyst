IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'ManageSubjects')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Subjects','ManageSubjects','Subjects')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'ManageSubjects'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END