using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Serilog;
using Serilog.Events;

namespace Mediator.Net.Middlewares.Serilog
{
    class SerilogMiddlewareSpecification<TContext> : IPipeSpecification<TContext>
        where TContext : IContext<IMessage>
    {
        private readonly ILogger _logger;
        private readonly LogEventLevel _level;
        private readonly Func<TContext, bool> _shouldExcute;

        public SerilogMiddlewareSpecification(ILogger logger, LogEventLevel level, Func<TContext, bool> shouldExcute)
        {
            _logger = logger;
            _level = level;
            _shouldExcute = shouldExcute;
        }
        
        public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
        {
            if (_shouldExcute == null) return true;
            
            return _shouldExcute.Invoke(context);
        }

        public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
        {
            return Task.WhenAll();
        }

        public Task Execute(TContext context, CancellationToken cancellationToken)
        {
            if (ShouldExecute(context, cancellationToken))
            {
                switch (_level)
                {
                    case LogEventLevel.Error:
                        _logger.Error("Receive message {@Message}", context.Message);
                        break;
                    case LogEventLevel.Debug:
                        _logger.Debug("Receive message {@Message}", context.Message);
                        break;
                    case LogEventLevel.Fatal:
                        _logger.Fatal("Receive message {@Message}", context.Message);
                        break;
                    case LogEventLevel.Information:
                        _logger.Information("Receive message {@Message}", context.Message);
                        break;
                    case LogEventLevel.Verbose:
                        _logger.Verbose("Receive message {@Message}", context.Message);
                        break;
                    case LogEventLevel.Warning:
                        _logger.Verbose("Receive message {@Message}", context.Message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            return Task.WhenAll();
        }

        public Task AfterExecute(TContext context, CancellationToken cancellationToken)
        {
            return Task.WhenAll();
        }

        public Task OnException(Exception ex, TContext context)
        {
            _logger.Error(ex, ex.Message);
            
            ExceptionDispatchInfo.Capture(ex).Throw();
            
            throw ex;
        }
    }
}
