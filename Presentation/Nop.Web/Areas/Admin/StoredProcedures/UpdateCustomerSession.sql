CREATE OR ALTER PROCEDURE [dbo].[UpdateCustomerSession] 
AS
BEGIN 
     UPDATE [CustomerSession]
     SET [IsActive] = 0 where [ExpiresOnUtc] < GETUTCDATE() and IsActive = 1;
END