# Mediator.Net
A mediator project for .NET
## Get Packages
You can get Autofac by [grabbing the latest NuGet packages](https://www.nuget.org/packages/Mediator.Net/).

## Get Started

Setup the mediator by using MediatorBuilder
```C#
  
	var mediaBuilder = new MediatorBuilder();
	var mediator = mediaBuilder.RegisterHandlers(typeof(this).Assembly).Build();
           
```

###Handlers scan registration
```C#

	var mediaBuilder = new MediatorBuilder();
	var mediator = mediaBuilder.RegisterHandlers(typeof(this).Assembly).Build();
           
```

###Handlers explicit registration
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

###Sending a command, publishing event and sending request and getting response
```C#

	await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
	await _mediator.PublishAsync(new TestEvent(Guid.NewGuid()));
	var result = await _mediator.RequestAsync<GetGuidRequest, GetGuidResponse>(new GetGuidRequest(_guid));

```

###Using pipelines
You can setup different pipelines for different purpose, see test project for samples

###Setting up middlewares
You can add as many middlewares into the pipelines as you want, see test project for samples
