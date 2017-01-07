using Mediator.Net.Binding;
using StructureMap;

namespace Mediator.Net.StructureMap
{
    public class StructureMapExtensions
    {
        public static void Configure(MediatorBuilder builder, IContainer container)
        {
            container.Configure(x =>
            {
                x.For<MediatorBuilder>().Add(builder).Singleton();
                x.For<IDependancyScope>().Use(() => new StructureMapDependancyScope(container).BeginScope());
                
                x.For<IMediator>().Use(context => builder.Build(context.GetInstance<IDependancyScope>()));
                foreach (var binding in MessageHandlerRegistry.MessageBindings)
                {
                    x.AddType(binding.HandlerType, binding.HandlerType);
                }
            });
        }
    }
}
