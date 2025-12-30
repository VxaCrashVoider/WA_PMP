USE WA_PM
CREATE TABLE [dbo].[Organizations]
(
    [OrganizationId]  UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_Organizations] PRIMARY KEY,
    [Name]            NVARCHAR(200) NOT NULL,
    [Slug]            NVARCHAR(100) NOT NULL,
    [BillingEmail]    NVARCHAR(320) NULL,
    [Plan]            NVARCHAR(50) NOT NULL CONSTRAINT [DF_Organizations_Plan] DEFAULT(N'Free'),
    [IsActive]        BIT NOT NULL CONSTRAINT [DF_Organizations_IsActive] DEFAULT(1),
    [IsDeleted]       BIT NOT NULL CONSTRAINT [DF_Organizations_IsDeleted] DEFAULT(0),
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Organizations_CreatedAt] DEFAULT SYSUTCDATETIME(),
    [UpdatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Organizations_UpdatedAt] DEFAULT SYSUTCDATETIME(),
    [RowVer]          ROWVERSION NOT NULL
);
GO

CREATE TABLE [dbo].[Users]
(
    [UserId]          UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_Users] PRIMARY KEY,
    [Email]           NVARCHAR(320) NOT NULL,
    [DisplayName]     NVARCHAR(200) NOT NULL,
    [PasswordHash]    VARBINARY(256) NULL,
    [AvatarUrl]       NVARCHAR(1000) NULL,
    [IsActive]        BIT NOT NULL CONSTRAINT [DF_Users_IsActive] DEFAULT(1),
    [IsDeleted]       BIT NOT NULL CONSTRAINT [DF_Users_IsDeleted] DEFAULT(0),
    [LastLoginAt]     DATETIME2(3) NULL,
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Users_CreatedAt] DEFAULT SYSUTCDATETIME(),
    [UpdatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Users_UpdatedAt] DEFAULT SYSUTCDATETIME(),
    [RowVer]          ROWVERSION NOT NULL
);
GO

CREATE TABLE [dbo].[OrganizationMembers]
(
    [OrganizationId]  UNIQUEIDENTIFIER NOT NULL,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [OrgRole]         NVARCHAR(50) NOT NULL CONSTRAINT [DF_OrgMembers_Role] DEFAULT(N'Member'),
    [JoinedAt]        DATETIME2(3) NOT NULL CONSTRAINT [DF_OrgMembers_JoinedAt] DEFAULT SYSUTCDATETIME(),
    [IsActive]        BIT NOT NULL CONSTRAINT [DF_OrgMembers_IsActive] DEFAULT(1),

    CONSTRAINT [PK_OrganizationMembers] PRIMARY KEY ([OrganizationId], [UserId]),
    CONSTRAINT [FK_OrgMembers_Org]  FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations]([OrganizationId]),
    CONSTRAINT [FK_OrgMembers_User] FOREIGN KEY ([UserId])         REFERENCES [dbo].[Users]([UserId])
);
GO

CREATE TABLE [dbo].[Projects]
(
    [ProjectId]       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_Projects] PRIMARY KEY,
    [OrganizationId]  UNIQUEIDENTIFIER NOT NULL,
    [Name]            NVARCHAR(200) NOT NULL,
    [KeyCode]         NVARCHAR(20) NOT NULL,
    [Description]     NVARCHAR(MAX) NULL,
    [ProjectType]     NVARCHAR(30) NOT NULL CONSTRAINT [DF_Projects_Type] DEFAULT(N'Software'),
    [IsArchived]      BIT NOT NULL CONSTRAINT [DF_Projects_IsArchived] DEFAULT(0),
    [IsDeleted]       BIT NOT NULL CONSTRAINT [DF_Projects_IsDeleted] DEFAULT(0),
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Projects_CreatedAt] DEFAULT SYSUTCDATETIME(),
    [UpdatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Projects_UpdatedAt] DEFAULT SYSUTCDATETIME(),
    [RowVer]          ROWVERSION NOT NULL,

    CONSTRAINT [FK_Projects_Org] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations]([OrganizationId])
);
GO

CREATE TABLE [dbo].[ProjectMembers]
(
    [ProjectId]       UNIQUEIDENTIFIER NOT NULL,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [ProjectRole]     NVARCHAR(50) NOT NULL CONSTRAINT [DF_ProjectMembers_Role] DEFAULT(N'Developer'),
    [AddedAt]         DATETIME2(3) NOT NULL CONSTRAINT [DF_ProjectMembers_AddedAt] DEFAULT SYSUTCDATETIME(),
    [IsActive]        BIT NOT NULL CONSTRAINT [DF_ProjectMembers_IsActive] DEFAULT(1),

    CONSTRAINT [PK_ProjectMembers] PRIMARY KEY ([ProjectId], [UserId]),
    CONSTRAINT [FK_ProjectMembers_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects]([ProjectId]),
    CONSTRAINT [FK_ProjectMembers_User]    FOREIGN KEY ([UserId])    REFERENCES [dbo].[Users]([UserId])
);
GO

CREATE TABLE [dbo].[Permissions]
(
    [PermissionKey]   NVARCHAR(100) NOT NULL CONSTRAINT [PK_Permissions] PRIMARY KEY,
    [Description]     NVARCHAR(400) NOT NULL
);
GO

CREATE TABLE [dbo].[RolePermissions]
(
    [Scope]           NVARCHAR(20) NOT NULL,
    [RoleName]        NVARCHAR(50) NOT NULL,
    [PermissionKey]   NVARCHAR(100) NOT NULL,

    CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([Scope], [RoleName], [PermissionKey]),
    CONSTRAINT [FK_RolePermissions_Permission] FOREIGN KEY ([PermissionKey]) REFERENCES [dbo].[Permissions]([PermissionKey])
);
GO

CREATE TABLE [dbo].[Boards]
(
    [BoardId]         UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_Boards] PRIMARY KEY,
    [ProjectId]       UNIQUEIDENTIFIER NOT NULL,
    [Name]            NVARCHAR(200) NOT NULL,
    [BoardType]       NVARCHAR(20) NOT NULL,
    [IsDefault]       BIT NOT NULL CONSTRAINT [DF_Boards_IsDefault] DEFAULT(0),
    [FilterJql]       NVARCHAR(2000) NULL,
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Boards_CreatedAt] DEFAULT SYSUTCDATETIME(),
    [UpdatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Boards_UpdatedAt] DEFAULT SYSUTCDATETIME(),
    [RowVer]          ROWVERSION NOT NULL,

    CONSTRAINT [FK_Boards_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects]([ProjectId])
);
GO

CREATE TABLE [dbo].[Sprints]
(
    [SprintId]        UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_Sprints] PRIMARY KEY,
    [BoardId]         UNIQUEIDENTIFIER NOT NULL,
    [Name]            NVARCHAR(200) NOT NULL,
    [Goal]            NVARCHAR(1000) NULL,
    [State]           NVARCHAR(20) NOT NULL CONSTRAINT [DF_Sprints_State] DEFAULT(N'Planned'),
    [StartDate]       DATETIME2(3) NULL,
    [EndDate]         DATETIME2(3) NULL,
    [CompleteDate]    DATETIME2(3) NULL,
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Sprints_CreatedAt] DEFAULT SYSUTCDATETIME(),
    [UpdatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Sprints_UpdatedAt] DEFAULT SYSUTCDATETIME(),
    [RowVer]          ROWVERSION NOT NULL,

    CONSTRAINT [FK_Sprints_Board] FOREIGN KEY ([BoardId]) REFERENCES [dbo].[Boards]([BoardId])
);
GO

CREATE TABLE [dbo].[IssueTypes]
(
    [IssueTypeId]     INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_IssueTypes] PRIMARY KEY,
    [Name]            NVARCHAR(50) NOT NULL,
    [IsSubTask]       BIT NOT NULL CONSTRAINT [DF_IssueTypes_IsSubTask] DEFAULT(0)
);
GO

CREATE TABLE [dbo].[Priorities]
(
    [PriorityId]      INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Priorities] PRIMARY KEY,
    [Name]            NVARCHAR(50) NOT NULL,
    [SortOrder]       INT NOT NULL
);
GO

CREATE TABLE [dbo].[Statuses]
(
    [StatusId]        INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Statuses] PRIMARY KEY,
    [Name]            NVARCHAR(50) NOT NULL,
    [Category]        NVARCHAR(20) NOT NULL,
    [SortOrder]       INT NOT NULL
);
GO

CREATE TABLE [dbo].[Workflows]
(
    [WorkflowId]      UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_Workflows] PRIMARY KEY,
    [ProjectId]       UNIQUEIDENTIFIER NOT NULL,
    [Name]            NVARCHAR(200) NOT NULL,
    [IsDefault]       BIT NOT NULL CONSTRAINT [DF_Workflows_IsDefault] DEFAULT(1),
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Workflows_CreatedAt] DEFAULT SYSUTCDATETIME(),
    [UpdatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Workflows_UpdatedAt] DEFAULT SYSUTCDATETIME(),
    [RowVer]          ROWVERSION NOT NULL,

    CONSTRAINT [FK_Workflows_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects]([ProjectId])
);
GO

CREATE TABLE [dbo].[WorkflowStatuses]
(
    [WorkflowId]      UNIQUEIDENTIFIER NOT NULL,
    [StatusId]        INT NOT NULL,
    [SortOrder]       INT NOT NULL,

    CONSTRAINT [PK_WorkflowStatuses] PRIMARY KEY ([WorkflowId], [StatusId]),
    CONSTRAINT [FK_WorkflowStatuses_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflows]([WorkflowId]),
    CONSTRAINT [FK_WorkflowStatuses_Status]   FOREIGN KEY ([StatusId])   REFERENCES [dbo].[Statuses]([StatusId])
);
GO

CREATE TABLE [dbo].[WorkflowTransitions]
(
    [WorkflowId]      UNIQUEIDENTIFIER NOT NULL,
    [FromStatusId]    INT NOT NULL,
    [ToStatusId]      INT NOT NULL,
    [Name]            NVARCHAR(100) NOT NULL,

    CONSTRAINT [PK_WorkflowTransitions] PRIMARY KEY ([WorkflowId], [FromStatusId], [ToStatusId]),
    CONSTRAINT [FK_WorkflowTransitions_Workflow] FOREIGN KEY ([WorkflowId])   REFERENCES [dbo].[Workflows]([WorkflowId]),
    CONSTRAINT [FK_WorkflowTransitions_From]     FOREIGN KEY ([FromStatusId]) REFERENCES [dbo].[Statuses]([StatusId]),
    CONSTRAINT [FK_WorkflowTransitions_To]       FOREIGN KEY ([ToStatusId])   REFERENCES [dbo].[Statuses]([StatusId])
);
GO

CREATE TABLE [dbo].[Issues]
(
    [IssueId]                 UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_Issues] PRIMARY KEY,
    [ProjectId]               UNIQUEIDENTIFIER NOT NULL,

    [IssueNo]                 INT NOT NULL,
    [ProjectKeySnapshot]      NVARCHAR(20) NOT NULL,
    [IssueKey]                AS (CONCAT([ProjectKeySnapshot], '-', CONVERT(VARCHAR(20), [IssueNo]))) PERSISTED,

    [IssueTypeId]             INT NOT NULL,
    [PriorityId]              INT NOT NULL,
    [StatusId]                INT NOT NULL,
    [WorkflowId]              UNIQUEIDENTIFIER NULL,

    [Summary]                 NVARCHAR(500) NOT NULL,
    [Description]             NVARCHAR(MAX) NULL,

    [ReporterId]              UNIQUEIDENTIFIER NOT NULL,
    [AssigneeId]              UNIQUEIDENTIFIER NULL,

    [ParentIssueId]           UNIQUEIDENTIFIER NULL,
    [EpicIssueId]             UNIQUEIDENTIFIER NULL,

    [BoardId]                 UNIQUEIDENTIFIER NULL,
    [SprintId]                UNIQUEIDENTIFIER NULL,
    [Rank]                    BIGINT NOT NULL CONSTRAINT [DF_Issues_Rank] DEFAULT(0),

    [OriginalEstimateMin]     INT NULL,
    [RemainingEstimateMin]    INT NULL,
    [TimeSpentMin]            INT NOT NULL CONSTRAINT [DF_Issues_TimeSpent] DEFAULT(0),

    [StoryPoints]             DECIMAL(10,2) NULL,
    [DueDate]                 DATE NULL,

    [Resolution]              NVARCHAR(50) NULL,
    [ResolvedAt]              DATETIME2(3) NULL,

    [IsDeleted]               BIT NOT NULL CONSTRAINT [DF_Issues_IsDeleted] DEFAULT(0),
    [CreatedAt]               DATETIME2(3) NOT NULL CONSTRAINT [DF_Issues_CreatedAt] DEFAULT SYSUTCDATETIME(),
    [UpdatedAt]               DATETIME2(3) NOT NULL CONSTRAINT [DF_Issues_UpdatedAt] DEFAULT SYSUTCDATETIME(),
    [RowVer]                  ROWVERSION NOT NULL,

    CONSTRAINT [FK_Issues_Project]   FOREIGN KEY ([ProjectId])   REFERENCES [dbo].[Projects]([ProjectId]),
    CONSTRAINT [FK_Issues_IssueType] FOREIGN KEY ([IssueTypeId]) REFERENCES [dbo].[IssueTypes]([IssueTypeId]),
    CONSTRAINT [FK_Issues_Priority]  FOREIGN KEY ([PriorityId])  REFERENCES [dbo].[Priorities]([PriorityId]),
    CONSTRAINT [FK_Issues_Status]    FOREIGN KEY ([StatusId])    REFERENCES [dbo].[Statuses]([StatusId]),
    CONSTRAINT [FK_Issues_Workflow]  FOREIGN KEY ([WorkflowId])  REFERENCES [dbo].[Workflows]([WorkflowId]),
    CONSTRAINT [FK_Issues_Reporter]  FOREIGN KEY ([ReporterId])  REFERENCES [dbo].[Users]([UserId]),
    CONSTRAINT [FK_Issues_Assignee]  FOREIGN KEY ([AssigneeId])  REFERENCES [dbo].[Users]([UserId]),
    CONSTRAINT [FK_Issues_Parent]    FOREIGN KEY ([ParentIssueId]) REFERENCES [dbo].[Issues]([IssueId]),
    CONSTRAINT [FK_Issues_Epic]      FOREIGN KEY ([EpicIssueId])   REFERENCES [dbo].[Issues]([IssueId]),
    CONSTRAINT [FK_Issues_Board]     FOREIGN KEY ([BoardId])     REFERENCES [dbo].[Boards]([BoardId]),
    CONSTRAINT [FK_Issues_Sprint]    FOREIGN KEY ([SprintId])    REFERENCES [dbo].[Sprints]([SprintId])
);
GO

CREATE TABLE [dbo].[ProjectCounters]
(
    [ProjectId]       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_ProjectCounters] PRIMARY KEY,
    [NextIssueNo]     INT NOT NULL,
    [RowVer]          ROWVERSION NOT NULL,

    CONSTRAINT [FK_ProjectCounters_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects]([ProjectId])
);
GO

CREATE TABLE [dbo].[Labels]
(
    [LabelId]         UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_Labels] PRIMARY KEY,
    [OrganizationId]  UNIQUEIDENTIFIER NOT NULL,
    [Name]            NVARCHAR(100) NOT NULL,
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Labels_CreatedAt] DEFAULT SYSUTCDATETIME(),

    CONSTRAINT [FK_Labels_Org] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations]([OrganizationId])
);
GO

CREATE TABLE [dbo].[IssueLabels]
(
    [IssueId]         UNIQUEIDENTIFIER NOT NULL,
    [LabelId]         UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT [PK_IssueLabels] PRIMARY KEY ([IssueId], [LabelId]),
    CONSTRAINT [FK_IssueLabels_Issue] FOREIGN KEY ([IssueId]) REFERENCES [dbo].[Issues]([IssueId]),
    CONSTRAINT [FK_IssueLabels_Label] FOREIGN KEY ([LabelId]) REFERENCES [dbo].[Labels]([LabelId])
);
GO

CREATE TABLE [dbo].[IssueComments]
(
    [CommentId]       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_IssueComments] PRIMARY KEY,
    [IssueId]         UNIQUEIDENTIFIER NOT NULL,
    [AuthorId]        UNIQUEIDENTIFIER NOT NULL,
    [Body]            NVARCHAR(MAX) NOT NULL,
    [IsEdited]        BIT NOT NULL CONSTRAINT [DF_IssueComments_IsEdited] DEFAULT(0),
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_IssueComments_CreatedAt] DEFAULT SYSUTCDATETIME(),
    [UpdatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_IssueComments_UpdatedAt] DEFAULT SYSUTCDATETIME(),
    [RowVer]          ROWVERSION NOT NULL,

    CONSTRAINT [FK_IssueComments_Issue]  FOREIGN KEY ([IssueId])  REFERENCES [dbo].[Issues]([IssueId]),
    CONSTRAINT [FK_IssueComments_Author] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Users]([UserId])
);
GO

CREATE TABLE [dbo].[Attachments]
(
    [AttachmentId]    UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_Attachments] PRIMARY KEY,
    [IssueId]         UNIQUEIDENTIFIER NOT NULL,
    [UploadedById]    UNIQUEIDENTIFIER NOT NULL,
    [FileName]        NVARCHAR(260) NOT NULL,
    [ContentType]     NVARCHAR(200) NULL,
    [FileSizeBytes]   BIGINT NOT NULL,
    [StorageProvider] NVARCHAR(50) NOT NULL,
    [StorageKey]      NVARCHAR(1000) NOT NULL,
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Attachments_CreatedAt] DEFAULT SYSUTCDATETIME(),

    CONSTRAINT [FK_Attachments_Issue] FOREIGN KEY ([IssueId]) REFERENCES [dbo].[Issues]([IssueId]),
    CONSTRAINT [FK_Attachments_User]  FOREIGN KEY ([UploadedById]) REFERENCES [dbo].[Users]([UserId])
);
GO

CREATE TABLE [dbo].[IssueWatchers]
(
    [IssueId]         UNIQUEIDENTIFIER NOT NULL,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_IssueWatchers_CreatedAt] DEFAULT SYSUTCDATETIME(),

    CONSTRAINT [PK_IssueWatchers] PRIMARY KEY ([IssueId], [UserId]),
    CONSTRAINT [FK_IssueWatchers_Issue] FOREIGN KEY ([IssueId]) REFERENCES [dbo].[Issues]([IssueId]),
    CONSTRAINT [FK_IssueWatchers_User]  FOREIGN KEY ([UserId])  REFERENCES [dbo].[Users]([UserId])
);
GO

CREATE TABLE [dbo].[IssueLinks]
(
    [IssueLinkId]     UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_IssueLinks] PRIMARY KEY,
    [FromIssueId]     UNIQUEIDENTIFIER NOT NULL,
    [ToIssueId]       UNIQUEIDENTIFIER NOT NULL,
    [LinkType]        NVARCHAR(50) NOT NULL,
    [CreatedById]     UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_IssueLinks_CreatedAt] DEFAULT SYSUTCDATETIME(),

    CONSTRAINT [FK_IssueLinks_From] FOREIGN KEY ([FromIssueId]) REFERENCES [dbo].[Issues]([IssueId]),
    CONSTRAINT [FK_IssueLinks_To]   FOREIGN KEY ([ToIssueId])   REFERENCES [dbo].[Issues]([IssueId]),
    CONSTRAINT [FK_IssueLinks_User] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[Users]([UserId])
);
GO

CREATE TABLE [dbo].[WorkLogs]
(
    [WorkLogId]       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_WorkLogs] PRIMARY KEY,
    [IssueId]         UNIQUEIDENTIFIER NOT NULL,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [TimeSpentMin]    INT NOT NULL,
    [StartedAt]       DATETIME2(3) NOT NULL,
    [Comment]         NVARCHAR(1000) NULL,
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_WorkLogs_CreatedAt] DEFAULT SYSUTCDATETIME(),

    CONSTRAINT [FK_WorkLogs_Issue] FOREIGN KEY ([IssueId]) REFERENCES [dbo].[Issues]([IssueId]),
    CONSTRAINT [FK_WorkLogs_User]  FOREIGN KEY ([UserId])  REFERENCES [dbo].[Users]([UserId])
);
GO

CREATE TABLE [dbo].[CustomFields]
(
    [CustomFieldId]   UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_CustomFields] PRIMARY KEY,
    [ProjectId]       UNIQUEIDENTIFIER NOT NULL,
    [Name]            NVARCHAR(200) NOT NULL,
    [FieldType]       NVARCHAR(30) NOT NULL,
    [IsRequired]      BIT NOT NULL CONSTRAINT [DF_CustomFields_IsRequired] DEFAULT(0),
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_CustomFields_CreatedAt] DEFAULT SYSUTCDATETIME(),

    CONSTRAINT [FK_CustomFields_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects]([ProjectId])
);
GO

CREATE TABLE [dbo].[CustomFieldOptions]
(
    [OptionId]        UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_CustomFieldOptions] PRIMARY KEY,
    [CustomFieldId]   UNIQUEIDENTIFIER NOT NULL,
    [Value]           NVARCHAR(200) NOT NULL,
    [SortOrder]       INT NOT NULL,

    CONSTRAINT [FK_CustomFieldOptions_Field] FOREIGN KEY ([CustomFieldId]) REFERENCES [dbo].[CustomFields]([CustomFieldId])
);
GO

CREATE TABLE [dbo].[IssueCustomFieldValues]
(
    [IssueId]         UNIQUEIDENTIFIER NOT NULL,
    [CustomFieldId]   UNIQUEIDENTIFIER NOT NULL,

    [ValueText]       NVARCHAR(MAX) NULL,
    [ValueNumber]     DECIMAL(18,4) NULL,
    [ValueDate]       DATE NULL,
    [ValueBool]       BIT NULL,
    [ValueOptionId]   UNIQUEIDENTIFIER NULL,
    [ValueJson]       NVARCHAR(MAX) NULL,

    [UpdatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_IssueCFV_UpdatedAt] DEFAULT SYSUTCDATETIME(),

    CONSTRAINT [PK_IssueCustomFieldValues] PRIMARY KEY ([IssueId], [CustomFieldId]),
    CONSTRAINT [FK_IssueCFV_Issue]  FOREIGN KEY ([IssueId])       REFERENCES [dbo].[Issues]([IssueId]),
    CONSTRAINT [FK_IssueCFV_Field]  FOREIGN KEY ([CustomFieldId]) REFERENCES [dbo].[CustomFields]([CustomFieldId]),
    CONSTRAINT [FK_IssueCFV_Option] FOREIGN KEY ([ValueOptionId]) REFERENCES [dbo].[CustomFieldOptions]([OptionId])
);
GO

CREATE TABLE [dbo].[Notifications]
(
    [NotificationId]  UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_Notifications] PRIMARY KEY,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [OrganizationId]  UNIQUEIDENTIFIER NULL,
    [ProjectId]       UNIQUEIDENTIFIER NULL,
    [IssueId]         UNIQUEIDENTIFIER NULL,
    [Type]            NVARCHAR(50) NOT NULL,
    [PayloadJson]     NVARCHAR(MAX) NOT NULL,
    [IsRead]          BIT NOT NULL CONSTRAINT [DF_Notifications_IsRead] DEFAULT(0),
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_Notifications_CreatedAt] DEFAULT SYSUTCDATETIME(),

    CONSTRAINT [FK_Notifications_User]    FOREIGN KEY ([UserId])         REFERENCES [dbo].[Users]([UserId]),
    CONSTRAINT [FK_Notifications_Org]     FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations]([OrganizationId]),
    CONSTRAINT [FK_Notifications_Project] FOREIGN KEY ([ProjectId])      REFERENCES [dbo].[Projects]([ProjectId]),
    CONSTRAINT [FK_Notifications_Issue]   FOREIGN KEY ([IssueId])        REFERENCES [dbo].[Issues]([IssueId])
);
GO

CREATE TABLE [dbo].[AuditLogs]
(
    [AuditLogId]      UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_AuditLogs] PRIMARY KEY,
    [OrganizationId]  UNIQUEIDENTIFIER NULL,
    [ProjectId]       UNIQUEIDENTIFIER NULL,
    [ActorUserId]     UNIQUEIDENTIFIER NULL,
    [EntityType]      NVARCHAR(50) NOT NULL,
    [EntityId]        UNIQUEIDENTIFIER NULL,
    [Action]          NVARCHAR(50) NOT NULL,
    [DataJson]        NVARCHAR(MAX) NULL,
    [CreatedAt]       DATETIME2(3) NOT NULL CONSTRAINT [DF_AuditLogs_CreatedAt] DEFAULT SYSUTCDATETIME(),

    CONSTRAINT [FK_AuditLogs_Org]     FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations]([OrganizationId]),
    CONSTRAINT [FK_AuditLogs_Project] FOREIGN KEY ([ProjectId])      REFERENCES [dbo].[Projects]([ProjectId]),
    CONSTRAINT [FK_AuditLogs_Actor]   FOREIGN KEY ([ActorUserId])    REFERENCES [dbo].[Users]([UserId])
);
GO

ALTER TABLE dbo.Users
ADD IsSystemAdmin BIT NOT NULL CONSTRAINT DF_Users_IsSystemAdmin DEFAULT(0);