using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TraficLightsRazorPages.Core.Workers;
using TraficLightsRazorPages.Data;
using TraficLightsRazorPages.Models;
using TraficLightsRazorPages.Models.Interfaces;

namespace TraficLightsRazorPages.Controllers
{
    public class TrafficLightsController : Controller
	{
		IServiceProvider Services
		{
			get;
		}
		private TrafficLight _currentTrafficLight;
		private readonly TraficLightsWorker _traficLightsWorker;
		private readonly TrafficLightRepository _repository;

		public TrafficLightsController(IServiceProvider serviceProvider, TrafficLight trafficLight, TraficLightsWorker traficLightsWorker)
		{
			Services = serviceProvider.CreateScope().ServiceProvider;
			_repository = Services.GetRequiredService<TrafficLightRepository>();
			_currentTrafficLight = trafficLight;
			_traficLightsWorker = traficLightsWorker;
		}
		public IActionResult Index(int id = 1)
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
				return Redirect($"{lastId}");
			}
			var traficLightWithCurrentId = TraficLightsWorker.TrafficLightsList.FirstOrDefault(t => t.Id == id);
			if (traficLightWithCurrentId != null)
			{
				_currentTrafficLight = traficLightWithCurrentId;
				_traficLightsWorker.CurrentTrafficLight = _currentTrafficLight;
				_traficLightsWorker.Activate();
				return View(_currentTrafficLight);
			}
			else
			{
				TraficLightsWorker.TrafficLightsList.Add(_currentTrafficLight);
				_currentTrafficLight.Id = traficLightByID.Id;
				_traficLightsWorker.CurrentTrafficLight = _currentTrafficLight;
				_traficLightsWorker.SetCurrentColorFromDBAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
				_traficLightsWorker.Activate();
				return View(_currentTrafficLight);
			}


		}

		/*[HttpPost]
		public async Task<IActionResult> Next(int TrafficLightId)
		{
			return Redirect($"/TrafficLights/Index/{TrafficLightId + 1}");
		}*/
	}
}