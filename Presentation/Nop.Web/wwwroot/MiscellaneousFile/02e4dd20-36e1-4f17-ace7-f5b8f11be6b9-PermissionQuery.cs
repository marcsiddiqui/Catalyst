IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'ManageFees')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Fees','ManageFees','Fees')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'ManageFees'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END