using System;
using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.WebApiSample.Handlers.CommandHandler;
using Microsoft.AspNetCore.Mvc;

namespace Mediator.Net.WebApiSample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IMediator _mediator;

        public ValuesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET api/values
        [HttpGet]
        [Route("{message}")]
        public async Task<SimpleResponse> Get(string message)
        {
            return await _mediator.RequestAsync<SimpleRequest, SimpleResponse>(new SimpleRequest(message));
        }

        // POST api/values
        [HttpPost]
        [Route("command")]
        public async Task Post([FromBody]CommandData value)
        {
            await _mediator.SendAsync(new CalculateCommand(value.Left, value.Right));
        }

        // PUT api/values/5
        [HttpPost]
        [Route(("event"))]
        public async Task PostEvent(int id, [FromBody]EventData value)
        {
            await _mediator.PublishAsync(new ResultCalculatedEvent(value.Result));
        }
        
    }

    public class CommandData
    {
        public int Left { get; set; }
        public int Right { get; set; }
    }

    public class EventData
    {
        public int Result { get; set; }
    }
}
