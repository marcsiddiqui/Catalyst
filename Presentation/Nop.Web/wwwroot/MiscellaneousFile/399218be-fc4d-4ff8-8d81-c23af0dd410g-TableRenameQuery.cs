IF OBJECT_ID('FeePayments') IS NOT NULL AND OBJECT_ID('FeePayment') IS NOT NULL
BEGIN
EXEC SP_RENAME 'FeePayments', 'FeePayment'
END

IF OBJECT_ID('AcadamicYearTerm') IS NOT NULL AND OBJECT_ID('AcademicYearTerm') IS NOT NULL
BEGIN
EXEC SP_RENAME 'AcadamicYearTerm', 'AcademicYearTerm'
END
