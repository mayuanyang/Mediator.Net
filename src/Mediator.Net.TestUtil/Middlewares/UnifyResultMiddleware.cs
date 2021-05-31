using System;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net.TestUtil.Services;
using Xunit.Sdk;

namespace Mediator.Net.TestUtil.Middlewares
{
    public class UnifiedResponse: IResponse
    {
        public object Result { get; set; }
        public Error Error { get; set; }
    }
    
    public class GenericUnifiedResponse<T>: IResponse
    {
        public T Result { get; set; }
        public Error Error { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
    
    public static class UnifyResultMiddleware
    {
        public static void UseUnifyResultMiddleware<TContext>(this IPipeConfigurator<TContext> configurator, Type unifiedType)  
            where TContext : IContext<IMessage>
        {
            configurator.AddPipeSpecification(new UnifyResultMiddlewareSpecification<TContext>(unifiedType));
        }
    }

    public class UnifyResultMiddlewareSpecification<TContext> : IPipeSpecification<TContext>
        where TContext : IContext<IMessage>
    {
        private readonly Type _unifiedType;

        public UnifyResultMiddlewareSpecification(Type unifiedType)
        {
            _unifiedType = unifiedType;
        }

        public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
        {
            return true;
        }

        public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task Execute(TContext context, CancellationToken cancellationToken)
        {
            if (ShouldExecute(context, cancellationToken))
            {
                Console.WriteLine($"Before 1: {context.Message}");
                context.RegisterService(new DummyTransaction());
            }
            return Task.FromResult(0);
        }

        public Task AfterExecute(TContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        
        public Task OnException(Exception ex, TContext context)
        {
            
            if (_unifiedType == null || ex.GetType() != typeof(BusinessException))
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
                throw ex;   
            }
            
            var businessException = ex as BusinessException;
            
            var tArgs = context.ResultGenericArguments;
            var targetType = tArgs != null && tArgs.Any() ? _unifiedType.MakeGenericType(tArgs) : _unifiedType;
            
            var unifiedTypeInstance = Activator.CreateInstance(targetType) as dynamic;
            
            unifiedTypeInstance.Error = new Error()
            {
                Code = businessException.Code,
                Message = businessException.Error
            };
            context.Result = unifiedTypeInstance;
            return Task.FromResult(0);
        }
    }

    public class BusinessException: Exception
    {
        public int Code { get; set; }
        public string Error { get; set; }
    }
}
