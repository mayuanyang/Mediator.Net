# Handler Registration

Mediator.Net supports two methods for registering handlers: assembly scanning and explicit registration.

## Assembly Scanning (Recommended)

```csharp
var mediator = new MediatorBuilder()
    .RegisterHandlers(typeof(Program).Assembly)
    .Build();
```

## Explicit Registration

```csharp
var mediator = new MediatorBuilder()
    .RegisterHandlers(() => new List<MessageBinding>
    {
        new MessageBinding(typeof(CreateUserCommand), typeof(CreateUserCommandHandler)),
        new MessageBinding(typeof(GetUserQuery), typeof(GetUserQueryHandler)),
        new MessageBinding(typeof(UserCreatedEvent), typeof(UserCreatedEventHandler))
    })
    .Build();
