USE [master]
GO
/****** Object:  Database [Ravenfall2]    Script Date: 2020-07-25 20:59:22 ******/
CREATE DATABASE [Ravenfall2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Ravenfall2', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Ravenfall2.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Ravenfall2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Ravenfall2_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Ravenfall2] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Ravenfall2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Ravenfall2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Ravenfall2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Ravenfall2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Ravenfall2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Ravenfall2] SET ARITHABORT OFF 
GO
ALTER DATABASE [Ravenfall2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Ravenfall2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Ravenfall2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Ravenfall2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Ravenfall2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Ravenfall2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Ravenfall2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Ravenfall2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Ravenfall2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Ravenfall2] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Ravenfall2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Ravenfall2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Ravenfall2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Ravenfall2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Ravenfall2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Ravenfall2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Ravenfall2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Ravenfall2] SET RECOVERY FULL 
GO
ALTER DATABASE [Ravenfall2] SET  MULTI_USER 
GO
ALTER DATABASE [Ravenfall2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Ravenfall2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Ravenfall2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Ravenfall2] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Ravenfall2] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Ravenfall2', N'ON'
GO
ALTER DATABASE [Ravenfall2] SET QUERY_STORE = OFF
GO
USE [Ravenfall2]
GO
/****** Object:  Table [dbo].[Appearance]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appearance](
	[Id] [int] NOT NULL,
	[Gender] [int] NULL,
	[Hair] [int] NULL,
	[Head] [int] NULL,
	[Eyebrows] [int] NULL,
	[FacialHair] [int] NULL,
	[SkinColor] [nvarchar](50) NULL,
	[HairColor] [nvarchar](50) NULL,
	[BeardColor] [nvarchar](50) NULL,
	[EyeColor] [nvarchar](50) NULL,
	[StubbleColor] [nvarchar](50) NULL,
	[WarPaintColor] [nvarchar](50) NULL,
	[HelmetVisible] [bit] NULL,
 CONSTRAINT [PK_Appearance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attributes]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attributes](
	[Id] [int] NOT NULL,
	[Health] [int] NULL,
	[Attack] [int] NULL,
	[Strength] [int] NULL,
	[Defense] [int] NULL,
	[Dexterity] [int] NULL,
	[Agility] [int] NULL,
	[Intellect] [int] NULL,
	[Endurance] [int] NULL,
	[Evasion] [int] NULL,
	[HealthExp] [decimal](18, 0) NULL,
	[AttackExp] [decimal](18, 0) NULL,
	[StrengthExp] [decimal](18, 0) NULL,
	[DefenseExp] [decimal](18, 0) NULL,
	[DexterityExp] [decimal](18, 0) NULL,
	[AgilityExp] [decimal](18, 0) NULL,
	[IntellectExp] [decimal](18, 0) NULL,
	[EnduranceExp] [decimal](18, 0) NULL,
	[EvasionExp] [decimal](18, 0) NULL,
 CONSTRAINT [PK_Attributes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GameObject]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameObject](
	[Id] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[TransformId] [int] NULL,
	[Experience] [numeric](18, 0) NULL,
	[InteractItemType] [int] NULL,
	[RespawnMilliseconds] [int] NULL,
	[Static] [bit] NOT NULL,
 CONSTRAINT [PK_GameObject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GameObjectInstance]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameObjectInstance](
	[Id] [int] NOT NULL,
	[ObjectId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[SessionId] [int] NOT NULL,
 CONSTRAINT [PK_GameObjectInstance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InventoryItem]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventoryItem](
	[Id] [int] NOT NULL,
	[PlayerId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[Equipped] [bit] NOT NULL,
 CONSTRAINT [PK_InventoryItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Stackable] [bit] NOT NULL,
	[Equippable] [bit] NOT NULL,
	[Consumable] [bit] NOT NULL,
	[Tier] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemDrop]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemDrop](
	[Id] [int] NOT NULL,
	[EntityId] [int] NOT NULL,
	[EntityType] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[DropChance] [float] NOT NULL,
 CONSTRAINT [PK_ItemDrop] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Npc]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Npc](
	[Id] [int] NOT NULL,
	[NpcId] [int] NOT NULL,
	[TransformId] [int] NULL,
	[AttributesId] [int] NULL,
	[Level] [int] NULL,
	[RespawnTimeMs] [int] NOT NULL,
	[Alignment] [int] NOT NULL,
 CONSTRAINT [PK_Npc] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NpcInstance]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NpcInstance](
	[Id] [int] NOT NULL,
	[NpcId] [int] NOT NULL,
	[TransformId] [int] NOT NULL,
	[Health] [int] NULL,
	[Endurance] [bigint] NULL,
	[Alignment] [int] NOT NULL,
	[SessionId] [int] NOT NULL,
 CONSTRAINT [PK_NpcInstance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Player]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Player](
	[Id] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[TransformId] [int] NULL,
	[AppearanceId] [int] NULL,
	[AttributesId] [int] NULL,
	[ProfessionsId] [int] NULL,
	[Level] [int] NULL,
	[Experience] [decimal](18, 0) NULL,
	[Health] [int] NULL,
	[Endurance] [bigint] NULL,
	[Coins] [bigint] NOT NULL,
	[SessionId] [int] NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Professions]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Professions](
	[Id] [int] NOT NULL,
	[Fishing] [int] NULL,
	[Mining] [int] NULL,
	[Crafting] [int] NULL,
	[Cooking] [int] NULL,
	[Woodcutting] [int] NULL,
	[Farming] [int] NULL,
	[Sailing] [int] NULL,
	[Slayer] [int] NULL,
	[FishingExp] [decimal](18, 0) NULL,
	[MiningExp] [decimal](18, 0) NULL,
	[CraftingExp] [decimal](18, 0) NULL,
	[CookingExp] [decimal](18, 0) NULL,
	[WoodcuttingExp] [decimal](18, 0) NULL,
	[FarmingExp] [decimal](18, 0) NULL,
	[SailingExp] [decimal](18, 0) NULL,
	[SlayerExp] [decimal](18, 0) NULL,
 CONSTRAINT [PK_Professions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Session]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Session](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UserId] [int] NULL,
	[Created] [datetime] NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShopItem]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShopItem](
	[Id] [int] NOT NULL,
	[NpcInstanceId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[Price] [int] NOT NULL,
 CONSTRAINT [PK_ShopItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transform]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transform](
	[Id] [int] NOT NULL,
	[PositionX] [float] NOT NULL,
	[PositionY] [float] NOT NULL,
	[PositionZ] [float] NOT NULL,
	[DestinationX] [float] NOT NULL,
	[DestinationY] [float] NOT NULL,
	[DestinationZ] [float] NOT NULL,
	[RotationX] [float] NOT NULL,
	[RotationY] [float] NOT NULL,
	[RotationZ] [float] NOT NULL,
 CONSTRAINT [PK_Transform] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2020-07-25 20:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[TwitchId] [nvarchar](50) NULL,
	[YouTubeId] [nvarchar](50) NULL,
	[Created] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Npc] ADD  CONSTRAINT [DF_Npc_Level]  DEFAULT ((1)) FOR [Level]
GO
ALTER TABLE [dbo].[Player] ADD  CONSTRAINT [DF_Player_Level]  DEFAULT ((1)) FOR [Level]
GO
ALTER TABLE [dbo].[Player] ADD  CONSTRAINT [DF_Player_Coins]  DEFAULT ((0)) FOR [Coins]
GO
ALTER TABLE [dbo].[Transform] ADD  CONSTRAINT [DF_Transform_PositionX]  DEFAULT ((0)) FOR [PositionX]
GO
ALTER TABLE [dbo].[Transform] ADD  CONSTRAINT [DF_Transform_PositionY]  DEFAULT ((0)) FOR [PositionY]
GO
ALTER TABLE [dbo].[Transform] ADD  CONSTRAINT [DF_Transform_PositionZ]  DEFAULT ((0)) FOR [PositionZ]
GO
ALTER TABLE [dbo].[Transform] ADD  CONSTRAINT [DF_Transform_DestinationX]  DEFAULT ((0)) FOR [DestinationX]
GO
ALTER TABLE [dbo].[Transform] ADD  CONSTRAINT [DF_Transform_DestinationY]  DEFAULT ((0)) FOR [DestinationY]
GO
ALTER TABLE [dbo].[Transform] ADD  CONSTRAINT [DF_Transform_DestinationZ]  DEFAULT ((0)) FOR [DestinationZ]
GO
ALTER TABLE [dbo].[Transform] ADD  CONSTRAINT [DF_Transform_RotationX]  DEFAULT ((0)) FOR [RotationX]
GO
ALTER TABLE [dbo].[Transform] ADD  CONSTRAINT [DF_Transform_RotationY]  DEFAULT ((0)) FOR [RotationY]
GO
ALTER TABLE [dbo].[Transform] ADD  CONSTRAINT [DF_Transform_RotationZ]  DEFAULT ((0)) FOR [RotationZ]
GO
USE [master]
GO
ALTER DATABASE [Ravenfall2] SET  READ_WRITE 
GO
