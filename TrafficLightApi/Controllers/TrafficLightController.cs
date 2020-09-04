using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TraficLightsRazorPages.Core.Workers;
using TraficLightsRazorPages.Data;
using TraficLightsRazorPages.Models;
using TraficLightsRazorPages.Models.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrafficLightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrafficLightController : ControllerBase
    {
        IServiceProvider Services
        {
            get;
        }
        private TrafficLight _currentTrafficLight;
        private readonly TraficLightsWorker _traficLightsWorker;
        private readonly TrafficLightRepository _repository;

        public TrafficLightController(IServiceProvider serviceProvider, TrafficLight trafficLight, TraficLightsWorker traficLightsWorker)
        {
            Services = serviceProvider.CreateScope().ServiceProvider;
            _repository = Services.GetRequiredService<TrafficLightRepository>();
            _currentTrafficLight = trafficLight;
            _traficLightsWorker = traficLightsWorker;
        }

        // GET: api/<TrafficLight>
        [HttpGet]
        public ITrafficLight Get()
        {
            var data =
             new TrafficLightEntity() { Id = 1, Color = Colors.Yellow, Date = DateTime.UtcNow };
            return data;
        }

        // GET api/<TrafficLight>/5
        [HttpGet("{id}")]
        public ITrafficLight Get(int id)
        {

            var traficLightByID = _repository.GetByIdAsync(id, CancellationToken.None).Result;

            if (traficLightByID is null)
            {
                traficLightByID = new TrafficLightEntity()
                {
                    Color = Colors.Red,
                    Date = DateTime.Now
                };
                _repository.AddTrafficLight(traficLightByID, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                int lastId = _repository.GetMaxTraficLightsIdAsync(CancellationToken.None).Result;
                _currentTrafficLight.Id = lastId;
                TraficLightsWorker.TrafficLightsList.Add(_currentTrafficLight);
                _traficLightsWorker.CurrentTrafficLight = _currentTrafficLight;
                _traficLightsWorker.SetCurrentColorFromDBAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                _traficLightsWorker.Activate();
                //TODO to look if it's possible to simplifer this method
               // return = TraficLightsWorker.TrafficLightsList.FirstOrDefault(e => e.Id == lastId);
                var trLights = TraficLightsWorker.TrafficLightsList.FirstOrDefault(e => e.Id == lastId);
                return new TrafficLightEntity() { Id = trLights.Id, Color = trLights.Color, Date = trLights.Date };
            }
            var traficLightWithCurrentId = TraficLightsWorker.TrafficLightsList.FirstOrDefault(t => t.Id == id);
            if (traficLightWithCurrentId != null)
            {
                _currentTrafficLight = traficLightWithCurrentId;
                _traficLightsWorker.CurrentTrafficLight = _currentTrafficLight;
                _traficLightsWorker.Activate();
                return new TrafficLightEntity() { Id = _currentTrafficLight.Id, Color = _currentTrafficLight.Color, Date = _currentTrafficLight.Date };
            }
            // return _currentTrafficLight;
        
            else
            {
                TraficLightsWorker.TrafficLightsList.Add(_currentTrafficLight);
                _currentTrafficLight.Id = traficLightByID.Id;
                _traficLightsWorker.CurrentTrafficLight = _currentTrafficLight;
                _traficLightsWorker.SetCurrentColorFromDBAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                _traficLightsWorker.Activate();
                //return _currentTrafficLight;
                return new TrafficLightEntity() { Id = _currentTrafficLight.Id, Color = _currentTrafficLight.Color, Date = _currentTrafficLight.Date }; ;
            }
        }

        // POST api/<TrafficLight>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TrafficLight>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TrafficLight>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
