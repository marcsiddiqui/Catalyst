IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'ManageGrades')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Grades','ManageGrades','GradeManagement')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'ManageGrades'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END