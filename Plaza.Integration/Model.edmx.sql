
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/09/2019 19:22:13
-- Generated from EDMX file: C:\bitbacket\Plaza-Dmc\Line.Integration\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Livedata];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Agents_Agency]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agents] DROP CONSTRAINT [FK_Agents_Agency];
GO
IF OBJECT_ID(N'[dbo].[FK_ChangeRoom_hotelchange]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChangeRoom] DROP CONSTRAINT [FK_ChangeRoom_hotelchange];
GO
IF OBJECT_ID(N'[dbo].[FK_ExtraService_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExtraService] DROP CONSTRAINT [FK_ExtraService_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_hotelchange_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[hotelchange] DROP CONSTRAINT [FK_hotelchange_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_ininvoice_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ininvoice] DROP CONSTRAINT [FK_ininvoice_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_Payments_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payments] DROP CONSTRAINT [FK_Payments_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_Reservations_Agency]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_Reservations_Agency];
GO
IF OBJECT_ID(N'[dbo].[FK_Reservations_Members]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_Reservations_Members];
GO
IF OBJECT_ID(N'[dbo].[FK_Rhotel_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Rhotel] DROP CONSTRAINT [FK_Rhotel_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_RHRoom_Rhotel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RHRoom] DROP CONSTRAINT [FK_RHRoom_Rhotel];
GO
IF OBJECT_ID(N'[dbo].[FK_Tour_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tour] DROP CONSTRAINT [FK_Tour_Reservations];
GO
IF OBJECT_ID(N'[dbo].[FK_Transfers_Reservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transfers] DROP CONSTRAINT [FK_Transfers_Reservations];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Agency]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Agency];
GO
IF OBJECT_ID(N'[dbo].[Agents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Agents];
GO
IF OBJECT_ID(N'[dbo].[ChangeRoom]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChangeRoom];
GO
IF OBJECT_ID(N'[dbo].[Coupun]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Coupun];
GO
IF OBJECT_ID(N'[dbo].[DeadLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeadLine];
GO
IF OBJECT_ID(N'[dbo].[ExtraService]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExtraService];
GO
IF OBJECT_ID(N'[dbo].[hotelchange]', 'U') IS NOT NULL
    DROP TABLE [dbo].[hotelchange];
GO
IF OBJECT_ID(N'[dbo].[ininvoice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ininvoice];
GO
IF OBJECT_ID(N'[dbo].[Marketman]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Marketman];
GO
IF OBJECT_ID(N'[dbo].[Members]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Members];
GO
IF OBJECT_ID(N'[dbo].[Payments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payments];
GO
IF OBJECT_ID(N'[dbo].[Reservations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reservations];
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
IF OBJECT_ID(N'[ReservationsModelStoreContainer].[old]', 'U') IS NOT NULL
    DROP TABLE [ReservationsModelStoreContainer].[old];
GO
IF OBJECT_ID(N'[ReservationsModelStoreContainer].[tahsilat]', 'U') IS NOT NULL
    DROP TABLE [ReservationsModelStoreContainer].[tahsilat];
GO
IF OBJECT_ID(N'[ReservationsModelStoreContainer].[uyeler]', 'U') IS NOT NULL
    DROP TABLE [ReservationsModelStoreContainer].[uyeler];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

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
    [Longitude] nvarchar(max)  NOT NULL,
    [Latitude] nvarchar(max)  NOT NULL,
    [Adress] nvarchar(max)  NOT NULL,
    [Commision] decimal(18,0)  NOT NULL,
    [Credit] nvarchar(max)  NOT NULL,
    [CreditAmount] nvarchar(max)  NOT NULL,
    [note] nvarchar(max)  NULL,
    [email] nvarchar(max)  NOT NULL,
    [password] nvarchar(max)  NOT NULL,
    [joindate] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Agents'
CREATE TABLE [dbo].[Agents] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AID] int  NOT NULL,
    [active] bit  NOT NULL,
    [date] datetime  NOT NULL,
    [name] nvarchar(50)  NOT NULL,
    [mobile] nvarchar(50)  NULL,
    [email] nvarchar(50)  NULL,
    [skype] nvarchar(50)  NULL,
    [other] nvarchar(150)  NULL
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
    [Pax] int  NULL
);
GO

-- Creating table 'Rhotel'
CREATE TABLE [dbo].[Rhotel] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RID] int  NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [hid] int  NULL,
    [checkout] datetime  NOT NULL,
    [checkin] datetime  NOT NULL,
    [Cost] decimal(18,0)  NOT NULL,
    [Total] decimal(18,0)  NOT NULL,
    [customer] nvarchar(max)  NOT NULL,
    [note] nvarchar(max)  NULL,
    [Confirmed] bit  NOT NULL,
    [SendHotel] bit  NOT NULL
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
    [checkout] datetime  NULL
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
    [guide] nvarchar(max)  NULL
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
    [guide] nvarchar(max)  NULL
);
GO

-- Creating table 'Members'
CREATE TABLE [dbo].[Members] (
    [ID] int IDENTITY(1,1) NOT NULL,
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

-- Creating table 'Coupun'
CREATE TABLE [dbo].[Coupun] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Total] nvarchar(max)  NOT NULL,
    [Rate] nvarchar(max)  NOT NULL,
    [ExpireDate] nvarchar(max)  NOT NULL
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
    [Gsm] nvarchar(max)  NOT NULL
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
    [checkindate] datetime  NOT NULL
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

-- Creating table 'uyeler'
CREATE TABLE [dbo].[uyeler] (
    [ID] int  NOT NULL,
    [kulladi] nvarchar(255)  NULL,
    [sifre] nvarchar(255)  NULL,
    [onay] bit  NOT NULL,
    [market] nvarchar(255)  NULL,
    [ulke] nvarchar(255)  NULL,
    [email] nvarchar(255)  NULL,
    [indirim] nvarchar(255)  NULL,
    [kredi] nvarchar(255)  NULL,
    [komisyon] nvarchar(255)  NULL,
    [adi] nvarchar(255)  NULL,
    [agent_type] nvarchar(255)  NULL,
    [soyadi] nvarchar(255)  NULL,
    [acente_code] nvarchar(255)  NULL,
    [firma] nvarchar(255)  NULL,
    [fax] nvarchar(255)  NULL,
    [tel] nvarchar(255)  NULL,
    [sehir] nvarchar(255)  NULL,
    [uyelik_tarihi] datetime  NULL,
    [son_giris_tarihi] datetime  NULL,
    [giris_sayisi] float  NULL,
    [uye_ip] nvarchar(255)  NULL,
    [cevap] nvarchar(255)  NULL,
    [sifresoru] nvarchar(255)  NULL,
    [adres] nvarchar(255)  NULL,
    [IP] nvarchar(255)  NULL,
    [gecici_kredi] bit  NOT NULL,
    [cperson] nvarchar(50)  NULL,
    [remarks] nvarchar(max)  NULL,
    [czipcode] nvarchar(50)  NULL,
    [iata] nvarchar(50)  NULL,
    [iperson] nvarchar(50)  NULL,
    [iemail] nvarchar(50)  NULL,
    [iadres] nvarchar(50)  NULL,
    [isehir] nvarchar(50)  NULL,
    [izipcode] nvarchar(50)  NULL,
    [iulke] nvarchar(50)  NULL,
    [itel] nvarchar(50)  NULL,
    [ifax] nvarchar(50)  NULL,
    [logo] nvarchar(max)  NULL,
    [ceptel] nvarchar(50)  NULL,
    [iceptel] nvarchar(50)  NULL,
    [subagent] bit  NOT NULL
);
GO

-- Creating table 'old'
CREATE TABLE [dbo].[old] (
    [res_id] int  NOT NULL,
    [market] nvarchar(50)  NULL,
    [Hoteladi] nvarchar(50)  NULL,
    [Hotelkod] nvarchar(50)  NULL,
    [emailsay] int  NULL,
    [belgesay] int  NULL,
    [email] nvarchar(50)  NULL,
    [aciklama] nvarchar(max)  NULL,
    [dosyano] nvarchar(50)  NULL,
    [Rezervlist] nvarchar(50)  NULL,
    [Rezervkod] nvarchar(50)  NULL,
    [Rezervkodmu] uniqueidentifier  NULL,
    [Checkindate] nvarchar(50)  NULL,
    [Checkoutdate] nvarchar(50)  NULL,
    [Totalnights] nvarchar(50)  NULL,
    [Board] nvarchar(50)  NULL,
    [OdaTipi] nvarchar(50)  NULL,
    [Sngfiyat] nvarchar(50)  NULL,
    [Sngoda] nvarchar(50)  NULL,
    [Sngmaliyet] nvarchar(50)  NULL,
    [Sngtotal] nvarchar(50)  NULL,
    [Doublefiyat] nvarchar(50)  NULL,
    [Doubleoda] nvarchar(50)  NULL,
    [Doublemaliyet] nvarchar(50)  NULL,
    [Doubletotal] nvarchar(50)  NULL,
    [Exbedfiyat] nvarchar(50)  NULL,
    [Exbedadet] nvarchar(50)  NULL,
    [Exbedtotal] nvarchar(50)  NULL,
    [Chdfreeadet] nvarchar(50)  NULL,
    [Chd50fiyat] nvarchar(50)  NULL,
    [Chd50adet] nvarchar(50)  NULL,
    [Chd50total] nvarchar(50)  NULL,
    [Rezervtotal] nvarchar(50)  NULL,
    [hotelmaliyet] nvarchar(50)  NULL,
    [hoteltotal] float  NULL,
    [maliyettotal] nvarchar(50)  NULL,
    [Grandtotal] nvarchar(50)  NULL,
    [Komisyontotal] nvarchar(50)  NULL,
    [netpayment] nvarchar(50)  NULL,
    [Info] nvarchar(255)  NULL,
    [Turadi] nvarchar(50)  NULL,
    [Turpaxcins] nvarchar(50)  NULL,
    [Turprice] nvarchar(50)  NULL,
    [Turadet] nvarchar(50)  NULL,
    [Turtotal] float  NULL,
    [Turmaliyet] nvarchar(50)  NULL,
    [fullturtotal] float  NULL,
    [Turtarih] nvarchar(50)  NULL,
    [TurRegion] nvarchar(50)  NULL,
    [Transferadi] nvarchar(255)  NULL,
    [Transferpaxcins] nvarchar(50)  NULL,
    [Transferadet] nvarchar(50)  NULL,
    [Transferprice] nvarchar(50)  NULL,
    [Transfermaliyet] nvarchar(50)  NULL,
    [Transfertotal] int  NULL,
    [fulltransfertotal] int  NULL,
    [Transfertarih] nvarchar(50)  NULL,
    [TransferRegion] nvarchar(50)  NULL,
    [Transferflightno] nvarchar(50)  NULL,
    [Transferflighttime] nvarchar(50)  NULL,
    [komisyon] nvarchar(50)  NULL,
    [kulladi] nvarchar(50)  NULL,
    [reconfirm] bit  NOT NULL,
    [confirm] bit  NOT NULL,
    [cancel] bit  NOT NULL,
    [onay] bit  NOT NULL,
    [degisiklik] bit  NOT NULL,
    [payment] bit  NOT NULL,
    [otel] bit  NOT NULL,
    [otel_bilgi] nvarchar(50)  NULL,
    [notlar] nvarchar(max)  NULL,
    [otel_tarih] nvarchar(50)  NULL,
    [yedek] nvarchar(50)  NULL,
    [rezervdurum] nvarchar(50)  NULL,
    [yedeksayi] int  NULL,
    [partlyconfirm] bit  NOT NULL,
    [tamamdir] bit  NOT NULL,
    [odeme] nvarchar(50)  NULL,
    [belge] bit  NOT NULL,
    [iptal_belge] bit  NOT NULL,
    [degisiklik_belge] bit  NOT NULL,
    [birim] nvarchar(50)  NULL,
    [mudahale] nvarchar(50)  NULL,
    [mudahale_tarih] datetime  NULL,
    [bizimkiler] nvarchar(50)  NULL,
    [tarih] datetime  NULL,
    [ilktarih] datetime  NULL,
    [Rezervkodu] int  NULL,
    [iptal_tarih] datetime  NULL,
    [degisiklikfark] int  NULL,
    [kasa] int  NULL,
    [kasahareketi] bit  NOT NULL,
    [silinecek] bit  NOT NULL,
    [online] bit  NOT NULL,
    [onlineiptal] bit  NOT NULL,
    [provider] nvarchar(50)  NULL,
    [onlineFilename] nvarchar(150)  NULL,
    [onlineiptaltarih] datetime  NULL,
    [cancelationtext] nvarchar(50)  NULL,
    [Triplefiyat] nvarchar(50)  NULL,
    [Tripleadet] nvarchar(max)  NULL,
    [Tripletotal] nvarchar(50)  NULL,
    [SENDMAIL] nvarchar(255)  NULL,
    [SENDPDF] bit  NOT NULL,
    [PDFFILE] nvarchar(255)  NULL
);
GO

-- Creating table 'tahsilat'
CREATE TABLE [dbo].[tahsilat] (
    [id] int  NOT NULL,
    [kulladi] nvarchar(50)  NULL,
    [tarih] datetime  NULL,
    [toplam] float  NULL,
    [staff] nvarchar(50)  NULL,
    [odeme_tipi] nvarchar(50)  NULL,
    [orderid] nvarchar(100)  NULL,
    [odeme_detay] nvarchar(30)  NULL,
    [odeme_tarihi] datetime  NULL,
    [ilgilidosya] nvarchar(30)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Agency'
ALTER TABLE [dbo].[Agency]
ADD CONSTRAINT [PK_Agency]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Agents'
ALTER TABLE [dbo].[Agents]
ADD CONSTRAINT [PK_Agents]
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

-- Creating primary key on [ID] in table 'Members'
ALTER TABLE [dbo].[Members]
ADD CONSTRAINT [PK_Members]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'Coupun'
ALTER TABLE [dbo].[Coupun]
ADD CONSTRAINT [PK_Coupun]
    PRIMARY KEY CLUSTERED ([Id] ASC);
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

-- Creating primary key on [ID], [onay], [gecici_kredi], [subagent] in table 'uyeler'
ALTER TABLE [dbo].[uyeler]
ADD CONSTRAINT [PK_uyeler]
    PRIMARY KEY CLUSTERED ([ID], [onay], [gecici_kredi], [subagent] ASC);
GO

-- Creating primary key on [res_id], [reconfirm], [confirm], [cancel], [onay], [degisiklik], [payment], [otel], [partlyconfirm], [tamamdir], [belge], [iptal_belge], [degisiklik_belge], [kasahareketi], [silinecek], [online], [onlineiptal], [SENDPDF] in table 'old'
ALTER TABLE [dbo].[old]
ADD CONSTRAINT [PK_old]
    PRIMARY KEY CLUSTERED ([res_id], [reconfirm], [confirm], [cancel], [onay], [degisiklik], [payment], [otel], [partlyconfirm], [tamamdir], [belge], [iptal_belge], [degisiklik_belge], [kasahareketi], [silinecek], [online], [onlineiptal], [SENDPDF] ASC);
GO

-- Creating primary key on [id] in table 'tahsilat'
ALTER TABLE [dbo].[tahsilat]
ADD CONSTRAINT [PK_tahsilat]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AID] in table 'Agents'
ALTER TABLE [dbo].[Agents]
ADD CONSTRAINT [FK_Agents_Agency]
    FOREIGN KEY ([AID])
    REFERENCES [dbo].[Agency]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Agents_Agency'
CREATE INDEX [IX_FK_Agents_Agency]
ON [dbo].[Agents]
    ([AID]);
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
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExtraService_Reservations'
CREATE INDEX [IX_FK_ExtraService_Reservations]
ON [dbo].[ExtraService]
    ([RID]);
GO

-- Creating foreign key on [RID] in table 'hotelchange'
ALTER TABLE [dbo].[hotelchange]
ADD CONSTRAINT [FK_hotelchange_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_hotelchange_Reservations'
CREATE INDEX [IX_FK_hotelchange_Reservations]
ON [dbo].[hotelchange]
    ([RID]);
GO

-- Creating foreign key on [RID] in table 'ininvoice'
ALTER TABLE [dbo].[ininvoice]
ADD CONSTRAINT [FK_ininvoice_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ininvoice_Reservations'
CREATE INDEX [IX_FK_ininvoice_Reservations]
ON [dbo].[ininvoice]
    ([RID]);
GO

-- Creating foreign key on [ref_member] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [FK_Reservations_Members]
    FOREIGN KEY ([ref_member])
    REFERENCES [dbo].[Members]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Reservations_Members'
CREATE INDEX [IX_FK_Reservations_Members]
ON [dbo].[Reservations]
    ([ref_member]);
GO

-- Creating foreign key on [RID] in table 'Rhotel'
ALTER TABLE [dbo].[Rhotel]
ADD CONSTRAINT [FK_Rhotel_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Rhotel_Reservations'
CREATE INDEX [IX_FK_Rhotel_Reservations]
ON [dbo].[Rhotel]
    ([RID]);
GO

-- Creating foreign key on [RID] in table 'Tour'
ALTER TABLE [dbo].[Tour]
ADD CONSTRAINT [FK_Tour_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Tour_Reservations'
CREATE INDEX [IX_FK_Tour_Reservations]
ON [dbo].[Tour]
    ([RID]);
GO

-- Creating foreign key on [RID] in table 'Transfers'
ALTER TABLE [dbo].[Transfers]
ADD CONSTRAINT [FK_Transfers_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Transfers_Reservations'
CREATE INDEX [IX_FK_Transfers_Reservations]
ON [dbo].[Transfers]
    ([RID]);
GO

-- Creating foreign key on [RID] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [FK_Payments_Reservations]
    FOREIGN KEY ([RID])
    REFERENCES [dbo].[Reservations]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Payments_Reservations'
CREATE INDEX [IX_FK_Payments_Reservations]
ON [dbo].[Payments]
    ([RID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------