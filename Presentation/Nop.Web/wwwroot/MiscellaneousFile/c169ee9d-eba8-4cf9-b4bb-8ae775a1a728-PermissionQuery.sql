IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'GenericDropDowns.ManageGenericDropDownOptions')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage GenericDropDownOptions','GenericDropDowns.ManageGenericDropDownOptions','GenericDropDowns')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'GenericDropDowns.ManageGenericDropDownOptions'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END