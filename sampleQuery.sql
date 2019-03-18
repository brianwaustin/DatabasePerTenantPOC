USE [CatalogDb]

/* ================================================= 
	Purpose: Queries tenant databases and clears them
	if necessary.
   ================================================= */

SELECT * FROM [dbo].[Databases]

SELECT * FROM [dbo].[ElasticPools]

SELECT * FROM [dbo].[Servers]

SELECT * FROM [dbo].[Tenants]

/*
	DELETE FROM  [dbo].[Databases]
	DELETE FROM  [dbo].[ElasticPools]
	DELETE FROM  [dbo].[Servers]
	DELETE FROM  [dbo].[Tenants]
*/