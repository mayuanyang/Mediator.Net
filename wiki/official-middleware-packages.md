# Official Middleware Packages

Mediator.Net provides several official middleware packages to extend its functionality.

## Serilog Logging

```bash
Install-Package Mediator.Net.Middlewares.Serilog
```

```csharp
.ConfigureGlobalReceivePipe(x => x.UseSerilog(LogEventLevel.Information))
```

## Unit of Work

```bash
Install-Package Mediator.Net.Middlewares.UnitOfWork
```

Provides `CommittableTransaction` support for transactional operations.

## EventStore Integration

```bash
Install-Package Mediator.Net.Middlewares.EventStore
```

Automatically persists events to EventStore.
