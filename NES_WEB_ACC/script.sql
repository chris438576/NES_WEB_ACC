USE [NES_WEB_ACC]
GO
/****** Object:  Table [dbo].[LNK_RolePermissions]    Script Date: 2024/4/2 上午 11:28:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LNK_RolePermissions](
	[Id] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NULL,
	[PermissionId] [uniqueidentifier] NULL,
	[Status] [bit] NULL,
	[CreateEmpId] [bigint] NULL,
	[CreateBy] [nvarchar](20) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateEmpId] [bigint] NULL,
	[UpdateBy] [nvarchar](20) NULL,
	[UpdateDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LNK_UserRole]    Script Date: 2024/4/2 上午 11:28:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LNK_UserRole](
	[Id] [uniqueidentifier] NOT NULL,
	[EmpId] [bigint] NULL,
	[RoleId] [uniqueidentifier] NULL,
	[Status] [bit] NULL,
	[CreateEmpId] [bigint] NULL,
	[CreateBy] [nvarchar](20) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateEmpId] [bigint] NULL,
	[UpdateBy] [nvarchar](20) NULL,
	[UpdateDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SYS_Permissions]    Script Date: 2024/4/2 上午 11:28:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SYS_Permissions](
	[PermissionId] [uniqueidentifier] NOT NULL,
	[PageName] [nvarchar](50) NULL,
	[Action] [nvarchar](50) NULL,
	[IsDisable] [bit] NULL,
	[Status] [bit] NULL,
	[CreateEmpId] [bigint] NULL,
	[CreateBy] [nvarchar](20) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateEmpId] [bigint] NULL,
	[UpdateBy] [nvarchar](20) NULL,
	[UpdateDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SYS_Roles]    Script Date: 2024/4/2 上午 11:28:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SYS_Roles](
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](50) NULL,
	[Status] [bit] NULL,
	[CreateEmpId] [bigint] NULL,
	[CreateBy] [nvarchar](20) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateEmpId] [bigint] NULL,
	[UpdateBy] [nvarchar](20) NULL,
	[UpdateDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SYS_Users]    Script Date: 2024/4/2 上午 11:28:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SYS_Users](
	[EmpId] [bigint] NOT NULL,
	[EmpNo] [nvarchar](20) NULL,
	[EmpNameC] [nvarchar](60) NULL,
	[DeptNo] [nvarchar](20) NULL,
	[DeptName] [nvarchar](60) NULL,
	[Status] [bit] NULL,
	[CreateEmpId] [bigint] NULL,
	[CreateBy] [nvarchar](20) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateEmpId] [bigint] NULL,
	[UpdateBy] [nvarchar](20) NULL,
	[UpdateDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmpId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[LNK_UserRole] ([Id], [EmpId], [RoleId], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (N'00000000-0000-0000-0000-000000000000', 5307382783087145956, N'f9f03290-6e5f-4a8f-a5ba-8406d525be55', 1, NULL, N'NES1492', CAST(N'2024-04-01T11:15:25.420' AS DateTime), NULL, NULL, CAST(N'2024-04-01T11:35:09.480' AS DateTime))
INSERT [dbo].[LNK_UserRole] ([Id], [EmpId], [RoleId], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (N'cf69c1f7-14c4-448a-8e48-3ba840580e38', 5307382783087145956, N'56afe5e3-9ed0-4fd0-9456-98ba1dc78b9d', 1, 5307382783087145956, N'NES1492', CAST(N'2024-04-01T00:00:00.000' AS DateTime), NULL, N'NES1492', CAST(N'2024-04-01T10:59:42.343' AS DateTime))
INSERT [dbo].[LNK_UserRole] ([Id], [EmpId], [RoleId], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (N'f6193524-fac5-45e6-932d-6e190e29a100', 150710180313866, N'f9f03290-6e5f-4a8f-a5ba-8406d525be55', 0, NULL, N'NES1492', CAST(N'2024-04-01T11:45:47.163' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[LNK_UserRole] ([Id], [EmpId], [RoleId], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (N'17dc7db9-3bc9-4d61-b2ac-cca7bb41a1dd', 150710180313866, N'56afe5e3-9ed0-4fd0-9456-98ba1dc78b9d', 1, NULL, N'NES1492', CAST(N'2024-04-01T11:45:47.163' AS DateTime), NULL, N'NES1492', CAST(N'2024-04-01T11:46:30.747' AS DateTime))
INSERT [dbo].[SYS_Roles] ([RoleId], [RoleName], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (N'f9fdcb5e-d2d5-4164-9325-0cc1a8955a43', N'TEST001', 1, NULL, N'NES1492', CAST(N'2024-04-01T14:09:56.227' AS DateTime), NULL, N'NES1492', CAST(N'2024-04-01T14:09:59.057' AS DateTime))
INSERT [dbo].[SYS_Roles] ([RoleId], [RoleName], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (N'f9f03290-6e5f-4a8f-a5ba-8406d525be55', N'User', 0, 5307382783087145956, N'NES1492', CAST(N'2024-04-01T00:00:00.000' AS DateTime), NULL, N'NES1492', CAST(N'2024-04-01T14:09:32.707' AS DateTime))
INSERT [dbo].[SYS_Roles] ([RoleId], [RoleName], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (N'56afe5e3-9ed0-4fd0-9456-98ba1dc78b9d', N'Admin', 1, 5307382783087145956, N'NES1492', CAST(N'2024-04-01T00:00:00.000' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[SYS_Users] ([EmpId], [EmpNo], [EmpNameC], [DeptNo], [DeptName], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (150710180313866, N'NES0008', N'蔡慈芳', N'A0', N'總經理室', 1, NULL, NULL, CAST(N'2024-04-01T10:18:36.247' AS DateTime), NULL, N'NES1492', CAST(N'2024-04-01T10:26:24.887' AS DateTime))
INSERT [dbo].[SYS_Users] ([EmpId], [EmpNo], [EmpNameC], [DeptNo], [DeptName], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (150710180345531, N'NES0009', N'邱良福', N'A3', N'採購部', 1, NULL, NULL, CAST(N'2024-04-01T10:18:36.247' AS DateTime), NULL, N'NES1492', CAST(N'2024-04-01T11:12:55.933' AS DateTime))
INSERT [dbo].[SYS_Users] ([EmpId], [EmpNo], [EmpNameC], [DeptNo], [DeptName], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (150710180416187, N'NES0024', N'許麗珠', N'A3', N'採購部', 0, 5307382783087145956, N'NES1492', CAST(N'2024-03-29T00:00:00.000' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[SYS_Users] ([EmpId], [EmpNo], [EmpNameC], [DeptNo], [DeptName], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (150710180539174, N'NES0048', N'林佳蓁', N'A6', N'工程部', 0, NULL, NULL, CAST(N'2024-04-01T10:18:56.603' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[SYS_Users] ([EmpId], [EmpNo], [EmpNameC], [DeptNo], [DeptName], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (150710180617193, N'NES0049', N'傅奕群', N'A4', N'業務部', 0, NULL, NULL, CAST(N'2024-04-01T10:18:56.603' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[SYS_Users] ([EmpId], [EmpNo], [EmpNameC], [DeptNo], [DeptName], [Status], [CreateEmpId], [CreateBy], [CreateDate], [UpdateEmpId], [UpdateBy], [UpdateDate]) VALUES (5307382783087145956, N'NES1492', N'陳頴賢', N'A0', N'總經理室', 1, 5307382783087145956, N'NES1492', CAST(N'2024-04-01T00:00:00.000' AS DateTime), NULL, NULL, NULL)
ALTER TABLE [dbo].[LNK_RolePermissions] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[LNK_UserRole] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[SYS_Permissions] ADD  DEFAULT (newid()) FOR [PermissionId]
GO
ALTER TABLE [dbo].[SYS_Roles] ADD  DEFAULT (newid()) FOR [RoleId]
GO
