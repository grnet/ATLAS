ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [StudentPractice_log], FILENAME = '$(DefaultLogPath)$(DatabaseName)_1.ldf', MAXSIZE = 2097152 MB, FILEGROWTH = 102400 KB);

