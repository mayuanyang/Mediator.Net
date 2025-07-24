# Quick Start

## Basic Setup

To get started with Mediator.Net, you can create and configure the mediator as follows:

```csharp
// Create and configure mediator
var mediaBuilder = new MediatorBuilder();
var mediator = mediaBuilder.RegisterHandlers(typeof(Program).Assembly).Build();
```

## Define Messages and Handlers

Define your messages and handlers as shown below:

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
