# Pipeline & Middleware

Mediator.Net supports five types of pipelines for different scenarios:

![Pipeline Architecture](https://cloud.githubusercontent.com/assets/3387099/21959127/9a065420-db09-11e6-8dbc-ca0069894e1c.png)

## Pipeline Types

| Pipeline | Description | Triggers For |
|----------|-------------|--------------|
| **GlobalReceivePipeline** | Executes for all messages | Commands, Requests, Events |
| **CommandReceivePipeline** | Executes only for commands | ICommand |
| **RequestReceivePipeline** | Executes only for requests | IRequest |
| **EventReceivePipeline** | Executes only for events | IEvent |
| **PublishPipeline** | Executes when events are published | IEvent (outgoing) |

## Creating Custom Middleware

### 1. Create Middleware Extension

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

### 2. Create Middleware Specification

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

## Configuring Pipelines

```csharp
var mediator = new MediatorBuilder()
    .RegisterHandlers(typeof(Program).Assembly)
    .ConfigureGlobalReceivePipe(x => x.UseLogging())
    .ConfigureCommandReceivePipe(x => x.UseValidation())
    .ConfigureRequestPipe(x => x.UseCaching())
    .ConfigureEventReceivePipe(x => x.UseEventStore())
    .ConfigurePublishPipe(x => x.UseOutboxPattern())
    .Build();
