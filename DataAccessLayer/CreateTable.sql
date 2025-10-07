-- SQL скрипт для создания таблицы Monitors
-- Используется для Dapper репозитория

USE MonitorDb;
GO

-- Создание таблицы Monitors
CREATE TABLE Monitors (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Manufacturer NVARCHAR(100) NOT NULL,
    Model NVARCHAR(100) NOT NULL,
    SizeInInches FLOAT NOT NULL,
    Resolution NVARCHAR(50) NOT NULL,
    PanelType NVARCHAR(50) NOT NULL,
    PurchaseDate DATETIME2 NULL,
    WarrantyMonths INT NOT NULL,
    Note NVARCHAR(500) NULL
);
GO

-- Создание индексов для улучшения производительности
CREATE INDEX IX_Monitors_Manufacturer ON Monitors(Manufacturer);
CREATE INDEX IX_Monitors_Model ON Monitors(Model);
CREATE INDEX IX_Monitors_PurchaseDate ON Monitors(PurchaseDate);
GO

PRINT 'Таблица Monitors создана успешно';
