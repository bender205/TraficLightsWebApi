using DataAccess.Context;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TraficLightsRazorPages.Core.Hubs;
using TraficLightsRazorPages.Data;
using TraficLightsRazorPages.Models;
using TraficLightsRazorPages.Models.Interfaces;

namespace TraficLightsRazorPages.Core.Workers
{
    public class TraficLightsWorker
    {
        public static List<TrafficLight> TrafficLightsList = new List<TrafficLight>();

        IServiceProvider _serviceProvider;
        public TrafficLight CurrentTrafficLight { get; set; }

        public delegate void ChangeColorHandler();

        private readonly TraficLightsContext _lightsContext;
       /* IServiceProvider Services { get; }*/

        private readonly IHubContext<TraficLightsHub> _hubContext;

        private readonly IMediator _mediator;

        private readonly TrafficLightRepository _repository;

        public event ChangeColorHandler Changes;
        public TraficLightsWorker(IHubContext<TraficLightsHub> hubContext, IServiceProvider serviceProvider, IMediator mediator, TrafficLight trafficLight)
        {
            _serviceProvider = serviceProvider.CreateScope().ServiceProvider;
            _lightsContext = _serviceProvider.GetRequiredService<TraficLightsContext>();
            _repository = _serviceProvider.GetRequiredService<TrafficLightRepository>();
            _hubContext = hubContext;
            _mediator = mediator;

        }
        public void Activate()
        {
            CurrentTrafficLight.ColorSwitchTimer = new Timer(SwitchNextColor, null, 0, Timeout.Infinite);
            Changes += () => SaveCurrentTrafficLightToDBAsync(_serviceProvider, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
            
        }
        public void InvokeChanges()
        {
            Changes?.Invoke();
        }

        public void SwitchNextColor(object? o)
        {
            if (CurrentTrafficLight.Color == Colors.Red)
            {
                CurrentTrafficLight.Color = Colors.Yellow;
                CurrentTrafficLight.ColorSwitchTimer.Change(1000, Timeout.Infinite);
                CurrentTrafficLight.IsSwitchingDown = true;
            }
            else if (CurrentTrafficLight.Color == Colors.Yellow && CurrentTrafficLight.IsSwitchingDown)
            {
                CurrentTrafficLight.ColorSwitchTimer.Change(3000, Timeout.Infinite);
                CurrentTrafficLight.Color = Colors.Green;
                CurrentTrafficLight.IsSwitchingDown = false;
            }
            else if (CurrentTrafficLight.Color == Colors.Yellow && !CurrentTrafficLight.IsSwitchingDown)
            {
                CurrentTrafficLight.ColorSwitchTimer.Change(3000, Timeout.Infinite);
                CurrentTrafficLight.Color = Colors.Red;
            }
            else if (CurrentTrafficLight.Color == Colors.Green)
            {
                CurrentTrafficLight.ColorSwitchTimer.Change(1000, Timeout.Infinite);
                CurrentTrafficLight.Color = Colors.Yellow;
            }
            try
            {//TODO fix sending params
                _hubContext.Clients.All.SendAsync("ReceiveColor", CurrentTrafficLight.Color.ToString(), CurrentTrafficLight.Id);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Changes?.Invoke();
        }
        private async Task SaveCurrentTrafficLightToDBAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {            
            try
            {
                var services = serviceProvider.CreateScope().ServiceProvider;
                var repository = services.GetRequiredService<TrafficLightRepository>();
                var lightsContext = services.GetRequiredService<TraficLightsContext>();

                var firstTraficLight = lightsContext.Lights.Where(l => l.Id == CurrentTrafficLight.Id).FirstOrDefault();
                Console.WriteLine();

              
                firstTraficLight.Color = CurrentTrafficLight.Color;
                firstTraficLight.Date = DateTime.Now;
                await lightsContext.SaveChangesAsync();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
/*
        private async Task SaveCurrentTrafficLightToDBAsync(CancellationToken cancellationToken)
        {
            var trafficLightFromDb = await _repository.GetByIdAsync(CurrentTrafficLight.Id, cancellationToken);
            if(trafficLightFromDb is null)
            {
                return;
            }
            else
            {
                trafficLightFromDb.Color = CurrentTrafficLight.CurrentColor.ToString();
                trafficLightFromDb.Time = DateTime.Now;
                await _lightsContext.SaveChangesAsync();
            }
        }*/
        public async Task SetCurrentColorFromDBAsync(CancellationToken cancellationToken)
        {
            var traficLights = await _repository.GetByIdAsync(CurrentTrafficLight.Id, cancellationToken).ConfigureAwait(false);
            if (traficLights.Color.Equals("red"))
            {
                CurrentTrafficLight.Color = Colors.Red;
            }
            else if (traficLights.Color.Equals("yellow"))
            {
                CurrentTrafficLight.Color = Colors.Yellow;
            }
            else
            {
                CurrentTrafficLight.Color = Colors.Green;
            }
            return;
        }
        
    }
}
