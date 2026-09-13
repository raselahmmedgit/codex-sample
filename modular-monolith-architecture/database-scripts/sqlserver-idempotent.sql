IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Addresses] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Line1] nvarchar(250) NOT NULL,
        [Line2] nvarchar(250) NULL,
        [City] nvarchar(100) NOT NULL,
        [State] nvarchar(max) NULL,
        [PostalCode] nvarchar(max) NULL,
        [Country] nvarchar(100) NOT NULL,
        [IsDefault] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [AuditLogs] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [Action] nvarchar(150) NOT NULL,
        [EntityName] nvarchar(150) NOT NULL,
        [EntityId] uniqueidentifier NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Brands] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Brands] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Carts] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Carts] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Categories] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [ParentCategoryId] uniqueidentifier NULL,
        [IsActive] bit NOT NULL,
        [SortOrder] int NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Coupons] (
        [Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(50) NOT NULL,
        [Type] nvarchar(30) NOT NULL,
        [Value] decimal(18,2) NOT NULL,
        [MinimumOrderAmount] decimal(18,2) NOT NULL,
        [MaximumDiscount] decimal(18,2) NULL,
        [StartsAtUtc] datetime2 NOT NULL,
        [ExpiresAtUtc] datetime2 NOT NULL,
        [UsageLimit] int NULL,
        [IsActive] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Coupons] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [CouponUsages] (
        [Id] uniqueidentifier NOT NULL,
        [CouponId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [OrderId] uniqueidentifier NOT NULL,
        [UsedAtUtc] datetime2 NOT NULL,
        CONSTRAINT [PK_CouponUsages] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Inventories] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [ReservedQuantity] int NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Inventories] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [InventoryTransactions] (
        [Id] uniqueidentifier NOT NULL,
        [InventoryId] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [Type] nvarchar(30) NOT NULL,
        [Reason] nvarchar(500) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        CONSTRAINT [PK_InventoryTransactions] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Orders] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [OrderNumber] nvarchar(50) NOT NULL,
        [Status] nvarchar(30) NOT NULL,
        [Subtotal] decimal(18,2) NOT NULL,
        [Discount] decimal(18,2) NOT NULL,
        [ShippingCost] decimal(18,2) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Payments] (
        [Id] uniqueidentifier NOT NULL,
        [OrderId] uniqueidentifier NOT NULL,
        [Provider] nvarchar(50) NOT NULL,
        [Status] nvarchar(30) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Payments] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [PaymentTransactions] (
        [Id] uniqueidentifier NOT NULL,
        [PaymentId] uniqueidentifier NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [Reference] nvarchar(150) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        CONSTRAINT [PK_PaymentTransactions] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] uniqueidentifier NOT NULL,
        [Sku] nvarchar(64) NOT NULL,
        [Name] nvarchar(250) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Price] decimal(18,2) NOT NULL,
        [Status] nvarchar(30) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [RefreshTokens] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [TokenHash] nvarchar(512) NOT NULL,
        [ExpiresAtUtc] datetime2 NOT NULL,
        [RevokedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Reviews] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Rating] int NOT NULL,
        [Comment] nvarchar(2000) NOT NULL,
        [ModerationStatus] nvarchar(30) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Reviews] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [Email] nvarchar(320) NOT NULL,
        [DisplayName] nvarchar(150) NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [Wishlists] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Wishlists] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [CartItems] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [UnitPrice] decimal(18,2) NOT NULL,
        [CartId] uniqueidentifier NULL,
        CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CartItems_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [OrderItems] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [ProductName] nvarchar(250) NOT NULL,
        [Quantity] int NOT NULL,
        [UnitPrice] decimal(18,2) NOT NULL,
        [OrderId] uniqueidentifier NULL,
        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [OrderStatusHistories] (
        [Id] uniqueidentifier NOT NULL,
        [Status] nvarchar(30) NOT NULL,
        [Reason] nvarchar(500) NOT NULL,
        [ChangedAtUtc] datetime2 NOT NULL,
        [OrderId] uniqueidentifier NULL,
        CONSTRAINT [PK_OrderStatusHistories] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderStatusHistories_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [ProductCategories] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [CategoryId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_ProductCategories] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductCategories_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [ProductImages] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [StorageKey] nvarchar(500) NOT NULL,
        [ContentType] nvarchar(100) NOT NULL,
        [IsPrimary] bit NOT NULL,
        CONSTRAINT [PK_ProductImages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductImages_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE TABLE [WishlistItems] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [WishlistId] uniqueidentifier NULL,
        CONSTRAINT [PK_WishlistItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_WishlistItems_Wishlists_WishlistId] FOREIGN KEY ([WishlistId]) REFERENCES [Wishlists] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Addresses_UserId_IsDefault] ON [Addresses] ([UserId], [IsDefault]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AuditLogs_EntityName_EntityId_CreatedAtUtc] ON [AuditLogs] ([EntityName], [EntityId], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Brands_Name] ON [Brands] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_CartItems_CartId_ProductId] ON [CartItems] ([CartId], [ProductId]) WHERE [CartId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Carts_UserId] ON [Carts] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Categories_ParentCategoryId_Name] ON [Categories] ([ParentCategoryId], [Name]) WHERE [ParentCategoryId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Coupons_Code] ON [Coupons] ([Code]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_CouponUsages_CouponId_UserId_OrderId] ON [CouponUsages] ([CouponId], [UserId], [OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Inventories_ProductId] ON [Inventories] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_InventoryTransactions_InventoryId_CreatedAtUtc] ON [InventoryTransactions] ([InventoryId], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Orders_OrderNumber] ON [Orders] ([OrderNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Orders_UserId_Status_CreatedAtUtc] ON [Orders] ([UserId], [Status], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_OrderStatusHistories_OrderId_ChangedAtUtc] ON [OrderStatusHistories] ([OrderId], [ChangedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Payments_OrderId] ON [Payments] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PaymentTransactions_Reference] ON [PaymentTransactions] ([Reference]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ProductCategories_ProductId_CategoryId] ON [ProductCategories] ([ProductId], [CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductImages_ProductId_IsPrimary] ON [ProductImages] ([ProductId], [IsPrimary]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Products_Name] ON [Products] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Products_Sku] ON [Products] ([Sku]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_RefreshTokens_TokenHash] ON [RefreshTokens] ([TokenHash]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RefreshTokens_UserId_ExpiresAtUtc] ON [RefreshTokens] ([UserId], [ExpiresAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reviews_ProductId] ON [Reviews] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Reviews_ProductId_UserId] ON [Reviews] ([ProductId], [UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_WishlistItems_WishlistId_ProductId] ON [WishlistItems] ([WishlistId], [ProductId]) WHERE [WishlistId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Wishlists_UserId] ON [Wishlists] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913064605_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260913064605_InitialCreate', N'9.0.9');
END;

COMMIT;
GO

