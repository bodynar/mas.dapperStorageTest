IF (
	NOT EXISTS
		(SELECT 
			NULL 
		FROM 
			master.dbo.sysdatabases 
		WHERE 
			('[' + name + ']' = 'DapperStorageTest' 
			OR name = 'DapperStorageTest')
	)
)
	BEGIN

		CREATE DATABASE [DapperStorageTest]
		 CONTAINMENT = NONE

		IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
		begin
			EXEC [DapperStorageTest].[dbo].[sp_fulltext_database] @action = 'enable'
		end

		ALTER DATABASE [DapperStorageTest] SET ANSI_NULL_DEFAULT OFF
		ALTER DATABASE [DapperStorageTest] SET ANSI_NULLS OFF
		ALTER DATABASE [DapperStorageTest] SET ANSI_PADDING OFF
		ALTER DATABASE [DapperStorageTest] SET ANSI_WARNINGS OFF
		ALTER DATABASE [DapperStorageTest] SET ARITHABORT OFF
		ALTER DATABASE [DapperStorageTest] SET AUTO_CLOSE ON
		ALTER DATABASE [DapperStorageTest] SET AUTO_SHRINK OFF
		ALTER DATABASE [DapperStorageTest] SET AUTO_UPDATE_STATISTICS ON
		ALTER DATABASE [DapperStorageTest] SET CURSOR_CLOSE_ON_COMMIT OFF
		ALTER DATABASE [DapperStorageTest] SET CURSOR_DEFAULT  GLOBAL
		ALTER DATABASE [DapperStorageTest] SET CONCAT_NULL_YIELDS_NULL OFF
		ALTER DATABASE [DapperStorageTest] SET NUMERIC_ROUNDABORT OFF
		ALTER DATABASE [DapperStorageTest] SET QUOTED_IDENTIFIER OFF
		ALTER DATABASE [DapperStorageTest] SET RECURSIVE_TRIGGERS OFF
		ALTER DATABASE [DapperStorageTest] SET  ENABLE_BROKER
		ALTER DATABASE [DapperStorageTest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
		ALTER DATABASE [DapperStorageTest] SET DATE_CORRELATION_OPTIMIZATION OFF
		ALTER DATABASE [DapperStorageTest] SET TRUSTWORTHY OFF
		ALTER DATABASE [DapperStorageTest] SET ALLOW_SNAPSHOT_ISOLATION OFF
		ALTER DATABASE [DapperStorageTest] SET PARAMETERIZATION SIMPLE
		ALTER DATABASE [DapperStorageTest] SET READ_COMMITTED_SNAPSHOT ON
		ALTER DATABASE [DapperStorageTest] SET HONOR_BROKER_PRIORITY OFF
		ALTER DATABASE [DapperStorageTest] SET RECOVERY SIMPLE
		ALTER DATABASE [DapperStorageTest] SET  MULTI_USER
		ALTER DATABASE [DapperStorageTest] SET PAGE_VERIFY CHECKSUM
		ALTER DATABASE [DapperStorageTest] SET DB_CHAINING OFF
		ALTER DATABASE [DapperStorageTest] SET FILESTREAM (NON_TRANSACTED_ACCESS = OFF)
		ALTER DATABASE [DapperStorageTest] SET TARGET_RECOVERY_TIME = 60 SECONDS
		ALTER DATABASE [DapperStorageTest] SET DELAYED_DURABILITY = DISABLED
		ALTER DATABASE [DapperStorageTest] SET QUERY_STORE = OFF
	END
;

USE [DapperStorageTest]
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF
ALTER DATABASE [DapperStorageTest] SET READ_WRITE

-- tables

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

-- [Driver]
IF (
	NOT EXISTS
	(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Driver')
)
	BEGIN
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
			([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
				ON [PRIMARY])
			ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		EXEC('
			CREATE TRIGGER [dbo].[Driver_Update]
			   ON [dbo].[Driver]
			AFTER UPDATE
			AS
				BEGIN
					IF (NOT UPDATE([ModifiedOn]))
						BEGIN
							UPDATE [dbo].[Driver]
							SET [ModifiedOn] = GETDATE()
							WHERE [Id] = (SELECT [Id] from [inserted])
						END
				END
		')

	END
	
-- [CarType]
IF (
	NOT EXISTS
	(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CarType')
)
	BEGIN
		CREATE TABLE [dbo].[CarType](
			[Id] [uniqueidentifier] NOT NULL,
			[Name] [nvarchar](max) NULL,
			[CreatedOn] [datetime2](7) NOT NULL,
			[ModifiedOn] [datetime2](7) NULL,
		 CONSTRAINT [PK_CarType] PRIMARY KEY CLUSTERED ([Id] ASC)
			WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		EXEC('
			CREATE TRIGGER [dbo].[CarType_Update]
			   ON [dbo].[CarType]
			   AFTER UPDATE
			AS 
			BEGIN
				SET NOCOUNT ON

				IF NOT UPDATE([ModifiedOn])
					BEGIN
						UPDATE [dbo].[CarType]
						SET [ModifiedOn] = GETDATE()
						WHERE [Id] = (SELECT [Id] from [inserted])
					END
			END
		')

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

	END

-- [Car]
IF (
	NOT EXISTS
	(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Car')
)
	BEGIN
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

		ALTER TABLE [dbo].[Car]  WITH CHECK ADD  CONSTRAINT [FK_Car_CarType_TypeId] FOREIGN KEY([TypeId])
		REFERENCES [dbo].[CarType] ([Id])

		ALTER TABLE [dbo].[Car] CHECK CONSTRAINT [FK_Car_CarType_TypeId]

		EXEC('
			CREATE TRIGGER [dbo].[Car_Update]
			   ON [dbo].[Car]
			   AFTER UPDATE
			AS 
			BEGIN
				SET NOCOUNT ON

				IF NOT UPDATE([ModifiedOn])
					BEGIN
						UPDATE [dbo].[Car]
						SET [ModifiedOn] = GETDATE()
						WHERE [Id] = (SELECT [Id] from [inserted])
					END
			END
		')
		
	END

-- [Passenger]
IF (
	NOT EXISTS
	(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Passenger')
)
	BEGIN
		CREATE TABLE [dbo].[Passenger](
			[Id] [uniqueidentifier] NOT NULL,
			[Rating] [float] NOT NULL,
			[CreatedOn] [datetime2](7) NOT NULL,
			[ModifiedOn] [datetime2](7) NULL,
			[FirstName] [nvarchar](max) NULL,
			[LastName] [nvarchar](max) NULL,
			[MiddleName] [nvarchar](max) NULL,
			[BirthDate] [datetime2](7) NOT NULL,
		 CONSTRAINT [PK_Passenger] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		EXEC('
			CREATE TRIGGER [dbo].[Passenger_Update]
			   ON [dbo].[Passenger]
			   AFTER UPDATE
			AS 
			BEGIN
				SET NOCOUNT ON

				IF NOT UPDATE([ModifiedOn])
					BEGIN
						UPDATE [dbo].[Passenger]
						SET [ModifiedOn] = GETDATE()
						WHERE [Id] = (SELECT [Id] from [inserted])
					END
			END
		')
END


-- [Drive]
IF (
	NOT EXISTS
	(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Drive')
)
	BEGIN
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
		 CONSTRAINT [PK_Drive] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[Drive]  WITH CHECK ADD  CONSTRAINT [FK_Drive_Car_CarId] FOREIGN KEY([CarId])
		REFERENCES [dbo].[Car] ([Id])

		ALTER TABLE [dbo].[Drive] CHECK CONSTRAINT [FK_Drive_Car_CarId]

		ALTER TABLE [dbo].[Drive]  WITH CHECK ADD  CONSTRAINT [FK_Drive_Driver_DriverId] FOREIGN KEY([DriverId])
		REFERENCES [dbo].[Driver] ([Id])

		ALTER TABLE [dbo].[Drive] CHECK CONSTRAINT [FK_Drive_Driver_DriverId]

		ALTER TABLE [dbo].[Drive]  WITH CHECK ADD  CONSTRAINT [FK_Drive_Passenger_PassengerId] FOREIGN KEY([PassengerId])
		REFERENCES [dbo].[Passenger] ([Id])

		ALTER TABLE [dbo].[Drive] CHECK CONSTRAINT [FK_Drive_Passenger_PassengerId]

		EXEC('
			CREATE TRIGGER [dbo].[Drive_Update]
			   ON [dbo].[Drive]
			   AFTER UPDATE
			AS 
			BEGIN
				SET NOCOUNT ON

				IF NOT UPDATE([ModifiedOn])
					BEGIN
						UPDATE [dbo].[Drive]
						SET [ModifiedOn] = GETDATE()
						WHERE [Id] = (SELECT [Id] from [inserted])
					END
			END
		')
	END

-- [Order]
IF (
	NOT EXISTS
	(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Order')
)
	BEGIN
		CREATE TABLE [dbo].[Order](
			[Id] [uniqueidentifier] NOT NULL,
			[PassengerId] [uniqueidentifier] NULL,
			[DriveId] [uniqueidentifier] NULL,
			[Price] [float] NOT NULL,
			[Target] [nvarchar](max) NULL,
			[Destination] [nvarchar](max) NULL,
			[CreatedOn] [datetime2](7) NOT NULL,
			[ModifiedOn] [datetime2](7) NULL,
		 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Drive_DriveId] FOREIGN KEY([DriveId])
		REFERENCES [dbo].[Drive] ([Id])

		ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Drive_DriveId]

		ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Passenger_PassengerId] FOREIGN KEY([PassengerId])
		REFERENCES [dbo].[Passenger] ([Id])

		ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Passenger_PassengerId]

		EXEC('
			CREATE TRIGGER [dbo].[Order_Update]
			   ON [dbo].[Order]
			   AFTER UPDATE
			AS 
			BEGIN
				SET NOCOUNT ON

				IF NOT UPDATE([ModifiedOn])
					BEGIN
						UPDATE [dbo].[Order]
						SET [ModifiedOn] = GETDATE()
						WHERE [Id] = (SELECT [Id] from [inserted])
					END
			END
		')
	END

-- [DriverCar]
IF (
	NOT EXISTS
	(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DriverCar')
)
	BEGIN
		CREATE TABLE [dbo].[DriverCar](
			[Id] [uniqueidentifier] NOT NULL,
			[DriverId] [uniqueidentifier] NULL,
			[CarId] [uniqueidentifier] NULL,
			[UsedAt] [datetime2](7) NOT NULL,
			[CreatedOn] [datetime2](7) NOT NULL,
			[ModifiedOn] [datetime2](7) NULL,
		 CONSTRAINT [PK_DriverCar] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[DriverCar]  WITH CHECK ADD  CONSTRAINT [FK_DriverCar_Car_CarId] FOREIGN KEY([CarId])
		REFERENCES [dbo].[Car] ([Id])

		ALTER TABLE [dbo].[DriverCar] CHECK CONSTRAINT [FK_DriverCar_Car_CarId]

		ALTER TABLE [dbo].[DriverCar]  WITH CHECK ADD  CONSTRAINT [FK_DriverCar_Driver_DriverId] FOREIGN KEY([DriverId])
		REFERENCES [dbo].[Driver] ([Id])

		ALTER TABLE [dbo].[DriverCar] CHECK CONSTRAINT [FK_DriverCar_Driver_DriverId]

		EXEC('
			CREATE TRIGGER [dbo].[DriverCar_Update]
			   ON [dbo].[DriverCar]
			   AFTER UPDATE
			AS 
			BEGIN
				SET NOCOUNT ON

				IF NOT UPDATE([ModifiedOn])
					BEGIN
						UPDATE [dbo].[DriverCar]
						SET [ModifiedOn] = GETDATE()
						WHERE [Id] = (SELECT [Id] from [inserted])
					END
			END
		')
	END

-- [DriverOrder]
IF (
	NOT EXISTS
	(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DriverOrder')
)
	BEGIN
		CREATE TABLE [dbo].[DriverOrder](
			[Id] [uniqueidentifier] NOT NULL,
			[DriverId] [uniqueidentifier] NULL,
			[OrderId] [uniqueidentifier] NULL,
			[CreatedOn] [datetime2](7) NOT NULL,
			[ModifiedOn] [datetime2](7) NULL,
		 CONSTRAINT [PK_DriverOrder] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[DriverOrder]  WITH CHECK ADD  CONSTRAINT [FK_DriverOrder_Driver_DriverId] FOREIGN KEY([DriverId])
		REFERENCES [dbo].[Driver] ([Id])

		ALTER TABLE [dbo].[DriverOrder] CHECK CONSTRAINT [FK_DriverOrder_Driver_DriverId]

		ALTER TABLE [dbo].[DriverOrder]  WITH CHECK ADD  CONSTRAINT [FK_DriverOrder_Order_OrderId] FOREIGN KEY([OrderId])
		REFERENCES [dbo].[Order] ([Id])

		ALTER TABLE [dbo].[DriverOrder] CHECK CONSTRAINT [FK_DriverOrder_Order_OrderId]

		EXEC('
			CREATE TRIGGER [dbo].[DriverOrder_Update]
			   ON [dbo].[DriverOrder]
			   AFTER UPDATE
			AS 
			BEGIN
				SET NOCOUNT ON

				IF NOT UPDATE([ModifiedOn])
					BEGIN
						UPDATE [dbo].[DriverOrder]
						SET [ModifiedOn] = GETDATE()
						WHERE [Id] = (SELECT [Id] from [inserted])
					END
			END
		')
	END

-- [PassengerOrder]
IF (
	NOT EXISTS
	(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PassengerOrder')
)
	BEGIN
		CREATE TABLE [dbo].[PassengerOrder](
			[Id] [uniqueidentifier] NOT NULL,
			[PassengerId] [uniqueidentifier] NULL,
			[OrderId] [uniqueidentifier] NULL,
			[CreatedOn] [datetime2](7) NOT NULL,
			[ModifiedOn] [datetime2](7) NULL,
		 CONSTRAINT [PK_PassengerOrder] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[PassengerOrder]  WITH CHECK ADD  CONSTRAINT [FK_PassengerOrder_Order_OrderId] FOREIGN KEY([OrderId])
		REFERENCES [dbo].[Order] ([Id])

		ALTER TABLE [dbo].[PassengerOrder] CHECK CONSTRAINT [FK_PassengerOrder_Order_OrderId]

		ALTER TABLE [dbo].[PassengerOrder]  WITH CHECK ADD  CONSTRAINT [FK_PassengerOrder_Passenger_PassengerId] FOREIGN KEY([PassengerId])
		REFERENCES [dbo].[Passenger] ([Id])

		ALTER TABLE [dbo].[PassengerOrder] CHECK CONSTRAINT [FK_PassengerOrder_Passenger_PassengerId]

		EXEC('
			CREATE TRIGGER [dbo].[PassengerOrder_Update]
			   ON [dbo].[PassengerOrder]
			   AFTER UPDATE
			AS 
			BEGIN
				SET NOCOUNT ON

				IF NOT UPDATE([ModifiedOn])
					BEGIN
						UPDATE [dbo].[PassengerOrder]
						SET [ModifiedOn] = GETDATE()
						WHERE [Id] = (SELECT [Id] from [inserted])
					END
			END
		')
	END

