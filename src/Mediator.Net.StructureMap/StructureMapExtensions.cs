using StructureMap;

namespace Mediator.Net.StructureMap
{
    public static class StructureMapExtensions
    {
        public static void Configure(this IContainer container, MediatorBuilder builder)
        {
            container.Configure(x =>
            {
                x.For<MediatorBuilder>().Add(builder).Singleton();
                x.For<IDependencyScope>().Use(() => new StructureMapDependencyScope(container).BeginScope());
                x.For<IMediator>().Use(context => builder.Build(context.GetInstance<IDependencyScope>()));
                
                foreach (var binding in builder.MessageHandlerRegistry.MessageBindings)
                {
                    x.AddType(binding.HandlerType, binding.HandlerType);
                }
            });
        }
    }
}