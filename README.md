![Build status](https://ci.appveyor.com/api/projects/status/j42okw862yjgdeo9?svg=true) [![Mediator.Net on Stack Overflow](https://img.shields.io/badge/stack%20overflow-Mediator.Net-yellowgreen.svg)](http://stackoverflow.com/questions/tagged/memdiator.net)

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


### Sending a command, publishing event and sending request and getting response
```C#
await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
await _mediator.PublishAsync(new TestEvent(Guid.NewGuid()));
var result = await _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_guid));
```

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
Follow the following steps to setup a middlewaree
* Add a static class for your middleware
* Add a public static extension method in that class you just added, usually follow the UseXxxx naming convention
* Add another class for your middleware's specification, note that this is the implementation of your middleware

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
        private readonly Func<bool> _shouldExcute;
        private readonly LogEventLevel _level;

        public SerilogMiddlewareSpecification(ILogger logger, LogEventLevel level, Func<bool> shouldExcute )
        {
            _logger = logger;
            _level = level;
            _shouldExcute = shouldExcute;
        }
        public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
        {
            if (_shouldExcute == null)
            {
                return true;
            }
            return _shouldExcute.Invoke();
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
#### StructureMap
```C#
Install-Package Mediator.Net.StructureMap
```
Setup an IContainer and do your normal registration, then pass it along with the MediatorBuilder to the StructureMapExtensions class to register Mediator.Net
```C#
var mediaBuilder = new MediatorBuilder();
mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
    .ConfigureCommandReceivePipe(x =>
    {
        x.UseSimpleMiddleware();
    });
_container = new Container();
_container.Configure(x =>
{
    // Do your thing
});
StructureMapExtensions.Configure(mediaBuilder, _container);
```

#### Unity
```C#
Install-Package Mediator.Net.Unity
```
Setup an IUnityContainer and do your normal registration, then pass it along with the MediatorBuilder to the UnityExtensions class to register Mediator.Net
```C#
var mediaBuilder = new MediatorBuilder();
var mediaBuilder = new MediatorBuilder();
mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
    .ConfigureCommandReceivePipe(x =>
    {
        x.UseSimpleMiddleware();
    });
_container = new UnityContainer();
_container.RegisterType<SimpleService>();
_container.RegisterType<AnotherSimpleService>();

UnityExtensioins.Configure(mediaBuilder, _container);
```

#### SimpleInjector
```C#
Install-Package Mediator.Net.SimpleInjector
```
We have created a helper class InjectHelper to register all necessary components for Mediator.Net

```C#
var mediaBuilder = new MediatorBuilder();
mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
    .ConfigureCommandReceivePipe(x =>
    {
        x.UseSimpleMiddleware();
    });
_container = new Container();
_container.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();
_container.Register<SimpleService>();
_container.Register<AnotherSimpleService>();
    
InjectHelper.RegisterMediator(_container, mediaBuilder);
```
Thought that you can have transient registration for IMediator, but we recommend to use lifetime scope, you can do constructor injection as well as the following
```C#
using (var scope = _container.BeginLifetimeScope())
{
    _mediator = scope.GetInstance<IMediator>();
    _task = _mediator.RequestAsync<SimpleRequest, SimpleResponse>(new SimpleRequest());
}
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
Middleware for Mediator.Net to write event to GetEventStore, it is a Middleware for Mediator.Net that plugs intothe publish pipeline
[Mediator.Net.Middlewares.UnitOfWork](https://github.com/mayuanyang/Mediator.Net.Middlewares.EventStore) - Middleware for Mediator.Net to persist event to EventStore.


