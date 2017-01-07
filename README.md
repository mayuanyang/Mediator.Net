# Mediator.Net
A mediator project for .NET
## Get Packages
You can get Autofac by [grabbing the latest NuGet packages](https://www.nuget.org/packages/Mediator.Net/).

## Get Started
Install the nuget package Mediator.Net
```C#
	Install-Package Mediator.Net
```

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
There are 4 different type of pipeline you can use
####GlobalReceivePipeline
This pipeline will be triggered whenever a message is sent, published or requested before it reach the next pipeline and handler

####CommandReceivePipeline
This pipeline will be triggered just after the GlobalReceivePipeline and before it reach its command handler, this pipeline will only used for ICommand

####EventReceivePipeline
This pipeline will be triggered just after the GlobalReceivePipeline and before it reach its event handler/handlers, this pipeline will only used for IEvent

####RequestReceivePipeline
This pipeline will be triggered just after the GlobalReceivePipeline and before it reach its request handler, this pipeline will only used for IRequest

####PublishPipeline
This pipeline will be triggered when an IEvent is published inside your handler, this pipeline will only used for IEvent, it is usually being used as outgoing interceptor

###Setting up middlewares
The most powerful thing for the pipelines above is you can add as many middlewares as you want.
Here is a simple middleware example
```C#

	static class ConsoleLog1
    {
        public static void UseConsoleLogger1<TContext>(this IPipeConfigurator<TContext> configurator)
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new ConsoleLogSpecification1<TContext>());
        }
    }

    class ConsoleLogSpecification1<TContext> : IPipeSpecification<TContext> 
        where TContext : IContext<IMessage>
    {
        public bool ShouldExecute(TContext context)
        {
            return true;

        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine($"Before 1: {context.Message}");
            RubishBox.Rublish.Add(nameof(ConsoleLog1.UseConsoleLogger1));
            return Task.FromResult(0);

        }

        public Task ExecuteAfterConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine($"After 1: {context.Message}");
            return Task.FromResult(0);
        }
    }

```

To hook up middlewares into pipelines
```C#

	
    var builder = new MediatorBuilder();
     _mediator = builder.RegisterHandlers(() =>
         {
             var binding = new List<MessageBinding>()
             {
                 new MessageBinding(typeof(TestBaseCommand), typeof(TestBaseCommandHandlerRaiseEvent)),
                 new MessageBinding(typeof(TestEvent), typeof(TestEventHandler)),
                 new MessageBinding(typeof(GetGuidRequest), typeof(GetGuidRequestHandler))
             };
             return binding;
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

###Using dependancy injection(IoC) frameworks
####Autofac
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
####StructureMap
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
        x.ForConcreteType<SimpleService>();
        x.ForConcreteType<AnotherSimpleService>();
    });
    StructureMapExtensions.Configure(mediaBuilder, _container);

```

####More IoC frameworks to be added
