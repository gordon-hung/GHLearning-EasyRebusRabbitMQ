# AGENTS.md

## Project Overview (專案概述)

本專案是一個採用 **Domain-Driven Design (DDD)** 的 Notification 系統。

主要目標：
- 構建高內聚、低耦合、可測試的企業級應用
- 提供 Notification 建立、儲存與查詢功能
- 支援事件通知機制（Rebus / RabbitMQ）

設計原則：
- Domain 與 Persistence 完全分離
- Application Interface / Implementation 明確分層
- 依賴方向由外向內（Presentation → Application → Domain）

---

## Tech Stack (技術堆疊)

- **Framework**: .NET 10 SDK (ASP.NET Core Web API)
- **Language**: C# 14
- **Database**: SQLite
- **ORM**: Entity Framework Core
- **Object Mapping**: AutoMapper
- **Validation**: FluentValidation
- **Testing**: xUnit, NSubstitute
- **Events**: Rebus, Rebus.RabbitMQ

---

## Architecture & Directory Structure (架構與目錄結構)

本專案遵循 **DDD 四層 + Clean Architecture**，依賴方向由外向內（Presentation → Application → Domain → Infrastructure）：

* `/src/Domain` (核心層 - 無依賴)

  * 包含 Entities, Value Objects, Aggregates, Domain Events, Repository Interfaces
  * **禁止**依賴任何外部庫或基礎設施
  * **Persistence 與 Domain 分離**

* `/src/Application` (應用層)

  * 包含 DTOs, Validators, Service Interfaces 與 Implementations
  * 結構：

    ```
    Application/
    ├─ Abstractions/Services   # Service Interface (e.g., INotificationService)
    ├─ Services                # Service Implementation (e.g., NotificationService)
    ├─ DTOs
    └─ Validators
    ```
  * 負責協調 Domain Entity 完成業務邏輯

* `/src/Infrastructure` (基礎設施層)

  * 包含 EF Core DbContext, Persistence Entity, Repositories 實作, Event Bus / External API Clients
  * 負責 Domain ↔ Persistence 轉換及資料存取
  * **Domain 不依賴 Infrastructure**

* `/src/WebApi` (表現層)

  * 包含 Controllers, Middleware, Filters, Program.cs
  * 僅負責接收 HTTP 請求並呼叫 Application Service
  * **不包含業務邏輯**

* `/src/SharedKernel` (共享內核)

  * 包含 AggregateRoot, IDomainEvent, 共用 Value Object / Interface
  * 方便不同 Domain 或專案間共用

---

## Coding Conventions (編碼規範)

* **Naming**:

  * Class, Method, Property 使用 `PascalCase` (e.g., `NotificationService`, `GetByIdAsync`)
  * Private Field 使用 `_camelCase` (e.g., `_repository`)
  * Local Variable 使用 `camelCase`
  * Interface 必須以 `I` 開頭 (e.g., `INotificationService`)

* **Async**:

  * 所有 I/O 操作必須使用 `async/await`
  * Method 名稱必須以 `Async` 結尾 (e.g., `SendNotificationAsync`)

* **Dependency Injection**:

  * 優先使用 Constructor Injection
  * Application Interface / Implementation 分層，Controller 僅依賴 Interface

* **Entity Guidelines**:

  * Domain Entity 屬性應設為 `private set`，透過 Method 修改狀態
  * Domain 不依賴 EF Core / Data Annotations
  * Persistence Entity 專門給 Infrastructure 使用，透過 Repository 做 Domain ↔ Persistence 轉換
  * EF Core 配置使用 Fluent API

---

## Testing Protocols (測試規範)

### Allowed Testing Frameworks（允許的測試框架）

本專案 **僅允許** 使用以下測試相關套件：

- **Test Framework**：xUnit
- **Mocking Framework**：NSubstitute

#### Required

- 所有測試專案 **必須** 引用：
  - `xunit`
  - `xunit.runner.visualstudio`
  - `Microsoft.NET.Test.Sdk`
  - `NSubstitute`
- 所有 Mock / Stub / Spy 行為 **必須** 使用 `NSubstitute`

#### Forbidden

以下測試框架與套件 **嚴格禁止使用**：

- NUnit
- MSTest
- Moq
- FakeItEasy
- Rhino Mocks
- 任何未經核准的 Mock / Test Framework

#### Rules

- 禁止使用 `[Test]`、`[TestMethod]`、`[SetUp]` 等 NUnit / MSTest Attributes
- 僅允許使用 xUnit 提供之 `[Fact]`、`[Theory]`
- PR 若引入任何禁止套件，**必須退回，不得合併**

---

## Pull Request Guidelines (PR 指南)

* PR 標題請遵循 Conventional Commits (e.g., `feat(notification): implement send notification use case`)
* 確認沒有破壞 DDD 依賴原則（Domain 層不能引用 Infrastructure 層）
* Interface / Implementation 分層清楚
* PR 必須包含單元測試或更新測試