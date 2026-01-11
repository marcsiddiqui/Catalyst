IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'ManageAcademicYears')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage AcademicYears','ManageAcademicYears','AcademicYears')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'ManageAcademicYears'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END