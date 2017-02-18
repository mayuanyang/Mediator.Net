using System;
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

        public SerilogMiddlewareSpecification(ILogger logger, LogEventLevel level)
        {
            _logger = logger;
            _level = level;
        }
        public bool ShouldExecute(TContext context)
        {
            return true;

        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            if (ShouldExecute(context))
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
            return Task.FromResult(0);
        }

        public Task ExecuteAfterConnect(TContext context)
        {
            return Task.FromResult(0);
        }

        public void OnException(Exception ex, TContext context)
        {
            throw ex;
        }
    }
}
