# Advanced Features

Mediator.Net offers several advanced features to enhance its functionality.

## Context Services

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

## Publishing Events from Handlers

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
