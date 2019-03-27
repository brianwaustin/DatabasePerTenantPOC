# DatabasePerTenantPOC
Sample project of the database per tenant sharding pattern for .Net Core

Limitations:

EF applications that use LocalDb first need to migrate to a regular SQL Server database before using elastic database client library. Scaling out an application through sharding with Elastic Scale is not possible with LocalDb. Note that development can still use LocalDb.

[Read More](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-elastic-scale-use-entity-framework-applications-visual-studio)

Requires a local install of SQL Express

Need to look up your dynamic TCP port: (e.g. 51011)

[Unable to connect to SQL Server Express](https://razorsql.com/docs/support_sqlserver_express.html)

data-dependent routing (DDR)