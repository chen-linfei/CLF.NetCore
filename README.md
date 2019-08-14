# CLF.NetCore
### 加入群聊：深圳.Net Core探索群：794318601

### 框架介绍
CLF.NetCore是一个拿来即用的B/S开源基础框架，采用DDD思想设计框架，以Asp.net core Mvc， EF Core为主要技术，易上手，易扩展，可适用于中小型项目。它已经帮你搭建好了一个基础的.NET Core框架，包括前端（AdminLTE，基于BootStrap），Download项目后可以直接开展业务逻辑，写增删改查代码，不管是.NET新手还是老鸟都能轻松上手。

### 项目架构分层
* 基础架构层
* 领域模型层
* 应用服务层（service层）
* Web展示层（mvc层）
* WebAPI层
* 测试项目

### 项目采用技术
* Asp.net Mvc Core
* EF Core（CodeFirst）
* Asp.net WebApi
* Autofac
* AutoMapper
* Serilog
* Redis
* IdentityServer 4（未完待续）
* 消息队列（未完待续）
* Vuejs（单页版web层，未完待续）
* AdminLTE(基于BootStrap)

### 如何快速运行项目
* 1:下载项目后，还原Nuget，打开测试项目（6.Test->CLF.Core.Test）,更改appsettings.json配置文件的数据库节点：SqlServerConnectionString，然后运行
DatabaseInitializerTest类中的InitAcccountDatabase方法，即可自动在你本地生成数据库。
* 2:打开CLF.Web.Mvc项目根目录下面的appsettings.json配置文件，更改数据库连接，然后F5，就可以跑起来了，是不是超级简单，哈哈。

### 如何快速创建一个领域模型
* 详细描述见项目文档

