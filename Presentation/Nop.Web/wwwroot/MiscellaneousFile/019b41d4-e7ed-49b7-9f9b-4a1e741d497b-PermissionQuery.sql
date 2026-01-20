IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'Subjects.ManageSubjects')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Subjects','Subjects.ManageSubjects','Subjects')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'Subjects.ManageSubjects'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END