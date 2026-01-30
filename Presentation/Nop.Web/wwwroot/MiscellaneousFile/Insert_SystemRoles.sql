DECLARE @Roles TABLE
(
    Name NVARCHAR(100),
    SystemName NVARCHAR(100)
);

INSERT INTO @Roles (Name, SystemName)
VALUES
('School Administrator', 'SchoolAdmin'),
('Principal', 'Principal'),
('Vice Principal', 'VicePrincipal'),
('Teacher', 'Teacher'),
('Parent', 'Parent'),
('Student', 'Student'),
('Receptionist', 'Receptionist'),
('Incharge', 'Incharge'),
('Examination', 'Examination');

INSERT INTO CustomerRole
(
    Name,
    SystemName,
    FreeShipping,
    TaxExempt,
    Active,
    IsSystemRole,
    EnablePasswordLifetime,
    OverrideTaxDisplayType,
    DefaultTaxDisplayTypeId,
    PurchasedWithProductId
)
SELECT
    r.Name,
    r.SystemName,
    0,  -- FreeShipping
    0,  -- TaxExempt
    1,  -- Active
    1,  -- IsSystemRole
    0,  -- EnablePasswordLifetime
    0,  -- OverrideTaxDisplayType
    0,  -- DefaultTaxDisplayTypeId
    0   -- PurchasedWithProductId
FROM @Roles r
WHERE NOT EXISTS
(
    SELECT 1 FROM CustomerRole cr WHERE cr.SystemName = r.SystemName
);