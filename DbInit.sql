CREATE DATABASE [StorageTest]
 CONTAINMENT = NONE
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StorageTest].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [StorageTest] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [StorageTest] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [StorageTest] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [StorageTest] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [StorageTest] SET ARITHABORT OFF 
GO

ALTER DATABASE [StorageTest] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [StorageTest] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [StorageTest] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [StorageTest] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [StorageTest] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [StorageTest] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [StorageTest] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [StorageTest] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [StorageTest] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [StorageTest] SET  ENABLE_BROKER 
GO

ALTER DATABASE [StorageTest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [StorageTest] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [StorageTest] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [StorageTest] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [StorageTest] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [StorageTest] SET READ_COMMITTED_SNAPSHOT ON 
GO

ALTER DATABASE [StorageTest] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [StorageTest] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [StorageTest] SET  MULTI_USER 
GO

ALTER DATABASE [StorageTest] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [StorageTest] SET DB_CHAINING OFF 
GO

ALTER DATABASE [StorageTest] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [StorageTest] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [StorageTest] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [StorageTest] SET QUERY_STORE = OFF
GO

USE [StorageTest]
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE [StorageTest] SET  READ_WRITE 
GO

-- tables

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- [Driver]
	CREATE TABLE [dbo].[Driver](
		[Id] [uniqueidentifier] NOT NULL,
		[Experience] [float] NOT NULL,
		[Rating] [float] NOT NULL,
		[IsSmoker] [bit] NOT NULL,
		[IsTalkative] [bit] NOT NULL,
		[IsMusicLover] [bit] NOT NULL,
		[CreatedOn] [datetime2](7) NOT NULL,
		[ModifiedOn] [datetime2](7) NULL,
		[FirstName] [nvarchar](max) NULL,
		[LastName] [nvarchar](max) NULL,
		[MiddleName] [nvarchar](max) NULL,
		[BirthDate] [datetime2](7) NOT NULL,
	 CONSTRAINT [PK_Driver] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	GO

	CREATE TRIGGER [dbo].[Driver_Update]
	   ON [dbo].[Driver]
	   AFTER UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;

		IF NOT UPDATE([ModifiedOn])
			BEGIN
				UPDATE [dbo].[Driver]
				SET [ModifiedOn] = GETDATE()
				WHERE [Id] = (SELECT [Id] from [inserted])
			END
	END
	GO

-- [CarType]
	CREATE TABLE [dbo].[CarType](
		[Id] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](max) NULL,
		[CreatedOn] [datetime2](7) NOT NULL,
		[ModifiedOn] [datetime2](7) NULL,
	 CONSTRAINT [PK_CarType] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	GO

	CREATE TRIGGER [dbo].[CarType_Update]
	   ON [dbo].[CarType]
	   AFTER UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;

		IF NOT UPDATE([ModifiedOn])
			BEGIN
				UPDATE [dbo].[CarType]
				SET [ModifiedOn] = GETDATE()
				WHERE [Id] = (SELECT [Id] from [inserted])
			END
	END
	GO

	INSERT INTO [dbo].[CarType]
			   ([Id]
			   ,[Name]
			   ,[CreatedOn])
		 VALUES
			   (NEWID()
			   ,'Business class'
			   ,GETDATE()),
			   (NEWID()
			   ,'Comfort'
			   ,GETDATE()),
			   (NEWID()
			   ,'Basic'
			   ,GETDATE())
	GO

-- [Car]
	CREATE TABLE [dbo].[Car](
		[Id] [uniqueidentifier] NOT NULL,
		[Manufacturer] [nvarchar](max) NULL,
		[Model] [nvarchar](max) NULL,
		[ManufacturedDate] [datetime2](7) NOT NULL,
		[Kilometrage] [float] NOT NULL,
		[SeatCount] [int] NOT NULL,
		[TypeId] [uniqueidentifier] NULL,
		[HasBabyChair] [bit] NOT NULL,
		[CreatedOn] [datetime2](7) NOT NULL,
		[ModifiedOn] [datetime2](7) NULL,
	 CONSTRAINT [PK_Car] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	GO

	ALTER TABLE [dbo].[Car]  WITH CHECK ADD  CONSTRAINT [FK_Car_CarType_TypeId] FOREIGN KEY([TypeId])
	REFERENCES [dbo].[CarType] ([Id])
	GO

	ALTER TABLE [dbo].[Car] CHECK CONSTRAINT [FK_Car_CarType_TypeId]
	GO

	CREATE TRIGGER [dbo].[Car_Update]
	   ON [dbo].[Car]
	   AFTER UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;

		IF NOT UPDATE([ModifiedOn])
			BEGIN
				UPDATE [dbo].[Car]
				SET [ModifiedOn] = GETDATE()
				WHERE [Id] = (SELECT [Id] from [inserted])
			END
	END
	GO

-- [Passenger]
	CREATE TABLE [dbo].[Passenger](
		[Id] [uniqueidentifier] NOT NULL,
		[Rating] [float] NOT NULL,
		[CreatedOn] [datetime2](7) NOT NULL,
		[ModifiedOn] [datetime2](7) NULL,
		[FirstName] [nvarchar](max) NULL,
		[LastName] [nvarchar](max) NULL,
		[MiddleName] [nvarchar](max) NULL,
		[BirthDate] [datetime2](7) NOT NULL,
	 CONSTRAINT [PK_Passenger] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	GO

	CREATE TRIGGER [dbo].[Passenger_Update]
	   ON [dbo].[Passenger]
	   AFTER UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;

		IF NOT UPDATE([ModifiedOn])
			BEGIN
				UPDATE [dbo].[Passenger]
				SET [ModifiedOn] = GETDATE()
				WHERE [Id] = (SELECT [Id] from [inserted])
			END
	END
	GO

-- [Drive]
	CREATE TABLE [dbo].[Drive](
		[Id] [uniqueidentifier] NOT NULL,
		[StartAt] [datetime2](7) NOT NULL,
		[CalculatedEndAt] [datetime2](7) NOT NULL,
		[FactEndAt] [datetime2](7) NULL,
		[PassengerId] [uniqueidentifier] NULL,
		[DriverId] [uniqueidentifier] NULL,
		[CarId] [uniqueidentifier] NULL,
		[CreatedOn] [datetime2](7) NOT NULL,
		[ModifiedOn] [datetime2](7) NULL,
	 CONSTRAINT [PK_Drive] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	ALTER TABLE [dbo].[Drive]  WITH CHECK ADD  CONSTRAINT [FK_Drive_Car_CarId] FOREIGN KEY([CarId])
	REFERENCES [dbo].[Car] ([Id])
	GO

	ALTER TABLE [dbo].[Drive] CHECK CONSTRAINT [FK_Drive_Car_CarId]
	GO

	ALTER TABLE [dbo].[Drive]  WITH CHECK ADD  CONSTRAINT [FK_Drive_Driver_DriverId] FOREIGN KEY([DriverId])
	REFERENCES [dbo].[Driver] ([Id])
	GO

	ALTER TABLE [dbo].[Drive] CHECK CONSTRAINT [FK_Drive_Driver_DriverId]
	GO

	ALTER TABLE [dbo].[Drive]  WITH CHECK ADD  CONSTRAINT [FK_Drive_Passenger_PassengerId] FOREIGN KEY([PassengerId])
	REFERENCES [dbo].[Passenger] ([Id])
	GO

	ALTER TABLE [dbo].[Drive] CHECK CONSTRAINT [FK_Drive_Passenger_PassengerId]
	GO

	CREATE TRIGGER [dbo].[Drive_Update]
	   ON [dbo].[Drive]
	   AFTER UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;

		IF NOT UPDATE([ModifiedOn])
			BEGIN
				UPDATE [dbo].[Drive]
				SET [ModifiedOn] = GETDATE()
				WHERE [Id] = (SELECT [Id] from [inserted])
			END
	END
	GO

-- [Order]
	CREATE TABLE [dbo].[Order](
		[Id] [uniqueidentifier] NOT NULL,
		[PassengerId] [uniqueidentifier] NULL,
		[DriveId] [uniqueidentifier] NULL,
		[Price] [float] NOT NULL,
		[Target] [nvarchar](max) NULL,
		[Destination] [nvarchar](max) NULL,
		[CreatedOn] [datetime2](7) NOT NULL,
		[ModifiedOn] [datetime2](7) NULL,
	 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	GO

	ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Drive_DriveId] FOREIGN KEY([DriveId])
	REFERENCES [dbo].[Drive] ([Id])
	GO

	ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Drive_DriveId]
	GO

	ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Passenger_PassengerId] FOREIGN KEY([PassengerId])
	REFERENCES [dbo].[Passenger] ([Id])
	GO

	ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Passenger_PassengerId]
	GO

	CREATE TRIGGER [dbo].[Order_Update]
	   ON [dbo].[Order]
	   AFTER UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;

		IF NOT UPDATE([ModifiedOn])
			BEGIN
				UPDATE [dbo].[Order]
				SET [ModifiedOn] = GETDATE()
				WHERE [Id] = (SELECT [Id] from [inserted])
			END
	END
	GO

-- [DriverCar]
	CREATE TABLE [dbo].[DriverCar](
		[Id] [uniqueidentifier] NOT NULL,
		[DriverId] [uniqueidentifier] NULL,
		[CarId] [uniqueidentifier] NULL,
		[UsedAt] [datetime2](7) NOT NULL,
		[CreatedOn] [datetime2](7) NOT NULL,
		[ModifiedOn] [datetime2](7) NULL,
	 CONSTRAINT [PK_DriverCar] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	ALTER TABLE [dbo].[DriverCar]  WITH CHECK ADD  CONSTRAINT [FK_DriverCar_Car_CarId] FOREIGN KEY([CarId])
	REFERENCES [dbo].[Car] ([Id])
	GO

	ALTER TABLE [dbo].[DriverCar] CHECK CONSTRAINT [FK_DriverCar_Car_CarId]
	GO

	ALTER TABLE [dbo].[DriverCar]  WITH CHECK ADD  CONSTRAINT [FK_DriverCar_Driver_DriverId] FOREIGN KEY([DriverId])
	REFERENCES [dbo].[Driver] ([Id])
	GO

	ALTER TABLE [dbo].[DriverCar] CHECK CONSTRAINT [FK_DriverCar_Driver_DriverId]
	GO

	CREATE TRIGGER [dbo].[DriverCar_Update]
	   ON [dbo].[DriverCar]
	   AFTER UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;

		IF NOT UPDATE([ModifiedOn])
			BEGIN
				UPDATE [dbo].[DriverCar]
				SET [ModifiedOn] = GETDATE()
				WHERE [Id] = (SELECT [Id] from [inserted])
			END
	END
	GO

-- [DriverOrder]
	CREATE TABLE [dbo].[DriverOrder](
		[Id] [uniqueidentifier] NOT NULL,
		[DriverId] [uniqueidentifier] NULL,
		[OrderId] [uniqueidentifier] NULL,
		[CreatedOn] [datetime2](7) NOT NULL,
		[ModifiedOn] [datetime2](7) NULL,
	 CONSTRAINT [PK_DriverOrder] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	ALTER TABLE [dbo].[DriverOrder]  WITH CHECK ADD  CONSTRAINT [FK_DriverOrder_Driver_DriverId] FOREIGN KEY([DriverId])
	REFERENCES [dbo].[Driver] ([Id])
	GO

	ALTER TABLE [dbo].[DriverOrder] CHECK CONSTRAINT [FK_DriverOrder_Driver_DriverId]
	GO

	ALTER TABLE [dbo].[DriverOrder]  WITH CHECK ADD  CONSTRAINT [FK_DriverOrder_Order_OrderId] FOREIGN KEY([OrderId])
	REFERENCES [dbo].[Order] ([Id])
	GO

	ALTER TABLE [dbo].[DriverOrder] CHECK CONSTRAINT [FK_DriverOrder_Order_OrderId]
	GO

	CREATE TRIGGER [dbo].[DriverOrder_Update]
	   ON [dbo].[DriverOrder]
	   AFTER UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;

		IF NOT UPDATE([ModifiedOn])
			BEGIN
				UPDATE [dbo].[DriverOrder]
				SET [ModifiedOn] = GETDATE()
				WHERE [Id] = (SELECT [Id] from [inserted])
			END
	END
	GO

-- [PassengerOrder]
	CREATE TABLE [dbo].[PassengerOrder](
		[Id] [uniqueidentifier] NOT NULL,
		[PassengerId] [uniqueidentifier] NULL,
		[OrderId] [uniqueidentifier] NULL,
		[CreatedOn] [datetime2](7) NOT NULL,
		[ModifiedOn] [datetime2](7) NULL,
	 CONSTRAINT [PK_PassengerOrder] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	ALTER TABLE [dbo].[PassengerOrder]  WITH CHECK ADD  CONSTRAINT [FK_PassengerOrder_Order_OrderId] FOREIGN KEY([OrderId])
	REFERENCES [dbo].[Order] ([Id])
	GO

	ALTER TABLE [dbo].[PassengerOrder] CHECK CONSTRAINT [FK_PassengerOrder_Order_OrderId]
	GO

	ALTER TABLE [dbo].[PassengerOrder]  WITH CHECK ADD  CONSTRAINT [FK_PassengerOrder_Passenger_PassengerId] FOREIGN KEY([PassengerId])
	REFERENCES [dbo].[Passenger] ([Id])
	GO

	ALTER TABLE [dbo].[PassengerOrder] CHECK CONSTRAINT [FK_PassengerOrder_Passenger_PassengerId]
	GO

	CREATE TRIGGER [dbo].[PassengerOrder_Update]
	   ON [dbo].[PassengerOrder]
	   AFTER UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;

		IF NOT UPDATE([ModifiedOn])
			BEGIN
				UPDATE [dbo].[PassengerOrder]
				SET [ModifiedOn] = GETDATE()
				WHERE [Id] = (SELECT [Id] from [inserted])
			END
	END
	GO