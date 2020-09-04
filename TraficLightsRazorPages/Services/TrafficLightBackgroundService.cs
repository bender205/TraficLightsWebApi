using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TraficLightsRazorPages.Data;

namespace TraficLightsRazorPages.Services
{
    public class TrafficLightBackgroundService : BackgroundService
    {
        //here must be client instance field

        private readonly TrafficLightRepository _repository;
        public TrafficLightBackgroundService(TrafficLightRepository repository)
        {
            _repository = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //here must be client instance field initialization 
            var allTraficLights = _repository.GetAllTraficLights();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
