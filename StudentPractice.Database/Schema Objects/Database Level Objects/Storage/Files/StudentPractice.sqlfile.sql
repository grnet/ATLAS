ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [StudentPractice], FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

