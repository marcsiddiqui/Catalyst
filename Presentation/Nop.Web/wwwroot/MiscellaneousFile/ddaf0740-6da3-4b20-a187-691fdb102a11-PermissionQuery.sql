IF NOT EXISTS(SELECT * FROM PermissionRecord WHERE [SystemName] = 'GradeManagement.ManageGrades')
BEGIN
    INSERT INTO PermissionRecord([Name],[SystemName],[Category])
    VALUES('Admin area. Manage Grades','GradeManagement.ManageGrades','GradeManagement')
    
    INSERT INTO PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id)
    VALUES((SELECT Id FROM PermissionRecord WHERE [SystemName] = 'GradeManagement.ManageGrades'), (SELECT Id FROM CustomerRole WHERE [Name] = 'Administrators'))
END