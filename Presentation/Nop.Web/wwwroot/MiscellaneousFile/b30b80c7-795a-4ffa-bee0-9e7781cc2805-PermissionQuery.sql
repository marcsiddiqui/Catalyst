IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'HolidaysNEvents.ManageHolidays')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Holidays','HolidaysNEvents.ManageHolidays','HolidaysNEvents')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'HolidaysNEvents.ManageHolidays'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END