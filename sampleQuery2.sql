
/* ================================================= 
	Purpose: Queries tenant databases and clears them
	if necessary.
   ================================================= */

SELECT * FROM [TenantDatabase1].[dbo].[Customers]
SELECT * FROM [TenantDatabase2].[dbo].[Customers]

/*
	DELETE FROM [TenantDatabase1].[dbo].[Customers]
	DELETE FROM [TenantDatabase2].[dbo].[Customers]
*/