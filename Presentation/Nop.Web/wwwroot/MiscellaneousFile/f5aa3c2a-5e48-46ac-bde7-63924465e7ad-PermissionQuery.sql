IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'AcademicYears.ManageAcademicYears')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage AcademicYears','AcademicYears.ManageAcademicYears','AcademicYears')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'AcademicYears.ManageAcademicYears'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END