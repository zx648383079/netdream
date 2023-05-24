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