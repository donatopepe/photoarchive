CREATE DATABASE PhotoCoilsDb
ON PRIMARY
(Name = PhotoArchiveCoilsDb,
FILENAME = 'C:\databases\PhotoArchiveCoilsDb\FTDB.mdf'),
FILEGROUP FTFG CONTAINS FILESTREAM
(NAME = PhotoArchiveCoilsDbFS,
FILENAME='C:\databases\PhotoArchiveCoilsDb\FS')
LOG ON
(Name = PhotoArchiveCoilsDbLog,
FILENAME = 'C:\databases\PhotoArchiveCoilsDb\FTDBLog.ldf')
WITH FILESTREAM (NON_TRANSACTED_ACCESS = FULL,
DIRECTORY_NAME = N'PhotoArchiveCoilsDb');
GO
