ServiceRequestDemo
--------------------

This solution has been implemented using .Net Core following Service-Repository pattern.

This uses SQL Server as the database and please update the connection string in appsettings.json file before running the application. The create table script is included in the DBScripts folder.

For logging, I've used Serilog and the path needs to be updated in the appsettings.json file for the log file.

Unit tests are written using xUnit and it validates both the Service as well as the Controller class.

I've used AutoMapper to map the database model class as well as the DTO class.
