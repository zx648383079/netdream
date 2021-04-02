# netdream
.net core for zodream

## 开发目标

`NetDream.Web` 这个只开发前台博客、聊天室（websoket）、代码生成器。

`NetDream.Razor` 只开发一个前台博客

`NetDream.Api` 这个为主，全部适配 [Angular-Zodream](https://github.com/zx648383079/Angular-ZoDream) 项目，作为 NET Core 版



```cmd
dotnet new webApp -o netdream --no-https

cd src

dotnet run
```