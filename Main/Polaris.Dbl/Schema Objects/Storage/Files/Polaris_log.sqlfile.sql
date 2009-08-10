ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [Polaris_log], FILENAME = 'D:\SQLServer\Data\Polaris_log.ldf', SIZE = 1024 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

