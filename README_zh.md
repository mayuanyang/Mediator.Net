# Mediator.Net

[![Stack Overflow](https://img.shields.io/badge/stack%20overflow-Mediator.Net-yellowgreen.svg)](http://stackoverflow.com/questions/tagged/memdiator.net)
[![构建状态](https://ci.appveyor.com/api/projects/status/j42okw862yjgdeo9?svg=true)](https://ci.appveyor.com/project/mayuanyang/mediator-net)
[![CI](https://github.com/mayuanyang/Mediator.Net/actions/workflows/ci.yml/badge.svg)](https://github.com/mayuanyang/Mediator.Net/actions/workflows/ci.yml)
[![Release](https://github.com/mayuanyang/Mediator.Net/actions/workflows/release.yml/badge.svg)](https://github.com/mayuanyang/Mediator.Net/actions/workflows/release.yml)
[![codecov](https://codecov.io/gh/mayuanyang/Mediator.Net/branch/master/graph/badge.svg?token=MuQkMlLAcG)](https://codecov.io/gh/mayuanyang/Mediator.Net)
[![NuGet](https://img.shields.io/nuget/v/Mediator.Net.svg)](https://www.nuget.org/packages/Mediator.Net/)

一个强大而灵活的 .NET 中介者模式实现，通过解耦请求/响应处理来实现清洁架构。

<p align="center">
  <img src="https://cloud.githubusercontent.com/assets/3387099/24353370/97f573f0-1330-11e7-890c-85855628a575.png" alt="Mediator.Net Logo" width="200"/>
</p>

## 📋 目录

- [特性](#-特性)
- [安装](#-安装)
- [快速开始](#-快速开始)
  - [基本设置](#基本设置)
  - [定义消息和处理器](#定义消息和处理器)
- [使用示例](#-使用示例)
  - [发送命令](#发送命令)
  - [处理请求](#处理请求)
  - [发布事件](#发布事件)
  - [流式响应](#流式响应)
- [处理器注册](#-处理器注册)
  - [程序集扫描（推荐）](#程序集扫描推荐)
  - [显式注册](#显式注册)
- [管道和中间件](#-管道和中间件)
  - [管道类型](#管道类型)
  - [创建自定义中间件](#创建自定义中间件)
  - [配置管道](#配置管道)
- [依赖注入集成](#️-依赖注入集成)
  - [Microsoft.Extensions.DependencyInjection](#microsoftextensionsdependencyinjection)
  - [Autofac](#autofac)
  - [其他支持的容器](#其他支持的容器)
- [官方中间件包](#-官方中间件包)
  - [Serilog 日志](#serilog-日志)
  - [工作单元](#工作单元)
  - [EventStore 集成](#eventstore-集成)
- [高级特性](#-高级特性)
  - [上下文服务](#上下文服务)
  - [从处理器发布事件](#从处理器发布事件)
- [文档](#-文档)
- [贡献](#-贡献)
- [许可证](#-许可证)
- [支持](#️-支持)

## 🚀 特性

- **命令/查询分离**：命令、查询和事件的清晰分离
- **管道支持**：用于横切关注点的可扩展中间件管道
- **流式支持**：使用 `IAsyncEnumerable` 处理多个响应
- **依赖注入**：内置支持流行的 IoC 容器
- **事件发布**：从处理器内部发布事件
- **灵活注册**：支持显式注册和程序集扫描注册
- **中间件生态系统**：丰富的预构建中间件集合

## 📦 安装

通过 NuGet 安装主包：

```bash
Install-Package Mediator.Net
```

或通过 .NET CLI：

```bash
dotnet add package Mediator.Net
```

## 🏁 快速开始

### 基本设置

```csharp
// 创建和配置中介者
var mediaBuilder = new MediatorBuilder();
var mediator = mediaBuilder.RegisterHandlers(typeof(Program).Assembly).Build();
```

### 定义消息和处理器

```csharp
// 命令（无响应）
public class CreateUserCommand : ICommand
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    public async Task Handle(IReceiveContext<CreateUserCommand> context, CancellationToken cancellationToken)
    {
        // 处理命令
        var user = new User(context.Message.Name, context.Message.Email);
        // 保存用户...
        
        // 发布事件
        await context.Publish(new UserCreatedEvent { UserId = user.Id });
    }
}

// 请求/响应
public class GetUserQuery : IRequest<UserDto>
{
    public int UserId { get; set; }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    public async Task<UserDto> Handle(IReceiveContext<GetUserQuery> context, CancellationToken cancellationToken)
    {
        // 处理查询并返回响应
        return new UserDto { Id = context.Message.UserId, Name = "张三" };
    }
}

// 事件
public class UserCreatedEvent : IEvent
{
    public int UserId { get; set; }
}

public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    public async Task Handle(IReceiveContext<UserCreatedEvent> context, CancellationToken cancellationToken)
    {
        // 处理事件
        Console.WriteLine($"用户 {context.Message.UserId} 已创建！");
    }
}
```

## 📋 使用示例

### 发送命令

```csharp
// 无响应的命令
await mediator.SendAsync(new CreateUserCommand 
{ 
    Name = "张三", 
    Email = "zhangsan@example.com" 
});

// 有响应的命令
var result = await mediator.SendAsync<CreateUserCommand, CreateUserResponse>(
    new CreateUserCommand { Name = "李四", Email = "lisi@example.com" });
```

### 处理请求

```csharp
// 有响应的请求
var user = await mediator.RequestAsync<GetUserQuery, UserDto>(
    new GetUserQuery { UserId = 123 });
```

### 发布事件

```csharp
// 向所有处理器发布事件
await mediator.Publish(new UserCreatedEvent { UserId = 123 });
```

### 流式响应

创建返回多个响应的处理器：

```csharp
public class GetMultipleUsersStreamHandler : IStreamRequestHandler<GetUsersQuery, UserDto>
{
    public async IAsyncEnumerable<UserDto> Handle(
        IReceiveContext<GetUsersQuery> context, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        for (var i = 0; i < 10; i++)
        {
            await Task.Delay(100, cancellationToken);
            yield return new UserDto { Id = i, Name = $"用户 {i}" };
        }
    }
}

// 消费流
await foreach (var user in mediator.CreateStream<GetUsersQuery, UserDto>(new GetUsersQuery()))
{
    Console.WriteLine($"接收到：{user.Name}");
}
```

## 🔧 处理器注册

### 程序集扫描（推荐）

```csharp
var mediator = new MediatorBuilder()
    .RegisterHandlers(typeof(Program).Assembly)
    .Build();
```

### 显式注册

```csharp
var mediator = new MediatorBuilder()
    .RegisterHandlers(() => new List<MessageBinding>
    {
        new MessageBinding(typeof(CreateUserCommand), typeof(CreateUserCommandHandler)),
        new MessageBinding(typeof(GetUserQuery), typeof(GetUserQueryHandler)),
        new MessageBinding(typeof(UserCreatedEvent), typeof(UserCreatedEventHandler))
    })
    .Build();
```

## 🔄 管道和中间件

Mediator.Net 支持五种不同场景的管道类型：

![管道架构](https://cloud.githubusercontent.com/assets/3387099/21959127/9a065420-db09-11e6-8dbc-ca0069894e1c.png)

### 管道类型

| 管道 | 描述 | 触发对象 |
|----------|-------------|--------------|
| **GlobalReceivePipeline** | 对所有消息执行 | 命令、请求、事件 |
| **CommandReceivePipeline** | 仅对命令执行 | ICommand |
| **RequestReceivePipeline** | 仅对请求执行 | IRequest |
| **EventReceivePipeline** | 仅对事件执行 | IEvent |
| **PublishPipeline** | 当事件被发布时执行 | IEvent（出站） |

### 创建自定义中间件

#### 1. 创建中间件扩展

```csharp
public static class LoggingMiddleware
{
    public static void UseLogging<TContext>(
        this IPipeConfigurator<TContext> configurator, 
        ILogger logger = null)
        where TContext : IContext<IMessage>
    {
        logger ??= configurator.DependencyScope?.Resolve<ILogger>();
        configurator.AddPipeSpecification(new LoggingMiddlewareSpecification<TContext>(logger));
    }
}
```

#### 2. 创建中间件规范

```csharp
public class LoggingMiddlewareSpecification<TContext> : IPipeSpecification<TContext> 
    where TContext : IContext<IMessage>
{
    private readonly ILogger _logger;

    public LoggingMiddlewareSpecification(ILogger logger)
    {
        _logger = logger;
    }

    public bool ShouldExecute(TContext context, CancellationToken cancellationToken) => true;

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("正在处理消息：{MessageType}", context.Message.GetType().Name);
        return Task.CompletedTask;
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("完成处理：{MessageType}", context.Message.GetType().Name);
        return Task.CompletedTask;
    }

    public void OnException(Exception ex, TContext context)
    {
        _logger.LogError(ex, "处理消息时出错：{MessageType}", context.Message.GetType().Name);
        throw ex;
    }
}
```

### 配置管道

```csharp
var mediator = new MediatorBuilder()
    .RegisterHandlers(typeof(Program).Assembly)
    .ConfigureGlobalReceivePipe(x => x.UseLogging())
    .ConfigureCommandReceivePipe(x => x.UseValidation())
    .ConfigureRequestPipe(x => x.UseCaching())
    .ConfigureEventReceivePipe(x => x.UseEventStore())
    .ConfigurePublishPipe(x => x.UseOutboxPattern())
    .Build();
```

## 🏗️ 依赖注入集成

### Microsoft.Extensions.DependencyInjection

```bash
Install-Package Mediator.Net.MicrosoftDependencyInjection
```

```csharp
services.AddMediator(builder => 
{
    builder.RegisterHandlers(typeof(Program).Assembly);
});
```

### Autofac

```bash
Install-Package Mediator.Net.Autofac
```

```csharp
var builder = new ContainerBuilder();
var mediatorBuilder = new MediatorBuilder()
    .RegisterHandlers(typeof(Program).Assembly);

builder.RegisterMediator(mediatorBuilder);
var container = builder.Build();
```

### 其他支持的容器

- **SimpleInjector**：`Mediator.Net.SimpleInjector`
- **StructureMap**：`Mediator.Net.StructureMap`
- **Ninject**：`Mediator.Net.Ninject`

## 🔌 官方中间件包

### Serilog 日志

```bash
Install-Package Mediator.Net.Middlewares.Serilog
```

```csharp
.ConfigureGlobalReceivePipe(x => x.UseSerilog(LogEventLevel.Information))
```

### 工作单元

```bash
Install-Package Mediator.Net.Middlewares.UnitOfWork
```

为事务操作提供 `CommittableTransaction` 支持。

### EventStore 集成

```bash
Install-Package Mediator.Net.Middlewares.EventStore
```

自动将事件持久化到 EventStore。

## 🎯 高级特性

### 上下文服务

在中间件和处理器之间共享服务：

```csharp
// 在中间件中
public Task Execute(TContext context, CancellationToken cancellationToken)
{
    context.RegisterService(new AuditInfo { Timestamp = DateTime.UtcNow });
    return Task.CompletedTask;
}

// 在处理器中
public async Task Handle(IReceiveContext<MyCommand> context, CancellationToken cancellationToken)
{
    if (context.TryGetService(out AuditInfo auditInfo))
    {
        // 使用审计信息
    }
}
```

### 从处理器发布事件

```csharp
public async Task Handle(IReceiveContext<CreateOrderCommand> context, CancellationToken cancellationToken)
{
    // 处理命令
    var order = new Order(context.Message.CustomerId);
    
    // 发布领域事件
    await context.Publish(new OrderCreatedEvent 
    { 
        OrderId = order.Id, 
        CustomerId = order.CustomerId 
    });
}
```

## 📚 文档

更详细的文档、示例和高级场景，请访问我们的 [Wiki](https://github.com/mayuanyang/Mediator.Net/wiki)。

## 🤝 贡献

我们欢迎贡献！请查看我们的[贡献指南](CONTRIBUTING.md)了解详情。

## 📄 许可证

本项目采用 MIT 许可证 - 详情请参阅 [LICENSE.txt](LICENSE.txt) 文件。

## 🙋‍♂️ 支持

- 📖 [文档](https://github.com/mayuanyang/Mediator.Net/wiki)
- 💬 [Stack Overflow](http://stackoverflow.com/questions/tagged/memdiator.net)（使用 `mediator.net` 标签）
- 🐛 [问题反馈](https://github.com/mayuanyang/Mediator.Net/issues)

---

⭐ 如果您觉得这个项目有用，请给它一个星标！
