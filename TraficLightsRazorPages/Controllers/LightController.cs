using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TraficLightsRazorPages.Core.Hubs;
using TraficLightsRazorPages.Core.TrafficLights.Queries;
using TraficLightsRazorPages.Models;

namespace TraficLightsRazorPages.Controllers
{
    [Route("Light")]
    public class LightController : Controller
    {
        private readonly TrafficLight _traficLight;
        private readonly IMediator _mediator;

        private readonly IHubContext<TraficLightsHub> _hubContext;
     /*   private readonly TraficLightsContext _lightsContext;*/
        public LightController(IHubContext<TraficLightsHub> hubContext, /*TraficLightsContext dbContext,*/
            TrafficLight trafficLight, IMediator mediator)
        {
            _traficLight = trafficLight;
            _mediator = mediator;
            this._hubContext = hubContext;
            
            /*
            this._lightsContext = dbContext;*/
        }
        public IActionResult Index()
        {
            //TODO
            ViewBag.Color = _traficLight.Color.ToString();
            return View(ViewBag);
        }

        [HttpPost("nextcolor")]
        public async Task<IActionResult> NextColor()
        {/*
            _traficLight.NextColor();//calling notifacation */
            await _mediator.Publish(new ChangeColorCommand());

            ViewBag.Color = _traficLight.Color.ToString();
            await _hubContext.Clients.All.SendAsync("ReceiveColor", _traficLight.Color.ToString());

            return NoContent();
        }
    }
}
