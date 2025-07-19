[![Mediator.Net on Stack Overflow](https://img.shields.io/badge/stack%20overflow-Mediator.Net-yellowgreen.svg)](http://stackoverflow.com/questions/tagged/memdiator.net)

![Build status](https://ci.appveyor.com/api/projects/status/j42okw862yjgdeo9?svg=true)
![example workflow](https://github.com/mayuanyang/Mediator.Net/actions/workflows/ci.yml/badge.svg)
![example workflow](https://github.com/mayuanyang/Mediator.Net/actions/workflows/release.yml/badge.svg)
[![codecov](https://codecov.io/gh/mayuanyang/Mediator.Net/branch/master/graph/badge.svg?token=MuQkMlLAcG)](https://codecov.io/gh/mayuanyang/Mediator.Net)

# Mediator.Net

A mediator project for .NET

![logo_sm](https://cloud.githubusercontent.com/assets/3387099/24353370/97f573f0-1330-11e7-890c-85855628a575.png)

## Get Packages

You can get Mediator.Net by [grabbing the latest NuGet packages](https://www.nuget.org/packages/Mediator.Net/).

## Get Started

Install the nuget package Mediator.Net

```C#
Install-Package Mediator.Net
```

## Simple usage

```C#
// Setup a mediator builder
var mediaBuilder = new MediatorBuilder();
var mediator = mediaBuilder.RegisterHandlers(typeof(this).Assembly).Build();
```

### Sending a command with no response

```C#
await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
```

### Sending a command with response

```C#
var pong = await _mediator.SendAsync<Ping, Pong>(new Ping());
```

### Sending request with response

```C#
var result = await _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_guid));
```

### Publishing an event

```C#
await _mediator.Publish(new OrderPlacedEvent);
```

### Publishing an event as the result of a command

Inside a command handler.Handle method, a IReceiveContext<T> expose a method of Publish

```C#
public async Task Handle(IReceiveContext<DerivedTestBaseCommand> context, CancellationToken cancellationToken)
{
    // Do you work
    await context.Publish(new OrderPlacedEvent());
}
```

### Create stream of responses

Sometimes you might want to get multiple responses by one request or command, you can do that by using the `CreateStream` method

```C#
// Define a StreamHandler by implementing the IStreamRequestHandler or IStreamCommandHandler interfaces for IRequest and ICommand
public class GetMultipleGuidStreamRequestHandler : IStreamRequestHandler<GetGuidRequest, GetGuidResponse>
{
    public async IAsyncEnumerable<GetGuidResponse> Handle(IReceiveContext<GetGuidRequest> context, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        for (var i = 0; i < 5; i++)
        {
            await Task.Delay(100, cancellationToken);
            yield return await Task.FromResult(new GetGuidResponse(Guid.NewGuid() ){Index = i});
        }
    }
}

// You can now get multiple responses back by using this
IAsyncEnumerable<GetGuiResponse> result = mediator.CreateStream<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_guid));

await foreach (var r in result)
{
  Console.WriteLine(r.Id.ToString());
}

```

How about EventHandler?
What would be the use cases of a stream of events? So it is currently not supported

How about middleware?
You can use middleware as normal, keep in mind that middleware will only get invoked once for each IRequest or ICommand thought that multiple responses might return

### Handling message from handler

Once a message is sent, it will reach its handlers, you can only have one handler for ICommand and IRequest and can have multi handlers for IEvent. ReceiveContext<T> will be delivered to the handler.

```C#
class TestBaseCommandHandler : ICommandHandler<TestBaseCommand>
{
    public Task Handle(ReceiveContext<TestBaseCommand> context)
    {
        Console.WriteLine(context.Message.Id);
        return Task.FromResult(0);
    }
}

// Or in async
class AsyncTestBaseCommandHandler : ICommandHandler<TestBaseCommand>
{
    public async Task Handle(ReceiveContext<TestBaseCommand> context)
    {
        Console.WriteLine(context.Message.Id);
        await Task.FromResult(0);
    }
}
```

## Handler Registration

### Handlers explicit registration

```C#
var mediator = builder.RegisterHandlers(() =>
{
    var binding = new List<MessageBinding>
    {
        new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandler)),
        new MessageBinding(typeof(DerivedTestBaseCommand), typeof(DerivedTestBaseCommandHandler))
    };
    return binding;
}).Build();
```

### Scan registration

```C#
var mediaBuilder = new MediatorBuilder();
var mediator = mediaBuilder.RegisterHandlers(typeof(this).Assembly).Build();
```

### Using pipelines

There are 5 different type of pipelines you can use
![image](https://cloud.githubusercontent.com/assets/3387099/21959127/9a065420-db09-11e6-8dbc-ca0069894e1c.png)

#### GlobalReceivePipeline

This pipeline will be triggered whenever a message is sent, published or requested before it reaches the next pipeline and handler

#### CommandReceivePipeline

This pipeline will be triggered just after the `GlobalReceivePipeline` and before it reaches its command handler, this pipeline will only be used for `ICommand`

#### EventReceivePipeline

This pipeline will be triggered just after the `GlobalReceivePipeline` and before it reaches its event handler/handlers, this pipeline will only be used for `IEvent`

#### RequestReceivePipeline

This pipeline will be triggered just after the `GlobalReceivePipeline` and before it reaches its request handler, this pipeline will only be used for `IRequest`

#### PublishPipeline

This pipeline will be triggered when an `IEvent` is published inside your handler, this pipeline will only be used for `IEvent` and is usually being used as outgoing interceptor

### Setting up middlewares

The most powerful thing for the pipelines above is you can add as many middlewares as you want.
Follow the following steps to setup a middleware

- Add a static class for your middleware
- Add a public static extension method in that class you just added, usually follow the UseXxxx naming convention
- Add another class for your middleware's specification, note that this is the implementation of your middleware

You might need some dependencies in your middleware, there are two ways to do it

- Pass them in explicitly
- Let the IoC container to resolve it for you (if you are using IoC)

Here is a sample middleware

## Middleware class

```C#
public static class SerilogMiddleware
{
    public static void UseSerilog<TContext>(this IPipeConfigurator<TContext> configurator, LogEventLevel logAsLevel, ILogger logger = null)
        where TContext : IContext<IMessage>
    {
        if (logger == null && configurator.DependencyScope == null)
        {
            throw new DependencyScopeNotConfiguredException($"{nameof(ILogger)} is not provided and IDependencyScope is not configured, Please ensure {nameof(ILogger)} is registered properly if you are using IoC container, otherwise please pass {nameof(ILogger)} as parameter");
        }
        logger = logger ?? configurator.DependencyScope.Resolve<ILogger>();

        configurator.AddPipeSpecification(new SerilogMiddlewareSpecification<TContext>(logger, logAsLevel));
    }
}
```

## Specification class

```C#
class SerilogMiddlewareSpecification<TContext> : IPipeSpecification<TContext> where TContext : IContext<IMessage>
    {
        private readonly ILogger _logger;
        private readonly Func<bool> _shouldExecute;
        private readonly LogEventLevel _level;

        public SerilogMiddlewareSpecification(ILogger logger, LogEventLevel level, Func<bool> shouldExecute )
        {
            _logger = logger;
            _level = level;
            _shouldExecute = shouldExecute;
        }
        public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
        {
            if (_shouldExecute == null)
            {
                return true;
            }
            return _shouldExecute.Invoke();
        }

        public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task Execute(TContext context, CancellationToken cancellationToken)
        {
            if (ShouldExecute(context, cancellationToken))
            {
                switch (_level)
                {
                    case LogEventLevel.Error:
                        _logger.Error("Receive message {@Message}", context.Message);
                        break;
                    case LogEventLevel.Debug:
                        _logger.Debug("Receive message {@Message}", context.Message);
                        break;
                    case LogEventLevel.Fatal:
                        _logger.Fatal("Receive message {@Message}", context.Message);
                        break;
                    case LogEventLevel.Information:
                        _logger.Information("Receive message {@Message}", context.Message);
                        break;
                    case LogEventLevel.Verbose:
                        _logger.Verbose("Receive message {@Message}", context.Message);
                        break;
                    case LogEventLevel.Warning:
                        _logger.Verbose("Receive message {@Message}", context.Message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return Task.FromResult(0);
        }

        public Task AfterExecute(TContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public void OnException(Exception ex, TContext context)
        {
            throw ex;
        }
    }
```

### To hook up middlewares into pipelines

```C#
var builder = new MediatorBuilder();
_mediator = builder.RegisterHandlers(() =>
    {
        return new List<MessageBinding>()
        {
            new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandlerRaiseEvent)),
            new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)),
            new MessageBinding(typeof(GetGuidRequest), typeof(GetGuidRequestHandler))
        };
    })
    .ConfigureGlobalReceivePipe(x =>
    {
        x.UseDummySave();
    })
    .ConfigureCommandReceivePipe(x =>
    {
        x.UseConsoleLogger1();
    })
    .ConfigureEventReceivePipe(x =>
    {
        x.UseConsoleLogger2();
    })
    .ConfigureRequestPipe(x =>
    {
        x.UseConsoleLogger3();
    })
    .ConfigurePublishPipe(x =>
    {
        x.UseConsoleLogger4();
    })
.Build();
```

### ReceiveContext in Handlers

As you might already noticed, mediator will deliver ReceiveContext<T> to the handler and it has a property `Message` which is the original message sent, in some cases you might have one event being handled in multiple handlers and you might want to share something between, `ReceiveContext` would is good place that to register your service or instance. For example you can make a middleware and register the service from there.

#### Register DummyTransaction from middleware

```C#
public class SimpleMiddlewareSpecification<TContext> : IPipeSpecification<TContext>
    where TContext : IContext<IMessage>
{
    public bool ShouldExecute(TContext context)
    {
        return true;
    }

    public Task BeforeExecute(TContext context)
    {
        return Task.FromResult(0);
    }

    public Task Execute(TContext context)
    {
        if (ShouldExecute(context))
        {
            context.RegisterService(new DummyTransaction());
        }
        return Task.FromResult(0);
    }

    public Task AfterExecute(TContext context)
    {
        return Task.FromResult(0);
    }
}
```

#### Get the DummyTransaction registered in the middleware from the handler

```C#
public Task Handle(ReceiveContext<SimpleCommand> context)
{
    _simpleService.DoWork();
    if (context.TryGetService(out DummyTransaction transaction))
    {
        transaction.Commit();
    }
    return Task.FromResult(0);
}
```

### Using dependency injection(IoC) frameworks

#### Autofac

Install the nuget package Mediator.Net.Autofac

```C#
Install-Package Mediator.Net.Autofac
```

An extension method RegisterMediator for ContainerBuilder from Autofac is used to register the builder

The super simple use case

```C#
var mediaBuilder = new MediatorBuilder();
mediaBuilder.RegisterHandlers(typeof(TestContainer).Assembly);
var containerBuilder = new ContainerBuilder();
containerBuilder.RegisterMediator(mediaBuilder);
 _container = containerBuilder.Build();
```

You can also setup middlewares for each pipe before register it

```C#
var mediaBuilder = new MediatorBuilder();
mediaBuilder.RegisterHandlers(typeof(TestContainer).Assembly)
    .ConfigureCommandReceivePipe(x =>
    {
        x.UseSimpleMiddleware();
    });
var containerBuilder = new ContainerBuilder();
containerBuilder.RegisterMediator(mediaBuilder);
_container = containerBuilder.Build();
```



## Middlewares

One of the key feature for Mediator.Net is you can plug as many middlewares as you like, we have implemented some common one as below

### Mediator.Net.Middlewares.UnitOfWork

```
Install-Package Mediator.Net.Middlewares.UnitOfWork
```

This middleware provide a CommittableTransaction inside the context, handlers can enlist the transaction if it requires UnitOfWork
[Mediator.Net.Middlewares.UnitOfWork](https://github.com/mayuanyang/Mediator.Net.Middlewares.UnitOfWork) - Middleware for Mediator.Net to support unit of work.

### Mediator.Net.Middlewares.Serilog

```
Install-Package Mediator.Net.Middlewares.Serilog
```

This middleware logs every message by using Serilog

### Mediator.Net.Middlewares.EventStore

```
Install-Package Mediator.Net.Middlewares.EventStore
```

Middleware for Mediator.Net to write events to GetEventStore, it is a Middleware for Mediator.Net that plugs into the publish pipeline
[Mediator.Net.Middlewares.UnitOfWork](https://github.com/mayuanyang/Mediator.Net.Middlewares.EventStore) - Middleware for Mediator.Net to persist event to EventStore.
