# Usage Examples

## Sending Commands

### Command with No Response

```csharp
await mediator.SendAsync(new CreateUserCommand 
{ 
    Name = "John Doe", 
    Email = "john@example.com" 
});
```

### Command with Response

```csharp
var result = await mediator.SendAsync<CreateUserCommand, CreateUserResponse>(
    new CreateUserCommand { Name = "Jane Doe", Email = "jane@example.com" });
```

## Handling Requests

```csharp
var user = await mediator.RequestAsync<GetUserQuery, UserDto>(
    new GetUserQuery { UserId = 123 });
```

## Publishing Events

```csharp
await mediator.Publish(new UserCreatedEvent { UserId = 123 });
```

## Streaming Responses

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
