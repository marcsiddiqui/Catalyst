IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'ManageAdmissions')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Admissions','Admissions.ManageAdmissions','Admissions')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'ManageAdmissions'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END