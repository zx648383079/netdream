# netdream
.net core for zodream

## 开发目标

`NetDream.Web` 这个只开发前台博客、聊天室（websoket）、代码生成器。

`NetDream.Razor` 只开发一个前台博客

`NetDream.Api` 这个为主，全部适配 [Angular-Zodream](https://github.com/zx648383079/Angular-ZoDream) 项目，作为 NET Core 版

`NetDream.Cli` 控制台生成一些代码。

`NetDream.Core` 核心代码，规范接口，提供公共方法

`NetDream.Tests` 测试代码

## 模块

基本命名空间 `NetDream.Modules.{模块名}`

每个模块实现一些 `CRUD` 等关键逻辑代码，不应该涉及界面UI、前台响应等功能



```cmd
dotnet new webApp -o netdream --no-https

cd src

dotnet run
```

## 使用规范

### 文件夹规范

每个模块


> Entities 定义数据表的字段
> Migrations 定义数据表创建字段相关信息
> Models 定义跟前端交互的数据结构
> Repositories 定义翻译查询数据库的数据
> Forms 定义接收前端传的参数


### 命名规范

1. 数据表对应的实体 `<表名>Entity`，放在 `Entities` 文件夹中
2. 创建数据表的 `<表名>EntityTypeConfiguration`，放在 `Migrations`
3. 定义当前模块连接数据库实体 `<模块>Context`，放在模块根目录
4. 对数据库进行读写的操作 `<>Repository`，放在 `Repositories`
5. 注册服务包括 `Repository` 全写在模块根目录下 `Extension.Provide<模块>Repositories` 拓展`IServiceCollection`的方法中
6. 响应列表数据 `<>ListItem`，每一项数据可能带一些标签，转成 `ListLabelItem{Id, Name}`
7. 响应单篇完整数据 `<>Model`
8. 批量处理接口，接收用 `<模块>BatchForm`，响应用 `<模块>BatchResult`