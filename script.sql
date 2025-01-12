USE [master]
GO
/****** Object:  Database [ArtPatio]    Script Date: 10/16/2024 5:20:00 PM ******/
CREATE DATABASE [ArtPatio]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ArtPatio', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ArtPatio.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ArtPatio_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ArtPatio_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ArtPatio] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ArtPatio].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ArtPatio] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ArtPatio] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ArtPatio] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ArtPatio] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ArtPatio] SET ARITHABORT OFF 
GO
ALTER DATABASE [ArtPatio] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ArtPatio] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ArtPatio] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ArtPatio] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ArtPatio] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ArtPatio] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ArtPatio] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ArtPatio] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ArtPatio] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ArtPatio] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ArtPatio] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ArtPatio] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ArtPatio] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ArtPatio] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ArtPatio] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ArtPatio] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ArtPatio] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ArtPatio] SET RECOVERY FULL 
GO
ALTER DATABASE [ArtPatio] SET  MULTI_USER 
GO
ALTER DATABASE [ArtPatio] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ArtPatio] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ArtPatio] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ArtPatio] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ArtPatio] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ArtPatio] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ArtPatio', N'ON'
GO
ALTER DATABASE [ArtPatio] SET QUERY_STORE = ON
GO
ALTER DATABASE [ArtPatio] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ArtPatio]
GO
/****** Object:  Table [dbo].[Artworks]    Script Date: 10/16/2024 5:20:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artworks](
	[ArtId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ArtName] [nvarchar](255) NOT NULL,
	[ArtMaterial] [nvarchar](255) NOT NULL,
	[ArtDetails] [nvarchar](max) NULL,
	[ArtImage] [nvarchar](255) NOT NULL,
	[UserName] [nvarchar](255) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Buyer] [int] NULL,
	[Status] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ArtId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 10/16/2024 5:20:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ArtId] [int] NULL,
	[TransactionType] [varchar](50) NOT NULL,
	[PreviousBalance] [decimal](18, 2) NOT NULL,
	[UpdatedBalance] [decimal](18, 2) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[BuyerId] [int] NULL,
	[ArtistId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 10/16/2024 5:20:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[ConfirmPassword] [nvarchar](255) NOT NULL,
	[Address] [varchar](255) NULL,
	[Contact] [varchar](50) NULL,
	[UserType] [varchar](50) NULL,
	[Description] [text] NULL,
	[Balance] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Artworks] ON 

INSERT [dbo].[Artworks] ([ArtId], [UserId], [ArtName], [ArtMaterial], [ArtDetails], [ArtImage], [UserName], [Price], [Buyer], [Status]) VALUES (1, 6, N'Guernica', N'Oil Painting', N'Guernica is a large 1937 oil painting by Spanish artist Pablo Picasso. It is one of his best-known works, regarded by many art critics as the most moving ', N'/images/artworks/Guernica.png', N'Pablo Piccaso', CAST(300.00 AS Decimal(18, 2)), NULL, N'Available')
INSERT [dbo].[Artworks] ([ArtId], [UserId], [ArtName], [ArtMaterial], [ArtDetails], [ArtImage], [UserName], [Price], [Buyer], [Status]) VALUES (2, 7, N'Monalisa', N'Wood, Cottonwood', N'The Mona Lisa is a half-length portrait painting by Italian artist Leonardo da Vinci. Considered an archetypal masterpiece of the Italian Renaissance, it has been described as "the best known, the most visited, the most written about, the most sung about, [and] the most parodied work of art in the world"', N'/images/artworks/monalisa.jpg', N'Leonardo De Vinci', CAST(400.00 AS Decimal(18, 2)), 9, N'Sold')
INSERT [dbo].[Artworks] ([ArtId], [UserId], [ArtName], [ArtMaterial], [ArtDetails], [ArtImage], [UserName], [Price], [Buyer], [Status]) VALUES (3, 6, N'Girl before a Mirror', N'Oil Painting', N'Girl before a Mirror is an oil on canvas painting by Pablo Picasso, which he created in 1932.', N'/images/artworks/girlBeforeAmirror.jpg', N'Pablo Piccaso', CAST(450.00 AS Decimal(18, 2)), NULL, N'Available')
INSERT [dbo].[Artworks] ([ArtId], [UserId], [ArtName], [ArtMaterial], [ArtDetails], [ArtImage], [UserName], [Price], [Buyer], [Status]) VALUES (4, 7, N'The Battle of Anghiari', N'Coal Painting', N'The Battle of Anghiari (1505) is a painting by Leonardo da Vinci in the Salone dei Cinquecento in the Palazzo Vecchio, Florence.', N'/images/artworks/TheBattleofAnghiari.jpg', N'Leonardo De Vinci', CAST(320.00 AS Decimal(18, 2)), NULL, N'Available')
INSERT [dbo].[Artworks] ([ArtId], [UserId], [ArtName], [ArtMaterial], [ArtDetails], [ArtImage], [UserName], [Price], [Buyer], [Status]) VALUES (5, 7, N'Demo', N'Demo', N'Demo', N'/images/artworks/demo.jpg', N'Leonardo De Vinci', CAST(1000.00 AS Decimal(18, 2)), NULL, N'Available')
INSERT [dbo].[Artworks] ([ArtId], [UserId], [ArtName], [ArtMaterial], [ArtDetails], [ArtImage], [UserName], [Price], [Buyer], [Status]) VALUES (6, 10, N'Cat', N'Pencil', N'Nice cat', N'/images/artworks/cat.jpg', N'Dipon Kumar', CAST(500.00 AS Decimal(18, 2)), NULL, N'Available')
SET IDENTITY_INSERT [dbo].[Artworks] OFF
GO
SET IDENTITY_INSERT [dbo].[Transactions] ON 

INSERT [dbo].[Transactions] ([TransactionId], [UserId], [ArtId], [TransactionType], [PreviousBalance], [UpdatedBalance], [TransactionDate], [BuyerId], [ArtistId]) VALUES (11, 9, NULL, N'BalanceAdded', CAST(0.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), CAST(N'2024-10-16T13:16:22.393' AS DateTime), NULL, NULL)
INSERT [dbo].[Transactions] ([TransactionId], [UserId], [ArtId], [TransactionType], [PreviousBalance], [UpdatedBalance], [TransactionDate], [BuyerId], [ArtistId]) VALUES (12, 9, NULL, N'BalanceAdded', CAST(500.00 AS Decimal(18, 2)), CAST(550.00 AS Decimal(18, 2)), CAST(N'2024-10-16T13:16:34.050' AS DateTime), NULL, NULL)
INSERT [dbo].[Transactions] ([TransactionId], [UserId], [ArtId], [TransactionType], [PreviousBalance], [UpdatedBalance], [TransactionDate], [BuyerId], [ArtistId]) VALUES (13, 9, 2, N'ArtworkPurchased', CAST(550.00 AS Decimal(18, 2)), CAST(150.00 AS Decimal(18, 2)), CAST(N'2024-10-16T13:25:07.510' AS DateTime), NULL, 7)
INSERT [dbo].[Transactions] ([TransactionId], [UserId], [ArtId], [TransactionType], [PreviousBalance], [UpdatedBalance], [TransactionDate], [BuyerId], [ArtistId]) VALUES (14, 7, 2, N'ArtworkSold', CAST(0.00 AS Decimal(18, 2)), CAST(400.00 AS Decimal(18, 2)), CAST(N'2024-10-16T13:25:07.513' AS DateTime), 9, NULL)
SET IDENTITY_INSERT [dbo].[Transactions] OFF
GO
SET IDENTITY_INSERT [dbo].[UserProfile] ON 

INSERT [dbo].[UserProfile] ([Id], [Name], [Email], [Password], [ConfirmPassword], [Address], [Contact], [UserType], [Description], [Balance]) VALUES (3, N'Karim mia', N'karim@gmail.com', N'Aa@12345', N'Aa@12345', NULL, NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[UserProfile] ([Id], [Name], [Email], [Password], [ConfirmPassword], [Address], [Contact], [UserType], [Description], [Balance]) VALUES (4, N'RIDHON', N'ridhon@gmail.com', N'Aa@12345', N'Aa@12345', NULL, NULL, NULL, NULL, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[UserProfile] ([Id], [Name], [Email], [Password], [ConfirmPassword], [Address], [Contact], [UserType], [Description], [Balance]) VALUES (5, N'Riajul', N'riajul@gmail.com', N'1234', N'1234', N'Dhaka,Bangladesh', N'1054566', N'Customer', N'new customer', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[UserProfile] ([Id], [Name], [Email], [Password], [ConfirmPassword], [Address], [Contact], [UserType], [Description], [Balance]) VALUES (6, N'Pablo Piccaso', N'pablo@gmail.com', N'1234', N'1234', N'Italy', N'98556655', N'Artist', N'The greatest Artist', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[UserProfile] ([Id], [Name], [Email], [Password], [ConfirmPassword], [Address], [Contact], [UserType], [Description], [Balance]) VALUES (7, N'Leonardo De Vinci', N'vinci@gmail.com', N'Aa@12345', N'Aa@12345', N'Anchiano, Italy', N'01687985456', N'Artist', N'Greatest Artist', CAST(400.00 AS Decimal(18, 2)))
INSERT [dbo].[UserProfile] ([Id], [Name], [Email], [Password], [ConfirmPassword], [Address], [Contact], [UserType], [Description], [Balance]) VALUES (8, N'Dhaka Art Gallery', N'dhaka@gmail.com', N'Aa@12345', N'Aa@12345', N'Dhaka,Bangladesh', N'01689758558', N'Gallery', N'This is the largest gallery in Dhaka', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[UserProfile] ([Id], [Name], [Email], [Password], [ConfirmPassword], [Address], [Contact], [UserType], [Description], [Balance]) VALUES (9, N'Fuad Hasan', N'fhasan@gmail.com', N'Aa@12345', N'Aa@12345', N'Dhaka,Bangladesh', N'01627695690', N'Customer', N'I am an art enthusiast', CAST(150.00 AS Decimal(18, 2)))
INSERT [dbo].[UserProfile] ([Id], [Name], [Email], [Password], [ConfirmPassword], [Address], [Contact], [UserType], [Description], [Balance]) VALUES (10, N'Dipon Kumar', N'dipon@gmail.com', N'Aa@12345', N'Aa@12345', N'Dhaka,Bangladesh', N'02168552255', N'Artist', N'a great artist', CAST(0.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__UserProf__A9D105345B170B6A]    Script Date: 10/16/2024 5:20:01 PM ******/
ALTER TABLE [dbo].[UserProfile] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Artworks] ADD  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[Artworks] ADD  DEFAULT ('Available') FOR [Status]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT (getdate()) FOR [TransactionDate]
GO
ALTER TABLE [dbo].[UserProfile] ADD  DEFAULT ((0.00)) FOR [Balance]
GO
ALTER TABLE [dbo].[Artworks]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([Id])
GO
USE [master]
GO
ALTER DATABASE [ArtPatio] SET  READ_WRITE 
GO
