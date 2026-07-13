# 图书馆座位预约系统 — 开发准备与 Sprint 0

## 1. 仓库结构

```
LibrarySeatReservation/
├── README.md
├── docs/                     # 设计文档（01-10 全套）
├── prototype/                # 静态原型（static-v1/）
│   └── review-1/             # 原型评审清单
└── src/                      # （Sprint 0 阶段创建，Sprint 1-4 填充代码）
    ├── LibrarySeatReservation.sln
    └── LibrarySeatReservation.Web/
        ├── Controllers/
        ├── Models/
        │   ├── Entities/
        │   └── ViewModels/
        ├── Services/
        ├── Data/
        ├── Views/
        ├── Migrations/
        └── wwwroot/
```

## 2. 分支策略

| 分支 | 用途 | 说明 |
|------|------|------|
| `main` | 稳定发布分支 | 每个里程碑完成后从 `dev` 合并 |
| `dev` | 开发主线 | Sprint 内部多轮推进在此分支进行 |
| `feat/xxx` | 功能分支 | 单人项目可选，复杂功能（如 db-migration / admin-panel）切出开发后合并回 `dev` |

**当前状态**：仓库位于 `dev` 分支（首次提交）。

## 3. 提交规范

采用简易 Conventional Commits：

```
<type>: <简短描述>

类型：
  docs     — 文档变更
  feat     — 新功能
  fix      — Bug 修复
  chore    — 工程配置变更
  style    — UI / 样式调整
  refactor — 代码重构（不改功能）

示例：
  feat: 完成座位列表页 Razor View + Controller
  docs: 更新 README 已实现范围
  fix: 修复预约冲突检查时未过滤已取消记录
```

## 4. Sprint 0 目标

**目标**：完成项目工程骨架搭建，让项目可以 `dotnet build` 和 `dotnet run`，并能通过 EF Core Migrations 自动建库建表并插入种子数据。

**Sprint 0 不属于功能开发**，它的交付物是：
- `.sln` 解决方案文件
- `.csproj` 项目文件（含 NuGet 包引用）
- `Program.cs` 最小启动配置
- `AppDbContext` + Entity 类 + Fluent API 配置（唯一索引、外键）
- 首次 `dotnet ef migrations add InitialCreate`
- 首次 `dotnet ef database update`（建库建表）
- `DbInitializer.Seed()` 种子数据（3 用户 + 1 管理员 + 2 区域 + 7 座位）

## 5. Sprint 1-4 主 Sprint 粗计划

> 每个主 Sprint 内允许多轮推进。每轮完成一部分任务卡后评审，通过后再推进下一轮。

### Sprint 1：用户端核心功能（主 Sprint，可多轮推进）

- **目标**：完成用户端 5 页面（P01-P05）的 Razor View 和 Controller，实现用户预约主链路
- **最低完成线**：P01→P02→P03→P04→P05 页面全部可运行；能在 P04 成功提交预约并跳转至 P05 查看
- **涉及 Controller**：HomeController、SeatController、ReservationController
- **涉及 Service**：ISeatService / SeatService、IReservationService / ReservationService

### Sprint 2：管理端核心功能（主 Sprint，可多轮推进）

- **目标**：完成管理端 4 页面（P06-P09）的 Razor View 和 Controller，实现管理员取消预约链路
- **最低完成线**：P06 可登录 → P07 可筛选并取消预约 → P08 可启用/停用座位 → P09 显示统计
- **涉及 Controller**：AdminController、AdminManageController
- **涉及 Service**：IAdminService / AdminService

### Sprint 3：完善与 Bug 修复（主 Sprint，可多轮推进）

- **目标**：全链路端到端测试、边角场景修复、UI 一致性微调、响应式适配确认
- **最低完成线**：用户链路和管理链路各自完成 3 轮完整走查无阻塞性 Bug

### Sprint 4：最终交付（主 Sprint，可多轮推进）

- **目标**：演示准备（README 完善、截图、录制路径说明）、已知限制文档化、代码清理、最终 `git tag` 打标
- **最低完成线**：可在全新机器上按 README 步骤跑通全流程

## 6. 里程碑节点

| 编号 | 里程碑 | 标志事件 | 预计覆盖 Sprint |
|------|--------|----------|----------------|
| M1 | 设计文档与原型完成 | 01-10 全套文档 + 静态原型 v1 通过审计 | 阶段 01-10 |
| M2 | 项目骨架与数据库就绪 | `.sln` 可 build、数据库可 migrate、种子数据可查 | Sprint 0 |
| M3 | 核心功能开发完成 | 用户端 + 管理端 9 页面全部可运行、主链路闭环 | Sprint 1-2 |
| M4 | 全部功能完成与演示交付 | 全链路无阻塞 Bug、README 完整、可演示 | Sprint 3-4 |

## 7. 默认补足项 / 当前假设

| 补足项 | 假设 | 理由 |
|--------|------|------|
| 分支策略 | 单人项目简化：仅 `dev` 分支 + 必要时 `feat/xxx`，`main` 在 M4 时合并 | 前序文档未指定多分支协同策略 |
| 提交规范 | 简易 Conventional Commits（type + 中文描述） | 学生友好，不引入 commitlint |
| Sprint 编号从 0 开始 | Sprint 0 为工程准备（不写功能代码），Sprint 1-4 为功能 Sprint | 与 09-关键链路设计的"后续代码实现衔接点"对齐 |
| GitHub/Gitee 远端地址 | **待补**（学生需自行在 GitHub/Gitee 创建仓库后填入） | 10a 提示词要求先填空 |
| 首次 git push 远端 | **待补**（待远端仓库地址提供后执行 `git remote add origin <url> && git push -u origin dev`） | 本地已执行 `git init` + `git add` + `git commit` |

## GitHub 或 Gitee 仓库链接

- 【GitHub 仓库链接】：https://github.com/Haocong314/LibrarySeatReservation
**待补** — 请学生在 GitHub/Gitee 创建仓库后将链接填入此处，并根据下方命令完成远端推送：

```bash
git remote add origin <仓库地址>
git push -u origin dev
```
