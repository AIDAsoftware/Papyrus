SET migrate="..\..\..\packages\FluentMigrator.1.5.1.0\tools\Migrate.exe"

%migrate% -t=rollback --timeout=3600 -a .\Papyrus.Infrastructure.Migrations.dll -conn "Data Source=SIMA2SQL\DESARROLLO;Initial Catalog=Papyrus;Trusted_Connection=True;" -db sqlServer2012

call listmigrations.bat