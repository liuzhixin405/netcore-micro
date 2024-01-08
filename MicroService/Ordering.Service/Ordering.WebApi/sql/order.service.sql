/*
找不到数据库级别的扩展属性，或所有现有扩展属性在其他窗口中处于打开状态
*/CREATE TABLE MOrder (
    Id BIGINT PRIMARY KEY,
    UserId BIGINT NOT NULL,
    ProductId BIGINT NOT NULL,
    Quantity INT NOT NULL,
    TotalAmount DECIMAL(18, 2) NOT NULL,
    OrderStatus INT NOT NULL,
    CreateTime BIGINT NOT NULL,
    -- 添加适当的外键约束，参考具体的数据库关系
);
CREATE TABLE OutBoxMessage (
    Id BIGINT PRIMARY KEY,
    Type VARCHAR(255), -- You may adjust the length based on your needs
    Content TEXT,      -- TEXT is used for potentially large content, adjust if needed
    OccurredOnUtc DATETIME,
    ProceddedOnUtc DATETIME,
    Error VARCHAR(MAX) -- Adjust the length based on your needs, or use TEXT if it can be large
);