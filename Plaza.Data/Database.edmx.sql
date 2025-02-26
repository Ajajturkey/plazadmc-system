
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/15/2019 07:36:07
-- Generated from EDMX file: C:\bitbacket\Plaza-Dmc\Eventor.Data\Database.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [LineDB2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_LanguageLocaleStringResource]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LocaleStringResources] DROP CONSTRAINT [FK_LanguageLocaleStringResource];
GO
IF OBJECT_ID(N'[dbo].[FK_VisitorVisitorTypes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VisitorTypes] DROP CONSTRAINT [FK_VisitorVisitorTypes];
GO
IF OBJECT_ID(N'[dbo].[FK_Reservations_Agency]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_Reservations_Agency];
GO
IF OBJECT_ID(N'[dbo].[FK_ExtraService_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExtraService] DROP CONSTRAINT [FK_ExtraService_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierExtraService]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExtraService] DROP CONSTRAINT [FK_SupplierExtraService];
GO
IF OBJECT_ID(N'[dbo].[FK_RHRoom_Rhotel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RHRoom] DROP CONSTRAINT [FK_RHRoom_Rhotel];
GO
IF OBJECT_ID(N'[dbo].[FK_Rhotel_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Rhotel] DROP CONSTRAINT [FK_Rhotel_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierRhotel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Rhotel] DROP CONSTRAINT [FK_SupplierRhotel];
GO
IF OBJECT_ID(N'[dbo].[FK_Tour_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tour] DROP CONSTRAINT [FK_Tour_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierTour]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tour] DROP CONSTRAINT [FK_SupplierTour];
GO
IF OBJECT_ID(N'[dbo].[FK_Transfers_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transfers] DROP CONSTRAINT [FK_Transfers_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierTransfers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transfers] DROP CONSTRAINT [FK_SupplierTransfers];
GO
IF OBJECT_ID(N'[dbo].[FK_ChangeRoom_hotelchange]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChangeRoom] DROP CONSTRAINT [FK_ChangeRoom_hotelchange];
GO
IF OBJECT_ID(N'[dbo].[FK_ininvoice_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ininvoice] DROP CONSTRAINT [FK_ininvoice_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_Payments_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payments] DROP CONSTRAINT [FK_Payments_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierSupplierServices]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierServicesSet] DROP CONSTRAINT [FK_SupplierSupplierServices];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierFlight]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Flights] DROP CONSTRAINT [FK_SupplierFlight];
GO
IF OBJECT_ID(N'[dbo].[FK_ReservationsFlight]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Flights] DROP CONSTRAINT [FK_ReservationsFlight];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierSupplierPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierPaymentSet] DROP CONSTRAINT [FK_SupplierSupplierPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_VisitorReservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_VisitorReservations];
GO
IF OBJECT_ID(N'[dbo].[FK_OfflineRooms_Hotel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OfflineRooms] DROP CONSTRAINT [FK_OfflineRooms_Hotel];
GO
IF OBJECT_ID(N'[dbo].[FK_OnlineRooms_Hotel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OnlineRooms] DROP CONSTRAINT [FK_OnlineRooms_Hotel];
GO
IF OBJECT_ID(N'[dbo].[FK_VisitorFilelog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Filelogs] DROP CONSTRAINT [FK_VisitorFilelog];
GO
IF OBJECT_ID(N'[dbo].[FK_ReservationsFilelog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Filelogs] DROP CONSTRAINT [FK_ReservationsFilelog];
GO
IF OBJECT_ID(N'[dbo].[FK_ReservationsUnpaid]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[unpaids] DROP CONSTRAINT [FK_ReservationsUnpaid];
GO
IF OBJECT_ID(N'[dbo].[FK_AgencyReport]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reports] DROP CONSTRAINT [FK_AgencyReport];
GO
IF OBJECT_ID(N'[dbo].[FK_AgencyUnpaid]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[unpaids] DROP CONSTRAINT [FK_AgencyUnpaid];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvancePaymentAgency]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdvancePayments] DROP CONSTRAINT [FK_AdvancePaymentAgency];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Languages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Languages];
GO
IF OBJECT_ID(N'[dbo].[LocaleStringResources]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LocaleStringResources];
GO
IF OBJECT_ID(N'[dbo].[Logs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Logs];
GO
IF OBJECT_ID(N'[dbo].[Members]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Members];
GO
IF OBJECT_ID(N'[dbo].[VisitorTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VisitorTypes];
GO
IF OBJECT_ID(N'[dbo].[PictureSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PictureSet];
GO
IF OBJECT_ID(N'[dbo].[SystemLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemLog];
GO
IF OBJECT_ID(N'[dbo].[Agency]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Agency];
GO
IF OBJECT_ID(N'[dbo].[ExtraService]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExtraService];
GO
IF OBJECT_ID(N'[dbo].[Rhotel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rhotel];
GO
IF OBJECT_ID(N'[dbo].[RHRoom]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RHRoom];
GO
IF OBJECT_ID(N'[dbo].[Tour]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tour];
GO
IF OBJECT_ID(N'[dbo].[Transfers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transfers];
GO
IF OBJECT_ID(N'[dbo].[DeadLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeadLine];
GO
IF OBJECT_ID(N'[dbo].[Marketman]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Marketman];
GO
IF OBJECT_ID(N'[dbo].[ChangeRoom]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChangeRoom];
GO
IF OBJECT_ID(N'[dbo].[hotelchange]', 'U') IS NOT NULL
    DROP TABLE [dbo].[hotelchange];
GO
IF OBJECT_ID(N'[dbo].[ininvoice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ininvoice];
GO
IF OBJECT_ID(N'[dbo].[Reservations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reservations];
GO
IF OBJECT_ID(N'[dbo].[Payments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payments];
GO
IF OBJECT_ID(N'[dbo].[Suppliers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Suppliers];
GO
IF OBJECT_ID(N'[dbo].[SupplierServicesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierServicesSet];
GO
IF OBJECT_ID(N'[dbo].[Flights]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Flights];
GO
IF OBJECT_ID(N'[dbo].[AdvancePayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdvancePayments];
GO
IF OBJECT_ID(N'[dbo].[SupplierPaymentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierPaymentSet];
GO
IF OBJECT_ID(N'[dbo].[DataTours]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DataTours];
GO
IF OBJECT_ID(N'[dbo].[DataPackages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DataPackages];
GO
IF OBJECT_ID(N'[dbo].[DataHotels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DataHotels];
GO
IF OBJECT_ID(N'[dbo].[OfflineRooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OfflineRooms];
GO
IF OBJECT_ID(N'[dbo].[OnlineRooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OnlineRooms];
GO
IF OBJECT_ID(N'[dbo].[Reports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reports];
GO
IF OBJECT_ID(N'[dbo].[unpaids]', 'U') IS NOT NULL
    DROP TABLE [dbo].[unpaids];
GO
IF OBJECT_ID(N'[dbo].[Filelogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Filelogs];
GO
IF OBJECT_ID(N'[dbo].[Settings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Settings];
GO
IF OBJECT_ID(N'[dbo].[HotelsReportSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HotelsReportSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Languages'
CREATE TABLE [dbo].[Languages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [LanguageCulture] nvarchar(max)  NULL,
    [UniqueSeoCode] nvarchar(max)  NULL,
    [FlagImageFileName] nvarchar(max)  NULL,
    [Rtl] bit  NOT NULL,
    [Published] bit  NOT NULL
);
GO

-- Creating table 'LocaleStringResources'
CREATE TABLE [dbo].[LocaleStringResources] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LanguageId] int  NOT NULL,
    [ResourceName] nvarchar(max)  NOT NULL,
    [ResourceValue] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Logs'
CREATE TABLE [dbo].[Logs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LogLevelId] nvarchar(max)  NOT NULL,
    [ShortMessage] nvarchar(max)  NULL,
    [FullMessage] nvarchar(max)  NULL,
    [PageUrl] nvarchar(max)  NOT NULL,
    [ReferrerUrl] nvarchar(max)  NOT NULL,
    [CreatedOnUtc] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Members'
CREATE TABLE [dbo].[Members] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IPaddress] nvarchar(max)  NULL,
    [lastActivityFromUtc] datetime  NOT NULL,
    [SenderEmail] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [languageId] int  NOT NULL,
    [Created_utc] datetime  NOT NULL,
    [Deleted] bit  NOT NULL,
    [VisitorGuid] uniqueidentifier  NULL,
    [Country] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [State] nvarchar(max)  NULL,
    [RecoveryToken] nvarchar(max)  NULL,
    [RecoveryDate] datetime  NULL,
    [active] bit  NOT NULL,
    [username] nvarchar(max)  NOT NULL,
    [password] nvarchar(max)  NOT NULL,
    [name] nvarchar(max)  NULL,
    [title] nvarchar(max)  NULL,
    [email] nvarchar(max)  NOT NULL,
    [manager] bit  NOT NULL,
    [accountManager] bit  NOT NULL,
    [reservationManager] bit  NOT NULL,
    [doreservations] bit  NOT NULL,
    [tel] nvarchar(max)  NULL,
    [emailPassword] nvarchar(max)  NULL
);
GO

-- Creating table 'VisitorTypes'
CREATE TABLE [dbo].[VisitorTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [VisitorId] int  NOT NULL
);
GO

-- Creating table 'PictureSet'
CREATE TABLE [dbo].[PictureSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [url] nvarchar(max)  NOT NULL,
    [data] nvarchar(max)  NOT NULL,
    [alt] nvarchar(max)  NOT NULL,
    [title] nvarchar(max)  NOT NULL,
    [thumbUrl] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SystemLog'
CREATE TABLE [dbo].[SystemLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EntityID] int  NOT NULL,
    [EntityName] varchar(max)  NULL,
    [Error] varchar(max)  NULL
);
GO

-- Creating table 'Agency'
CREATE TABLE [dbo].[Agency] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AgencyName] varchar(max)  NOT NULL,
    [Active] bit  NOT NULL,
    [Market] varchar(50)  NULL,
    [Country] varchar(50)  NOT NULL,
    [City] varchar(50)  NOT NULL,
    [Zip] nchar(10)  NULL,
    [Tel] varchar(50)  NULL,
    [Fax] varchar(50)  NULL,
    [Longitude] nvarchar(max)  NULL,
    [Latitude] nvarchar(max)  NULL,
    [Adress] nvarchar(max)  NOT NULL,
    [Commision] decimal(18,0)  NOT NULL,
    [Credit] nvarchar(max)  NOT NULL,
    [CreditAmount] nvarchar(max)  NOT NULL,
    [note] nvarchar(max)  NULL,
    [email] nvarchar(max)  NOT NULL,
    [password] nvarchar(max)  NOT NULL,
    [joindate] nvarchar(max)  NOT NULL,
    [username] nvarchar(max)  NULL,
    [contact] nvarchar(max)  NULL
);
GO

-- Creating table 'ExtraService'
CREATE TABLE [dbo].[ExtraService] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RID] int  NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [date] datetime  NOT NULL,
    [Cost] decimal(18,0)  NOT NULL,
    [Total] decimal(18,0)  NOT NULL,
    [customer] nvarchar(max)  NULL,
    [note] nvarchar(250)  NULL,
    [dateout] datetime  NOT NULL,
    [Pax] int  NULL,
    [lockbuying] bit  NOT NULL,
    [lockSelling] bit  NOT NULL,
    [ref_supplier] int  NOT NULL,
    [buyCom] decimal(18,0)  NOT NULL,
    [sellCom] decimal(18,0)  NOT NULL,
    [IsBuyRate] bit  NOT NULL,
    [IsSellRate] bit  NOT NULL,
    [SupplierCode] nvarchar(max)  NULL
);
GO

-- Creating table 'Rhotel'
CREATE TABLE [dbo].[Rhotel] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RID] int  NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [hid] int  NOT NULL,
    [checkout] datetime  NOT NULL,
    [checkin] datetime  NOT NULL,
    [Cost] decimal(18,0)  NOT NULL,
    [Total] decimal(18,0)  NOT NULL,
    [customer] nvarchar(max)  NOT NULL,
    [note] nvarchar(max)  NULL,
    [Confirmed] bit  NOT NULL,
    [SendHotel] bit  NOT NULL,
    [ref_supplier] int  NOT NULL,
    [buyCom] decimal(18,0)  NOT NULL,
    [sellCom] decimal(18,0)  NOT NULL,
    [IsBuyRate] bit  NOT NULL,
    [IsSellRate] bit  NOT NULL,
    [SupplierCode] nvarchar(max)  NULL
);
GO

-- Creating table 'RHRoom'
CREATE TABLE [dbo].[RHRoom] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HID] int  NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [type] nvarchar(100)  NOT NULL,
    [count] int  NOT NULL,
    [cost] decimal(18,0)  NOT NULL,
    [price] decimal(18,0)  NOT NULL,
    [board] nvarchar(50)  NULL,
    [guset] nvarchar(max)  NULL,
    [checkin] datetime  NULL,
    [checkout] datetime  NULL,
    [lockbuying] bit  NOT NULL,
    [lockSelling] bit  NOT NULL
);
GO

-- Creating table 'Tour'
CREATE TABLE [dbo].[Tour] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RID] int  NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [date] datetime  NOT NULL,
    [Cost] decimal(18,0)  NOT NULL,
    [Total] decimal(18,0)  NOT NULL,
    [customer] nvarchar(max)  NOT NULL,
    [note] nvarchar(max)  NULL,
    [Pax] int  NOT NULL,
    [pickuptime] nvarchar(100)  NULL,
    [pickup] nvarchar(100)  NULL,
    [dropoff] nvarchar(100)  NULL,
    [city] nvarchar(100)  NULL,
    [car] nvarchar(100)  NULL,
    [guide] nvarchar(max)  NULL,
    [lockbuying] bit  NOT NULL,
    [lockSelling] bit  NOT NULL,
    [ref_supplier] int  NOT NULL,
    [buyCom] decimal(18,0)  NOT NULL,
    [sellCom] decimal(18,0)  NOT NULL,
    [IsBuyRate] bit  NOT NULL,
    [IsSellRate] bit  NOT NULL,
    [SupplierCode] nvarchar(max)  NULL
);
GO

-- Creating table 'Transfers'
CREATE TABLE [dbo].[Transfers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RID] int  NOT NULL,
    [date] datetime  NOT NULL,
    [Cost] decimal(18,0)  NOT NULL,
    [Total] decimal(18,0)  NOT NULL,
    [customer] nvarchar(max)  NOT NULL,
    [note] nvarchar(max)  NULL,
    [pax] int  NOT NULL,
    [city] nvarchar(100)  NOT NULL,
    [pickup] nvarchar(100)  NULL,
    [dropoff] nvarchar(100)  NULL,
    [flightCode] nvarchar(100)  NULL,
    [flightTime] nvarchar(100)  NULL,
    [car] nvarchar(100)  NULL,
    [guide] nvarchar(max)  NULL,
    [lockbuying] bit  NOT NULL,
    [lockSelling] bit  NOT NULL,
    [ref_supplier] int  NOT NULL,
    [buyCom] decimal(18,0)  NOT NULL,
    [sellCom] decimal(18,0)  NOT NULL,
    [IsBuyRate] bit  NOT NULL,
    [IsSellRate] bit  NOT NULL,
    [SupplierCode] nvarchar(max)  NULL
);
GO

-- Creating table 'DeadLine'
CREATE TABLE [dbo].[DeadLine] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Ref_file] int  NOT NULL,
    [Expaire] datetime  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Category] nvarchar(max)  NULL
);
GO

-- Creating table 'Marketman'
CREATE TABLE [dbo].[Marketman] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Country] nvarchar(max)  NOT NULL,
    [City] nvarchar(max)  NOT NULL,
    [Gsm] nvarchar(max)  NOT NULL,
    [SalingCommsion] decimal(18,0)  NOT NULL,
    [BuyingCommsion] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'ChangeRoom'
CREATE TABLE [dbo].[ChangeRoom] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HID] int  NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [type] nvarchar(100)  NOT NULL,
    [count] int  NOT NULL,
    [cost] decimal(18,0)  NOT NULL,
    [price] decimal(18,0)  NOT NULL,
    [board] nchar(10)  NULL,
    [guset] nvarchar(max)  NULL,
    [checkin] datetime  NULL,
    [checkout] datetime  NULL
);
GO

-- Creating table 'hotelchange'
CREATE TABLE [dbo].[hotelchange] (
    [id] int IDENTITY(1,1) NOT NULL,
    [RID] int  NOT NULL,
    [name] nvarchar(150)  NOT NULL,
    [hid] int  NULL,
    [checkout] datetime  NOT NULL,
    [checkin] datetime  NOT NULL,
    [Cost] decimal(18,0)  NOT NULL,
    [Total] decimal(18,0)  NOT NULL,
    [customer] nvarchar(max)  NOT NULL,
    [note] nvarchar(250)  NULL
);
GO

-- Creating table 'ininvoice'
CREATE TABLE [dbo].[ininvoice] (
    [id] int IDENTITY(1,1) NOT NULL,
    [RID] int  NOT NULL,
    [no] nvarchar(100)  NOT NULL,
    [description] nvarchar(500)  NOT NULL,
    [date] datetime  NOT NULL,
    [curency] decimal(18,3)  NOT NULL,
    [price] decimal(18,2)  NOT NULL,
    [total] decimal(18,2)  NOT NULL,
    [customer] nvarchar(max)  NOT NULL,
    [url] nvarchar(max)  NULL,
    [currency] nvarchar(50)  NULL,
    [type] int  NULL,
    [typeid] int  NULL
);
GO

-- Creating table 'Reservations'
CREATE TABLE [dbo].[Reservations] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NULL,
    [ref_marketman] nvarchar(max)  NOT NULL,
    [ref_member] int  NOT NULL,
    [ref_agency] int  NULL,
    [status] nvarchar(50)  NULL,
    [date] datetime  NULL,
    [net] decimal(18,0)  NULL,
    [commision] decimal(18,0)  NULL,
    [discount] decimal(18,0)  NULL,
    [discountid] int  NULL,
    [cost] decimal(18,0)  NULL,
    [profit] decimal(18,0)  NULL,
    [price] decimal(18,0)  NOT NULL,
    [priceTL] decimal(18,0)  NULL,
    [costTL] decimal(18,0)  NULL,
    [curency] nchar(10)  NULL,
    [finishDate] datetime  NULL,
    [balanceDate] datetime  NOT NULL,
    [updatedate] datetime  NOT NULL,
    [updateuser] nvarchar(50)  NOT NULL,
    [reconfirmed] bit  NOT NULL,
    [vocher] nvarchar(150)  NULL,
    [sendmail] bit  NOT NULL,
    [note] nvarchar(max)  NULL,
    [agencystaff] nvarchar(50)  NULL,
    [balance] decimal(18,0)  NOT NULL,
    [checkindate] datetime  NOT NULL,
    [IsB2B] bit  NOT NULL,
    [Ref_CustomerId] int  NOT NULL
);
GO

-- Creating table 'Payments'
CREATE TABLE [dbo].[Payments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RID] int  NOT NULL,
    [date] datetime  NOT NULL,
    [type] varchar(50)  NOT NULL,
    [Payment] decimal(18,0)  NOT NULL,
    [note] varchar(max)  NULL,
    [Member] varchar(50)  NOT NULL,
    [Currency] varchar(10)  NOT NULL,
    [TotalPrice] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Suppliers'
CREATE TABLE [dbo].[Suppliers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [type] nvarchar(max)  NOT NULL,
    [SalingCommsion] decimal(18,0)  NOT NULL,
    [BuyingCommsion] decimal(18,0)  NOT NULL,
    [Active] bit  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [mobile] nvarchar(max)  NOT NULL,
    [tel] nvarchar(max)  NOT NULL,
    [fax] nvarchar(max)  NOT NULL,
    [contactPerson] nvarchar(max)  NOT NULL,
    [Country] nvarchar(max)  NOT NULL,
    [City] nvarchar(max)  NOT NULL,
    [JoinDate] datetime  NOT NULL,
    [IsBuyRate] bit  NOT NULL,
    [IsSellRate] bit  NOT NULL
);
GO

-- Creating table 'SupplierServicesSet'
CREATE TABLE [dbo].[SupplierServicesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ref_supplierkey] int  NOT NULL,
    [Servie] nvarchar(max)  NOT NULL,
    [descreption] nvarchar(max)  NOT NULL,
    [buy] decimal(18,0)  NOT NULL,
    [sell] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Flights'
CREATE TABLE [dbo].[Flights] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RID] int  NOT NULL,
    [name] nvarchar(100)  NOT NULL,
    [date] datetime  NOT NULL,
    [Cost] decimal(18,0)  NOT NULL,
    [Total] decimal(18,0)  NOT NULL,
    [customer] nvarchar(max)  NULL,
    [note] nvarchar(250)  NULL,
    [dateout] datetime  NOT NULL,
    [Pax] int  NULL,
    [lockbuying] bit  NOT NULL,
    [lockSelling] bit  NOT NULL,
    [ref_supplier] int  NOT NULL,
    [buyCom] decimal(18,0)  NOT NULL,
    [sellCom] decimal(18,0)  NOT NULL,
    [IsBuyRate] bit  NOT NULL,
    [IsSellRate] bit  NOT NULL,
    [SupplierCode] nvarchar(max)  NULL
);
GO

-- Creating table 'AdvancePayments'
CREATE TABLE [dbo].[AdvancePayments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ref_Agency] int  NOT NULL,
    [date] datetime  NOT NULL,
    [type] varchar(50)  NOT NULL,
    [Payment] decimal(18,0)  NOT NULL,
    [note] varchar(max)  NULL,
    [Member] varchar(50)  NOT NULL,
    [Currency] varchar(10)  NOT NULL,
    [TotalPrice] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'SupplierPaymentSet'
CREATE TABLE [dbo].[SupplierPaymentSet] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SID] int  NOT NULL,
    [date] datetime  NOT NULL,
    [type] varchar(50)  NOT NULL,
    [Payment] decimal(18,0)  NOT NULL,
    [note] varchar(max)  NULL,
    [Member] varchar(50)  NOT NULL,
    [Currency] varchar(10)  NOT NULL,
    [TotalPrice] decimal(18,0)  NOT NULL,
    [RID] int  NOT NULL,
    [TypeID] int  NOT NULL
);
GO

-- Creating table 'DataTours'
CREATE TABLE [dbo].[DataTours] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] varchar(250)  NOT NULL,
    [country] varchar(50)  NULL,
    [city] varchar(50)  NULL,
    [description] varchar(250)  NULL,
    [url] varchar(450)  NULL,
    [photourl] varchar(450)  NULL,
    [deleted] bit  NOT NULL
);
GO

-- Creating table 'DataPackages'
CREATE TABLE [dbo].[DataPackages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] varchar(250)  NOT NULL,
    [country] varchar(50)  NULL,
    [city] varchar(50)  NULL,
    [description] varchar(250)  NULL,
    [url] varchar(450)  NULL,
    [photourl] varchar(450)  NULL,
    [deleted] bit  NOT NULL
);
GO

-- Creating table 'DataHotels'
CREATE TABLE [dbo].[DataHotels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] varchar(150)  NULL,
    [city] varchar(150)  NULL,
    [country] varchar(150)  NULL,
    [zip] nchar(10)  NULL,
    [adress] varchar(250)  NULL,
    [rate] int  NULL,
    [minrate] decimal(18,0)  NULL,
    [maxrate] decimal(18,0)  NULL,
    [rank] int  NULL,
    [rooms] int  NULL,
    [map_longitude] varchar(150)  NULL,
    [map_latitude] varchar(150)  NULL,
    [url_booking] varchar(150)  NULL,
    [photo] varchar(150)  NULL,
    [desc] varchar(1000)  NULL,
    [desc_ar] varchar(1000)  NULL,
    [bookingID] int  NULL,
    [Email] nvarchar(150)  NULL,
    [Tel] nvarchar(150)  NULL,
    [Fax] nvarchar(150)  NULL,
    [rname] nchar(50)  NULL,
    [deleted] bit  NOT NULL
);
GO

-- Creating table 'OfflineRooms'
CREATE TABLE [dbo].[OfflineRooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [hid] int  NULL,
    [roomtype] varchar(250)  NULL,
    [board] nchar(10)  NULL,
    [market] nchar(10)  NULL,
    [validfrom] datetime  NULL,
    [validuntil] datetime  NULL,
    [room_ficilites] varchar(250)  NULL,
    [maxpax] int  NOT NULL,
    [bedprice] decimal(18,0)  NULL,
    [bedbreakfastprice] decimal(18,0)  NULL,
    [extrabed] decimal(18,0)  NULL,
    [note] varchar(max)  NULL,
    [name] nvarchar(150)  NULL
);
GO

-- Creating table 'OnlineRooms'
CREATE TABLE [dbo].[OnlineRooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [hid] int  NULL,
    [roomtype] varchar(250)  NULL,
    [board] nchar(10)  NULL,
    [market] nchar(10)  NULL,
    [validfrom] datetime  NULL,
    [validuntil] datetime  NULL,
    [room_ficilites] varchar(250)  NULL,
    [maxpax] int  NULL,
    [bedprice] decimal(18,0)  NULL,
    [bedbreakfastprice] decimal(18,0)  NULL,
    [extrabed] decimal(18,0)  NULL,
    [note] varchar(250)  NULL,
    [name] varchar(150)  NULL
);
GO

-- Creating table 'Reports'
CREATE TABLE [dbo].[Reports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ref_agency] int  NOT NULL,
    [Country] nvarchar(max)  NOT NULL,
    [Year] int  NOT NULL,
    [Jan] decimal(16,2)  NOT NULL,
    [Feb] decimal(16,2)  NOT NULL,
    [Mar] decimal(16,2)  NOT NULL,
    [Apr] decimal(16,2)  NOT NULL,
    [May] decimal(16,2)  NOT NULL,
    [Jun] decimal(16,2)  NOT NULL,
    [Jul] decimal(16,2)  NOT NULL,
    [Aug] decimal(16,2)  NOT NULL,
    [Sep] decimal(16,2)  NOT NULL,
    [Oct] decimal(16,2)  NOT NULL,
    [Nov] decimal(16,2)  NOT NULL,
    [Dec] decimal(16,2)  NOT NULL,
    [Total] decimal(16,2)  NOT NULL,
    [Costs] decimal(16,2)  NOT NULL,
    [JanCost] decimal(16,2)  NOT NULL,
    [FebCost] decimal(16,2)  NOT NULL,
    [MarCost] decimal(16,2)  NOT NULL,
    [AprCost] decimal(16,2)  NOT NULL,
    [MayCost] decimal(16,2)  NOT NULL,
    [JunCost] decimal(16,2)  NOT NULL,
    [JulCost] decimal(16,2)  NOT NULL,
    [AugCost] decimal(16,2)  NOT NULL,
    [SepCost] decimal(16,2)  NOT NULL,
    [OctCost] decimal(16,2)  NOT NULL,
    [NovCost] decimal(16,2)  NOT NULL,
    [DecCost] decimal(16,2)  NOT NULL
);
GO

-- Creating table 'unpaids'
CREATE TABLE [dbo].[unpaids] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ref_file] int  NOT NULL,
    [amount] decimal(16,2)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [CheckoutDate] datetime  NOT NULL,
    [Paid] bit  NOT NULL,
    [PaymentDate] nvarchar(max)  NOT NULL,
    [ref_agency] int  NOT NULL
);
GO

-- Creating table 'Filelogs'
CREATE TABLE [dbo].[Filelogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ref_file] int  NOT NULL,
    [ref_member] int  NOT NULL,
    [Action] nvarchar(max)  NOT NULL,
    [shortMessage] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Settings'
CREATE TABLE [dbo].[Settings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Logo] int  NOT NULL,
    [InvoiceHeader] int  NOT NULL,
    [InvoiceFooter] int  NOT NULL,
    [VoucherHeader] int  NOT NULL,
    [VoucherFooter] int  NOT NULL,
    [HotelRequest] int  NOT NULL,
    [HotelChange] int  NOT NULL,
    [HotelCancel] int  NOT NULL,
    [HotelFooter] nvarchar(max)  NOT NULL,
    [EmailSMTP] nvarchar(max)  NOT NULL,
    [EmailPort] nvarchar(max)  NOT NULL,
    [EmailUseSSL] nvarchar(max)  NOT NULL,
    [EmailUseCredentials] nvarchar(max)  NOT NULL,
    [EmailDefaultUser] nvarchar(max)  NOT NULL,
    [EmailDefaultPass] nvarchar(max)  NOT NULL,
    [AccountingName] nvarchar(max)  NOT NULL,
    [AccountingTitle] nvarchar(max)  NOT NULL,
    [BankName] nvarchar(max)  NOT NULL,
    [BankAccount] nvarchar(max)  NOT NULL,
    [BankSwift] nvarchar(max)  NOT NULL,
    [BackIBAN] nvarchar(max)  NOT NULL,
    [BankAccountNo] nvarchar(max)  NOT NULL,
    [BankAddress] nvarchar(max)  NOT NULL,
    [DefaultCurrency] int  NOT NULL
);
GO

-- Creating table 'HotelsReports'
CREATE TABLE [dbo].[HotelsReports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ref_agency] int  NOT NULL,
    [City] nvarchar(max)  NOT NULL,
    [Year] int  NOT NULL,
    [Jan] decimal(16,2)  NOT NULL,
    [Feb] decimal(16,2)  NOT NULL,
    [Mar] decimal(16,2)  NOT NULL,
    [Apr] decimal(16,2)  NOT NULL,
    [May] decimal(16,2)  NOT NULL,
    [Jun] decimal(16,2)  NOT NULL,
    [Jul] decimal(16,2)  NOT NULL,
    [Aug] decimal(16,2)  NOT NULL,
    [Sep] decimal(16,2)  NOT NULL,
    [Oct] decimal(16,2)  NOT NULL,
    [Nov] decimal(16,2)  NOT NULL,
    [Dec] decimal(16,2)  NOT NULL,
    [Total] decimal(16,2)  NOT NULL,
    [Costs] decimal(16,2)  NOT NULL,
    [ref_hotel] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Languages'
ALTER TABLE [dbo].[Languages]
ADD CONSTRAINT [PK_Languages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LocaleStringResources'
ALTER TABLE [dbo].[LocaleStringResources]
ADD CONSTRAINT [PK_LocaleStringResources]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Logs'
ALTER TABLE [dbo].[Logs]
ADD CONSTRAINT [PK_Logs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Members'
ALTER TABLE [dbo].[Members]
ADD CONSTRAINT [PK_Members]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VisitorTypes'
ALTER TABLE [dbo].[VisitorTypes]
ADD CONSTRAINT [PK_VisitorTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PictureSet'
ALTER TABLE [dbo].[PictureSet]
ADD CONSTRAINT [PK_PictureSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SystemLog'
ALTER TABLE [dbo].[SystemLog]
ADD CONSTRAINT [PK_SystemLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'Agency'
ALTER TABLE [dbo].[Agency]
ADD CONSTRAINT [PK_Agency]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ExtraService'
ALTER TABLE [dbo].[ExtraService]
ADD CONSTRAINT [PK_ExtraService]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Rhotel'
ALTER TABLE [dbo].[Rhotel]
ADD CONSTRAINT [PK_Rhotel]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RHRoom'
ALTER TABLE [dbo].[RHRoom]
ADD CONSTRAINT [PK_RHRoom]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Tour'
ALTER TABLE [dbo].[Tour]
ADD CONSTRAINT [PK_Tour]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Transfers'
ALTER TABLE [dbo].[Transfers]
ADD CONSTRAINT [PK_Transfers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [id] in table 'DeadLine'
ALTER TABLE [dbo].[DeadLine]
ADD CONSTRAINT [PK_DeadLine]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'Marketman'
ALTER TABLE [dbo].[Marketman]
ADD CONSTRAINT [PK_Marketman]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'ChangeRoom'
ALTER TABLE [dbo].[ChangeRoom]
ADD CONSTRAINT [PK_ChangeRoom]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [id] in table 'hotelchange'
ALTER TABLE [dbo].[hotelchange]
ADD CONSTRAINT [PK_hotelchange]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'ininvoice'
ALTER TABLE [dbo].[ininvoice]
ADD CONSTRAINT [PK_ininvoice]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [ID] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [PK_Reservations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [PK_Payments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [PK_Suppliers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierServicesSet'
ALTER TABLE [dbo].[SupplierServicesSet]
ADD CONSTRAINT [PK_SupplierServicesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Flights'
ALTER TABLE [dbo].[Flights]
ADD CONSTRAINT [PK_Flights]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'AdvancePayments'
ALTER TABLE [dbo].[AdvancePayments]
ADD CONSTRAINT [PK_AdvancePayments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SupplierPaymentSet'
ALTER TABLE [dbo].[SupplierPaymentSet]
ADD CONSTRAINT [PK_SupplierPaymentSet]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'DataTours'
ALTER TABLE [dbo].[DataTours]
ADD CONSTRAINT [PK_DataTours]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DataPackages'
ALTER TABLE [dbo].[DataPackages]
ADD CONSTRAINT [PK_DataPackages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DataHotels'
ALTER TABLE [dbo].[DataHotels]
ADD CONSTRAINT [PK_DataHotels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OfflineRooms'
ALTER TABLE [dbo].[OfflineRooms]
ADD CONSTRAINT [PK_OfflineRooms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OnlineRooms'
ALTER TABLE [dbo].[OnlineRooms]
ADD CONSTRAINT [PK_OnlineRooms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Reports'
ALTER TABLE [dbo].[Reports]
ADD CONSTRAINT [PK_Reports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'unpaids'
ALTER TABLE [dbo].[unpaids]
ADD CONSTRAINT [PK_unpaids]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Filelogs'
ALTER TABLE [dbo].[Filelogs]
ADD CONSTRAINT [PK_Filelogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Settings'
ALTER TABLE [dbo].[Settings]
ADD CONSTRAINT [PK_Settings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HotelsReports'
ALTER TABLE [dbo].[HotelsReports]
ADD CONSTRAINT [PK_HotelsReports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [LanguageId] in table 'LocaleStringResources'
ALTER TABLE [dbo].[LocaleStringResources]
ADD CONSTRAINT [FK_LanguageLocaleStringResource]
    FOREIGN KEY ([LanguageId])
    REFERENCES [dbo].[Languages]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LanguageLocaleStringResource'
CREATE INDEX [IX_FK_LanguageLocaleStringResource]
ON [dbo].[LocaleStringResources]
    ([LanguageId]);
GO

-- Creating foreign key on [VisitorId] in table 'VisitorTypes'
ALTER TABLE [dbo].[VisitorTypes]
ADD CONSTRAINT [FK_VisitorVisitorTypes]
    FOREIGN KEY ([VisitorId])
    REFERENCES [dbo].[Members]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitorVisitorTypes'
CREATE INDEX [IX_FK_VisitorVisitorTypes]
ON [dbo].[VisitorTypes]
    ([VisitorId]);
GO

-- Creating foreign key on [ref_agency] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [FK_Reservations_Agency]
    FOREIGN KEY ([ref_agency])
    REFERENCES [dbo].[Agency]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Reservations_Agency'
CREATE INDEX [IX_FK_Reservations_Agency]
ON [dbo].[Reservations]
    ([ref_agency]);
GO

-- Creating foreign key on [RID] in table 'ExtraService'
ALTER TABLE [dbo].[ExtraService]
ADD CONSTRAINT [FK_ExtraService_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExtraService_Reservations'
CREATE INDEX [IX_FK_ExtraService_Reservations]
ON [dbo].[ExtraService]
    ([RID]);
GO

-- Creating foreign key on [ref_supplier] in table 'ExtraService'
ALTER TABLE [dbo].[ExtraService]
ADD CONSTRAINT [FK_SupplierExtraService]
    FOREIGN KEY ([ref_supplier])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierExtraService'
CREATE INDEX [IX_FK_SupplierExtraService]
ON [dbo].[ExtraService]
    ([ref_supplier]);
GO

-- Creating foreign key on [HID] in table 'RHRoom'
ALTER TABLE [dbo].[RHRoom]
ADD CONSTRAINT [FK_RHRoom_Rhotel]
    FOREIGN KEY ([HID])
    REFERENCES [dbo].[Rhotel]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RHRoom_Rhotel'
CREATE INDEX [IX_FK_RHRoom_Rhotel]
ON [dbo].[RHRoom]
    ([HID]);
GO

-- Creating foreign key on [RID] in table 'Rhotel'
ALTER TABLE [dbo].[Rhotel]
ADD CONSTRAINT [FK_Rhotel_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Rhotel_Reservations'
CREATE INDEX [IX_FK_Rhotel_Reservations]
ON [dbo].[Rhotel]
    ([RID]);
GO

-- Creating foreign key on [ref_supplier] in table 'Rhotel'
ALTER TABLE [dbo].[Rhotel]
ADD CONSTRAINT [FK_SupplierRhotel]
    FOREIGN KEY ([ref_supplier])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierRhotel'
CREATE INDEX [IX_FK_SupplierRhotel]
ON [dbo].[Rhotel]
    ([ref_supplier]);
GO

-- Creating foreign key on [RID] in table 'Tour'
ALTER TABLE [dbo].[Tour]
ADD CONSTRAINT [FK_Tour_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Tour_Reservations'
CREATE INDEX [IX_FK_Tour_Reservations]
ON [dbo].[Tour]
    ([RID]);
GO

-- Creating foreign key on [ref_supplier] in table 'Tour'
ALTER TABLE [dbo].[Tour]
ADD CONSTRAINT [FK_SupplierTour]
    FOREIGN KEY ([ref_supplier])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierTour'
CREATE INDEX [IX_FK_SupplierTour]
ON [dbo].[Tour]
    ([ref_supplier]);
GO

-- Creating foreign key on [RID] in table 'Transfers'
ALTER TABLE [dbo].[Transfers]
ADD CONSTRAINT [FK_Transfers_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Transfers_Reservations'
CREATE INDEX [IX_FK_Transfers_Reservations]
ON [dbo].[Transfers]
    ([RID]);
GO

-- Creating foreign key on [ref_supplier] in table 'Transfers'
ALTER TABLE [dbo].[Transfers]
ADD CONSTRAINT [FK_SupplierTransfers]
    FOREIGN KEY ([ref_supplier])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierTransfers'
CREATE INDEX [IX_FK_SupplierTransfers]
ON [dbo].[Transfers]
    ([ref_supplier]);
GO

-- Creating foreign key on [HID] in table 'ChangeRoom'
ALTER TABLE [dbo].[ChangeRoom]
ADD CONSTRAINT [FK_ChangeRoom_hotelchange]
    FOREIGN KEY ([HID])
    REFERENCES [dbo].[hotelchange]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChangeRoom_hotelchange'
CREATE INDEX [IX_FK_ChangeRoom_hotelchange]
ON [dbo].[ChangeRoom]
    ([HID]);
GO

-- Creating foreign key on [RID] in table 'ininvoice'
ALTER TABLE [dbo].[ininvoice]
ADD CONSTRAINT [FK_ininvoice_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ininvoice_Reservations'
CREATE INDEX [IX_FK_ininvoice_Reservations]
ON [dbo].[ininvoice]
    ([RID]);
GO

-- Creating foreign key on [RID] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [FK_Payments_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Payments_Reservations'
CREATE INDEX [IX_FK_Payments_Reservations]
ON [dbo].[Payments]
    ([RID]);
GO

-- Creating foreign key on [ref_supplierkey] in table 'SupplierServicesSet'
ALTER TABLE [dbo].[SupplierServicesSet]
ADD CONSTRAINT [FK_SupplierSupplierServices]
    FOREIGN KEY ([ref_supplierkey])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierServices'
CREATE INDEX [IX_FK_SupplierSupplierServices]
ON [dbo].[SupplierServicesSet]
    ([ref_supplierkey]);
GO

-- Creating foreign key on [ref_supplier] in table 'Flights'
ALTER TABLE [dbo].[Flights]
ADD CONSTRAINT [FK_SupplierFlight]
    FOREIGN KEY ([ref_supplier])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierFlight'
CREATE INDEX [IX_FK_SupplierFlight]
ON [dbo].[Flights]
    ([ref_supplier]);
GO

-- Creating foreign key on [RID] in table 'Flights'
ALTER TABLE [dbo].[Flights]
ADD CONSTRAINT [FK_ReservationsFlight]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservationsFlight'
CREATE INDEX [IX_FK_ReservationsFlight]
ON [dbo].[Flights]
    ([RID]);
GO

-- Creating foreign key on [SID] in table 'SupplierPaymentSet'
ALTER TABLE [dbo].[SupplierPaymentSet]
ADD CONSTRAINT [FK_SupplierSupplierPayment]
    FOREIGN KEY ([SID])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierPayment'
CREATE INDEX [IX_FK_SupplierSupplierPayment]
ON [dbo].[SupplierPaymentSet]
    ([SID]);
GO

-- Creating foreign key on [ref_member] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [FK_VisitorReservations]
    FOREIGN KEY ([ref_member])
    REFERENCES [dbo].[Members]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitorReservations'
CREATE INDEX [IX_FK_VisitorReservations]
ON [dbo].[Reservations]
    ([ref_member]);
GO

-- Creating foreign key on [hid] in table 'OfflineRooms'
ALTER TABLE [dbo].[OfflineRooms]
ADD CONSTRAINT [FK_OfflineRooms_Hotel]
    FOREIGN KEY ([hid])
    REFERENCES [dbo].[DataHotels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OfflineRooms_Hotel'
CREATE INDEX [IX_FK_OfflineRooms_Hotel]
ON [dbo].[OfflineRooms]
    ([hid]);
GO

-- Creating foreign key on [hid] in table 'OnlineRooms'
ALTER TABLE [dbo].[OnlineRooms]
ADD CONSTRAINT [FK_OnlineRooms_Hotel]
    FOREIGN KEY ([hid])
    REFERENCES [dbo].[DataHotels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OnlineRooms_Hotel'
CREATE INDEX [IX_FK_OnlineRooms_Hotel]
ON [dbo].[OnlineRooms]
    ([hid]);
GO

-- Creating foreign key on [ref_member] in table 'Filelogs'
ALTER TABLE [dbo].[Filelogs]
ADD CONSTRAINT [FK_VisitorFilelog]
    FOREIGN KEY ([ref_member])
    REFERENCES [dbo].[Members]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitorFilelog'
CREATE INDEX [IX_FK_VisitorFilelog]
ON [dbo].[Filelogs]
    ([ref_member]);
GO

-- Creating foreign key on [ref_file] in table 'Filelogs'
ALTER TABLE [dbo].[Filelogs]
ADD CONSTRAINT [FK_ReservationsFilelog]
    FOREIGN KEY ([ref_file])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservationsFilelog'
CREATE INDEX [IX_FK_ReservationsFilelog]
ON [dbo].[Filelogs]
    ([ref_file]);
GO

-- Creating foreign key on [ref_file] in table 'unpaids'
ALTER TABLE [dbo].[unpaids]
ADD CONSTRAINT [FK_ReservationsUnpaid]
    FOREIGN KEY ([ref_file])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservationsUnpaid'
CREATE INDEX [IX_FK_ReservationsUnpaid]
ON [dbo].[unpaids]
    ([ref_file]);
GO

-- Creating foreign key on [ref_agency] in table 'Reports'
ALTER TABLE [dbo].[Reports]
ADD CONSTRAINT [FK_AgencyReport]
    FOREIGN KEY ([ref_agency])
    REFERENCES [dbo].[Agency]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgencyReport'
CREATE INDEX [IX_FK_AgencyReport]
ON [dbo].[Reports]
    ([ref_agency]);
GO

-- Creating foreign key on [ref_agency] in table 'unpaids'
ALTER TABLE [dbo].[unpaids]
ADD CONSTRAINT [FK_AgencyUnpaid]
    FOREIGN KEY ([ref_agency])
    REFERENCES [dbo].[Agency]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgencyUnpaid'
CREATE INDEX [IX_FK_AgencyUnpaid]
ON [dbo].[unpaids]
    ([ref_agency]);
GO

-- Creating foreign key on [ref_Agency] in table 'AdvancePayments'
ALTER TABLE [dbo].[AdvancePayments]
ADD CONSTRAINT [FK_AdvancePaymentAgency]
    FOREIGN KEY ([ref_Agency])
    REFERENCES [dbo].[Agency]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvancePaymentAgency'
CREATE INDEX [IX_FK_AdvancePaymentAgency]
ON [dbo].[AdvancePayments]
    ([ref_Agency]);
GO

-- Creating foreign key on [ref_hotel] in table 'HotelsReports'
ALTER TABLE [dbo].[HotelsReports]
ADD CONSTRAINT [FK_DataHotelHotelsReport]
    FOREIGN KEY ([ref_hotel])
    REFERENCES [dbo].[DataHotels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DataHotelHotelsReport'
CREATE INDEX [IX_FK_DataHotelHotelsReport]
ON [dbo].[HotelsReports]
    ([ref_hotel]);
GO

-- Creating foreign key on [hid] in table 'Rhotel'
ALTER TABLE [dbo].[Rhotel]
ADD CONSTRAINT [FK_DataHotelRhotel]
    FOREIGN KEY ([hid])
    REFERENCES [dbo].[DataHotels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DataHotelRhotel'
CREATE INDEX [IX_FK_DataHotelRhotel]
ON [dbo].[Rhotel]
    ([hid]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------