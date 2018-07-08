using System;
using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil.Messages;
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
        public async Task<SimpleResponse> Get()
        {
            return await _mediator.RequestAsync<SimpleRequest, SimpleResponse>(new SimpleRequest("Hello world"));
        }

        // POST api/values
        [HttpPost]
        [Route("command")]
        public async Task Post([FromBody]string value)
        {
            await _mediator.SendAsync(new SimpleCommand(Guid.NewGuid()));
        }

        // PUT api/values/5
        [HttpPost]
        [Route(("event"))]
        public async Task PostEvent(int id, [FromBody]string value)
        {
            await _mediator.PublishAsync(new SimpleEvent());
        }
        
    }
}
