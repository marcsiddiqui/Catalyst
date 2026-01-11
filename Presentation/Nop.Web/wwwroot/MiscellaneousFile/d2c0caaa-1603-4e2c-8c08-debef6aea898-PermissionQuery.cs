IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'ManageEvents')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Events','ManageEvents','HolidaysNEvents')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'ManageEvents'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END