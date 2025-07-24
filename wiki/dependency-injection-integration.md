# Dependency Injection Integration

Mediator.Net integrates with various dependency injection containers to manage the lifecycle of handlers and other components.

## Microsoft.Extensions.DependencyInjection

```bash
Install-Package Mediator.Net.MicrosoftDependencyInjection
```

```csharp
services.AddMediator(builder => 
{
    builder.RegisterHandlers(typeof(Program).Assembly);
});
```

## Autofac

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

## Other Supported Containers

- **SimpleInjector**: `Mediator.Net.SimpleInjector`
- **StructureMap**: `Mediator.Net.StructureMap`
- **Ninject**: `Mediator.Net.Ninject`
