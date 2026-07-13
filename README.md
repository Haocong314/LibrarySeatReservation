# 图书馆座位预约系统 (Library Seat Reservation)

## 项目简介

一个基于 ASP.NET Core MVC (.NET 8) 的图书馆座位预约 Web 应用。面向高校图书馆场景，用户端支持浏览座位、查看时段占用、提交预约、查看/取消个人预约；管理端支持筛选查询预约、管理座位状态、查看预约统计。

本项目为软件专业课程第三阶段实训项目，模拟"入职后独立负责第一个小项目"的真实工作过程。

## 技术栈

| 层级 | 技术 |
|------|------|
| 后端框架 | ASP.NET Core MVC (.NET 8) |
| 视图引擎 | Razor Pages / Views |
| ORM | Entity Framework Core 8 |
| 数据库 | SQL Server LocalDB |
| 前端样式 | Bootstrap 5 + 自定义 CSS |
| 前端脚本 | 原生 JavaScript（无框架） |
| 用户端登录 | 体验账号下拉切换（无 Session） |
| 管理端登录 | Session / Cookie 轻量登录 |
| 版本控制 | Git + GitHub / Gitee |

## 目录结构

### 当前已存在 / 本阶段产物

```
LibrarySeatReservation/
│
├── README.md                            # 本文件
├── docs/                                # 全套设计文档
│   ├── 01-项目立项单.md
│   ├── 01-项目立项单-审计.md
│   ├── 02-需求分析与MVP确认.md
│   ├── 02-需求分析与MVP确认-审计.md
│   ├── 03-PRD-Lite.md
│   ├── 03-PRD-Lite-审计.md
│   ├── 04-页面树与业务流程.md
│   ├── 04-页面树与业务流程-审计.md
│   ├── 05-页面卡与UI规范.md
│   ├── 05-页面卡与UI规范-审计.md
│   ├── 06-静态原型与原型评审.md
│   ├── 06-静态原型与原型评审-审计.md
│   ├── 07-系统设计说明.md
│   ├── 07-系统设计说明-审计.md
│   ├── 08-数据库设计.md
│   ├── 08-数据库设计-审计.md
│   ├── 09-关键链路详细设计.md
│   ├── 09-关键链路详细设计-审计.md
│   ├── 10-开发准备与Sprint0.md
│   ├── 10-开发准备与Sprint0-审计.md
│   └── 项目任务板与迭代记录.md
│
├── prototype/                           # 静态原型
│   ├── static-v1/                       # 第一版原型（9 个 HTML 页面）
│   │   ├── index.html                   # P01 用户首页
│   │   ├── seat-list.html               # P02 座位列表页
│   │   ├── seat-detail.html             # P03 座位详情页
│   │   ├── reservation-create.html      # P04 预约提交页
│   │   ├── my-reservations.html         # P05 我的预约页
│   │   ├── admin-login.html             # P06 管理员登录页
│   │   ├── admin-reservations.html      # P07 预约管理页
│   │   ├── admin-seats.html             # P08 座位管理页
│   │   ├── admin-stats.html             # P09 统计页
│   │   └── css/
│   │       └── style.css                # 自定义样式文件
│   └── review-1/
│       └── 原型评审清单.md
```

### 后续计划 / 待生成项

```
LibrarySeatReservation/
│
├── src/                                 # 源代码（下一阶段生成）
│   └── LibrarySeatReservation.sln       # 解决方案文件
│   └── LibrarySeatReservation.Web/      # Web 项目
│       ├── LibrarySeatReservation.Web.csproj
│       ├── Program.cs
│       ├── Controllers/
│       ├── Models/
│       │   ├── Entities/
│       │   └── ViewModels/
│       ├── Services/
│       ├── Data/
│       │   └── AppDbContext.cs
│       ├── Views/
│       │   ├── Home/
│       │   ├── Seat/
│       │   ├── Reservation/
│       │   ├── Admin/
│       │   ├── AdminManage/
│       │   └── Shared/
│       ├── Migrations/
│       └── wwwroot/
│           ├── css/
│           ├── js/
│           └── lib/
```

## 运行前提

| 软件 | 版本要求 | 说明 |
|------|----------|------|
| .NET SDK | 8.0 或更高 | [下载页面](https://dotnet.microsoft.com/download) |
| SQL Server LocalDB | 随 Visual Studio 安装 | 或单独安装 [SQL Server Express LocalDB](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) |
| 浏览器 | Chrome / Edge / Firefox 最新版 | 用户端建议用手机模式（F12 → Toggle Device Toolbar）查看 |
| Git | 2.40+ | 用于版本管理（可选，但不装也能运行项目） |

## 快速开始（待 Sprint 0 完成后补充）

```bash
# 1. 进入源代码目录
cd src/LibrarySeatReservation.Web

# 2. 还原 NuGet 包
dotnet restore

# 3. 创建数据库并执行迁移
dotnet ef database update

# 4. 启动项目
dotnet run

# 5. 浏览器打开
# 用户端：http://localhost:5000
# 管理端：http://localhost:5000/Admin/Login
```

## 当前阶段

**阶段 14a：管理端、权限与状态回流开发（Sprint 2）** — AdminService（登录验证/预约筛选/取消预约/座位管理/数据统计）、AdminController（登录/登出含 Session 守卫）、AdminManageController（EnsureAdmin 权限守护）、管理端 4 视图（P06-P09）、侧边栏布局（_AdminLayout）、状态回流（用户端即时可见管理端变更）。代码已对齐文档，待 .NET SDK 环境完成 T02-08 端到端走查后进入 14b 审计。

## 已实现范围（待各 Sprint 完成后更新）

| Sprint | 范围 | 状态 |
|--------|------|------|
| Sprint 0 | 工程骨架搭建 | 🏁 已完成（阶段 12a） |
| Sprint 1 | 用户端 5 页 + 预约核心链路 | ✅ 待验证（阶段 13a，代码已对齐） |
| Sprint 2 | 管理端 4 页 + 管理核心链路 | ✅ 待验证（阶段 14a，代码已对齐） |
| Sprint 3 | 完善与Bug修复 | 📋 待开始 |
| Sprint 4 | 最终交付 | 📋 待开始 |

## 数据库初始化方式

使用 EF Core Code First Migrations：

1. `dotnet ef migrations add InitialCreate` — 生成迁移文件
2. `dotnet ef database update` — 执行迁移，自动建库建表
3. 种子数据（体验用户、管理员、座位区域、座位）通过 `DbInitializer.Seed()` 在应用启动时自动插入

## 演示账号

| 角色 | 账号 | 说明 |
|------|------|------|
| 体验用户 | 2024001 / 2024002 / 2024003 | 在用户首页通过下拉框选择切换 |
| 管理员 | admin / admin123 | 在 P06 管理员登录页输入 |

## 已知限制（待各 Sprint 完成后更新）

- 用户端不实现注册/登录，通过体验账号下拉切换模拟
- 本期不做支付、扫码签到、消息推送
- 预约时段固定为 7 个（08:00-22:00，每 2 小时一段）
- 管理端仅 1 个管理员账号（种子数据）
- 不做多馆区 / 多图书馆联动
- 无邮件 / 短信通知
