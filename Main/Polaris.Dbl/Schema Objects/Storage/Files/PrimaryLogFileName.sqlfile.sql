
ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [PrimaryLogFileName], FILENAME = 'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\Polaris_log.ldf', SIZE = 1024 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

 

