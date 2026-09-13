# SQL Server Backup and Restore

## Backup

Run a full backup from a trusted SQL Server administration host:

```sql
BACKUP DATABASE [ECommerceDb]
TO DISK = N'/var/opt/mssql/backup/ECommerceDb-full.bak'
WITH INIT, COMPRESSION, CHECKSUM, STATS = 10;
RESTORE VERIFYONLY
FROM DISK = N'/var/opt/mssql/backup/ECommerceDb-full.bak';
```

Copy the backup to encrypted, versioned storage and test restore on a schedule. The backup path must be mounted and writable by SQL Server.

## Restore validation

Restore into a separate database first, verify schema and recent orders, then perform the production restore during an approved maintenance window. Record the restore timestamp and migration version in the deployment log.
