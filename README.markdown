#Extremely simple mssql restore console app
This is a simple console application using Sql Server Management Objects to perform a database restore from a backup file.

##Usage
sqlbackup.exe [server] [database] [backup's filepath]
If there is no database with the given name on the server, it will create a new one, otherwise, it will perform a restore and replace the given database with the backup.
