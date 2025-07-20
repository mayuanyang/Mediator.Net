# Mediator.Net

[![Stack Overflow](https://img.shields.io/badge/stack%20overflow-Mediator.Net-yellowgreen.svg)](http://stackoverflow.com/questions/tagged/memdiator.net)
[![Build Status](https://ci.appveyor.com/api/projects/status/j42okw862yjgdeo9?svg=true)](https://ci.appveyor.com/project/mayuanyang/mediator-net)
[![CI](https://github.com/mayuanyang/Mediator.Net/actions/workflows/ci.yml/badge.svg)](https://github.com/mayuanyang/Mediator.Net/actions/workflows/ci.yml)
[![Release](https://github.com/mayuanyang/Mediator.Net/actions/workflows/release.yml/badge.svg)](https://github.com/mayuanyang/Mediator.Net/actions/workflows/release.yml)
[![codecov](https://codecov.io/gh/mayuanyang/Mediator.Net/branch/master/graph/badge.svg?token=MuQkMlLAcG)](https://codecov.io/gh/mayuanyang/Mediator.Net)
[![NuGet](https://img.shields.io/nuget/v/Mediator.Net.svg)](https://www.nuget.org/packages/Mediator.Net/)

A powerful and flexible mediator implementation for .NET that enables clean architecture by decoupling request/response handling through the mediator pattern.

<p align="center">
  <img src="https://cloud.githubusercontent.com/assets/3387099/24353370/97f573f0-1330-11e7-890c-85855628a575.png" alt="Mediator.Net Logo" width="200"/>
</p>

## 📋 Table of Contents

- [Features](#-features)
- [Installation](#-installation)
- [Quick Start](#-quick-start)
  - [Basic Setup](#basic-setup)
  - [Define Messages and Handlers](#define-messages-and-handlers)
- [Usage Examples](#-usage-examples)
  - [Sending Commands](#sending-commands)
  - [Handling Requests](#handling-requests)
  - [Publishing Events](#publishing-events)
  - [Streaming Responses](#streaming-responses)
- [Handler Registration](#-handler-registration)
  - [Assembly Scanning (Recommended)](#assembly-scanning-recommended)
  - [Explicit Registration](#explicit-registration)
- [Pipeline & Middleware](#-pipeline--middleware)
  - [Pipeline Types](#pipeline-types)
  - [Creating Custom Middleware](#creating-custom-middleware)
  - [Configuring Pipelines](#configuring-pipelines)
- [Dependency Injection Integration](#️-dependency-injection-integration)
  - [Microsoft.Extensions.DependencyInjection](#microsoftextensionsdependencyinjection)
  - [Autofac](#autofac)
  - [Other Supported Containers](#other-supported-containers)
- [Official Middleware Packages](#-official-middleware-packages)
  - [Serilog Logging](#serilog-logging)
  - [Unit of Work](#unit-of-work)
  - [EventStore Integration](#eventstore-integration)
- [Advanced Features](#-advanced-features)
  - [Context Services](#context-services)
  - [Publishing Events from Handlers](#publishing-events-from-handlers)
- [Documentation](#-documentation)
- [Contributing](#-contributing)
- [License](#-license)
- [Support](#️-support)

## 🚀 Features

- **Command/Query Separation**: Clear separation between commands, queries, and events
- **Pipeline Support**: Extensible middleware pipeline for cross-cutting concerns
- **Streaming Support**: Handle multiple responses with `IAsyncEnumerable`
- **Dependency Injection**: Built-in support for popular IoC containers
- **Event Publishing**: Publish events from within handlers
- **Flexible Registration**: Both explicit and assembly scanning registration
- **Middleware Ecosystem**: Rich collection of pre-built middlewares

## 📦 Installation

Install the main package via NuGet:

```bash
Install-Package Mediator.Net
```

Or via .NET CLI:

```bash
dotnet add package Mediator.Net
```

## 🏁 Quick Start

### Basic Setup

```csharp
// Create and configure mediator
var mediaBuilder = new MediatorBuilder();
var mediator = mediaBuilder.RegisterHandlers(typeof(Program).Assembly).Build();
```

### Define Messages and Handlers

```csharp
// Command (no response)
public class CreateUserCommand : ICommand
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    public async Task Handle(IReceiveContext<CreateUserCommand> context, CancellationToken cancellationToken)
    {
        // Handle the command
        var user = new User(context.Message.Name, context.Message.Email);
        // Save user...
        
        // Publish an event
        await context.Publish(new UserCreatedEvent { UserId = user.Id });
    }
}

// Request/Response
public class GetUserQuery : IRequest<UserDto>
{
    public int UserId { get; set; }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    public async Task<UserDto> Handle(IReceiveContext<GetUserQuery> context, CancellationToken cancellationToken)
    {
        // Handle the query and return response
        return new UserDto { Id = context.Message.UserId, Name = "John Doe" };
    }
}

// Event
public class UserCreatedEvent : IEvent
{
    public int UserId { get; set; }
}

public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    public async Task Handle(IReceiveContext<UserCreatedEvent> context, CancellationToken cancellationToken)
    {
        // Handle the event
        Console.WriteLine($"User {context.Message.UserId} was created!");
    }
}
```

## 📋 Usage Examples

### Sending Commands

```csharp
// Command with no response
await mediator.SendAsync(new CreateUserCommand 
{ 
    Name = "John Doe", 
    Email = "john@example.com" 
});

// Command with response
var result = await mediator.SendAsync<CreateUserCommand, CreateUserResponse>(
    new CreateUserCommand { Name = "Jane Doe", Email = "jane@example.com" });
```

### Handling Requests

```csharp
// Request with response
var user = await mediator.RequestAsync<GetUserQuery, UserDto>(
    new GetUserQuery { UserId = 123 });
```

### Publishing Events

```csharp
// Publish event to all handlers
await mediator.Publish(new UserCreatedEvent { UserId = 123 });
```

### Streaming Responses

Create handlers that return multiple responses:

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
            yield return new UserDto { Id = i, Name = $"User {i}" };
        }
    }
}

// Consume the stream
await foreach (var user in mediator.CreateStream<GetUsersQuery, UserDto>(new GetUsersQuery()))
{
    Console.WriteLine($"Received: {user.Name}");
}
```

## 🔧 Handler Registration

### Assembly Scanning (Recommended)

```csharp
var mediator = new MediatorBuilder()
    .RegisterHandlers(typeof(Program).Assembly)
    .Build();
```

### Explicit Registration

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

## 🔄 Pipeline & Middleware

Mediator.Net supports five types of pipelines for different scenarios:

![Pipeline Architecture](https://cloud.githubusercontent.com/assets/3387099/21959127/9a065420-db09-11e6-8dbc-ca0069894e1c.png)

### Pipeline Types

| Pipeline | Description | Triggers For |
|----------|-------------|--------------|
| **GlobalReceivePipeline** | Executes for all messages | Commands, Requests, Events |
| **CommandReceivePipeline** | Executes only for commands | ICommand |
| **RequestReceivePipeline** | Executes only for requests | IRequest |
| **EventReceivePipeline** | Executes only for events | IEvent |
| **PublishPipeline** | Executes when events are published | IEvent (outgoing) |

### Creating Custom Middleware

#### 1. Create Middleware Extension

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

#### 2. Create Middleware Specification

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
        _logger.LogInformation("Processing message: {MessageType}", context.Message.GetType().Name);
        return Task.CompletedTask;
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Completed processing: {MessageType}", context.Message.GetType().Name);
        return Task.CompletedTask;
    }

    public void OnException(Exception ex, TContext context)
    {
        _logger.LogError(ex, "Error processing message: {MessageType}", context.Message.GetType().Name);
        throw ex;
    }
}
```

### Configuring Pipelines

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

## 🏗️ Dependency Injection Integration

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

### Other Supported Containers

- **SimpleInjector**: `Mediator.Net.SimpleInjector`
- **StructureMap**: `Mediator.Net.StructureMap`
- **Ninject**: `Mediator.Net.Ninject`

## 🔌 Official Middleware Packages

### Serilog Logging

```bash
Install-Package Mediator.Net.Middlewares.Serilog
```

```csharp
.ConfigureGlobalReceivePipe(x => x.UseSerilog(LogEventLevel.Information))
```

### Unit of Work

```bash
Install-Package Mediator.Net.Middlewares.UnitOfWork
```

Provides `CommittableTransaction` support for transactional operations.

### EventStore Integration

```bash
Install-Package Mediator.Net.Middlewares.EventStore
```

Automatically persists events to EventStore.

## 🎯 Advanced Features

### Context Services

Share services between middleware and handlers:

```csharp
// In middleware
public Task Execute(TContext context, CancellationToken cancellationToken)
{
    context.RegisterService(new AuditInfo { Timestamp = DateTime.UtcNow });
    return Task.CompletedTask;
}

// In handler
public async Task Handle(IReceiveContext<MyCommand> context, CancellationToken cancellationToken)
{
    if (context.TryGetService(out AuditInfo auditInfo))
    {
        // Use the audit info
    }
}
```

### Publishing Events from Handlers

```csharp
public async Task Handle(IReceiveContext<CreateOrderCommand> context, CancellationToken cancellationToken)
{
    // Process the command
    var order = new Order(context.Message.CustomerId);
    
    // Publish domain event
    await context.Publish(new OrderCreatedEvent 
    { 
        OrderId = order.Id, 
        CustomerId = order.CustomerId 
    });
}
```

## 📚 Documentation

For more detailed documentation, examples, and advanced scenarios, visit our [Wiki](https://github.com/mayuanyang/Mediator.Net/wiki).

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## 🙋‍♂️ Support

- 📖 [Documentation](https://github.com/mayuanyang/Mediator.Net/wiki)
- 💬 [Stack Overflow](http://stackoverflow.com/questions/tagged/memdiator.net) (use the `mediator.net` tag)
- 🐛 [Issues](https://github.com/mayuanyang/Mediator.Net/issues)

---

⭐ If you find this project useful, please give it a star!
