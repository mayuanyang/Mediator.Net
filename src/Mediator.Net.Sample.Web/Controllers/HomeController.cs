using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mediator.Net.IoCTestUtil.Messages;

namespace Mediator.Net.Sample.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<ActionResult> Index()
        {
            await _mediator.SendAsync(new SimpleCommand(Guid.NewGuid()));

            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
