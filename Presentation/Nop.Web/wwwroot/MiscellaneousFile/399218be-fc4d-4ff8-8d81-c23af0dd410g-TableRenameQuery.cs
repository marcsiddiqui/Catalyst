IF OBJECT_ID('FeePayments') IS NOT NULL
BEGIN
EXEC SP_RENAME 'FeePayments', 'FeePayment'
END
