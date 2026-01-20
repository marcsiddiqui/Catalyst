IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'HolidaysNEvents.ManageEvents')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Events','HolidaysNEvents.ManageEvents','HolidaysNEvents')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'HolidaysNEvents.ManageEvents'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END