USE [CatalogDb]

/* ================================================= 
	Purpose: Queries shard catalog databases and 
	clears them if necessary.
   ================================================= */

SELECT * FROM [dbo].[Databases]

SELECT * FROM [dbo].[ElasticPools]

SELECT * FROM [dbo].[Servers]

SELECT * FROM [dbo].[Tenants]

SELECT * FROM [dbo].[AspNetUsers]

/*
	DELETE FROM  [dbo].[Databases]
	DELETE FROM  [dbo].[ElasticPools]
	DELETE FROM  [dbo].[Servers]
	DELETE FROM  [dbo].[Tenants]
*/