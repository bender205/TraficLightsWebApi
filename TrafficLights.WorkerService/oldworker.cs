using System;
using System.Collections.Generic;
using System.Text;

namespace TrafficLights.WorkerService
{
    class oldworker
    {
        /*
         
        using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TraficLightsRazorPages.Core.Hubs;
using TraficLightsRazorPages.Data;
using TraficLightsRazorPages.Models;
using TraficLightsRazorPages.Models.Interfaces;

namespace TrafficLights.WorkerService
{
    public class Worker : BackgroundService, IHostedService, IDisposable
    {
        #region Fields
        private Timer _timer { get;  }
        private List<TrafficLight> ActiveTrafficLights { get; set; }
        
        public delegate void ChangeColorHandler();

        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<TraficLightsHub> _hubContext;

        private readonly TrafficLightRepository _repository;

        public event ChangeColorHandler Changes;
        private IServiceProvider _serviceProvider;
        #endregion
        #region Constructors


        public Worker(ILogger<Worker> logger, IHubContext<TraficLightsHub> hubContext, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider.CreateScope().ServiceProvider;
            _repository = _serviceProvider.GetRequiredService<TrafficLightRepository>();
            this._logger = logger;
            _hubContext = hubContext;
        }
        #endregion
        #region Methods
        public async void AddTrafficLight(TrafficLight trafficLight)
        {
           if(this.ActiveTrafficLights == null)
            {
                await Task.Run(() =>
                {
                    this.ActiveTrafficLights = new List<TrafficLight>();
                    this.ActiveTrafficLights.Add(trafficLight);
                });
                ;
            }
           if(ActiveTrafficLights.FirstOrDefault(t => t.Id == trafficLight.Id) == null)
            {
                 await Task.Run(() => 
                 {
                     ActiveTrafficLights.Add(trafficLight);
                 });   
            }
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        public void InvokeChanges()
        {
            Changes?.Invoke();
        }
        /// <summary>
        /// Use this method to start ExecuteAsync method 
        /// </summary>
        /// <param name="currentTrafficLight">traffic light </param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.ExecuteAsync(cancellationToken);
            #region old
            /*   await Task.Run(() =>
               {
                   currentTrafficLight.ColorSwitchTimer = new Timer(SwitchNextColor, currentTrafficLight, 0, Timeout.Infinite);
                   Changes += () => SaveCurrentTrafficLightToDBAsync(currentTrafficLight, _serviceProvider, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
               });       */
/*#endregion
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        *//*  _timer?.Change(Timeout.Infinite, 0);*//*
        return Task.CompletedTask;
    }
    /// <summary>
    /// Used to do some work with trafficlight
    /// </summary>
    /// <param name="stoppingToken">Cancellation token</param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.AddTrafficLight(new TrafficLight() { Id = 1, Color = Colors.Red, Date = DateTime.UtcNow, IsSwitchingDown = true });
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(2000);
            if (ActiveTrafficLights != null)
            {
                foreach (var trafficLight in this.ActiveTrafficLights)
                {
                    _logger.LogDebug($"traffic light {trafficLight.Id} changed color to {trafficLight.Color}, date: {trafficLight.Date} ! \n");
                    await SwitchNextColor(trafficLight);
                    await SaveTrafficLightToDBAsync(trafficLight, _serviceProvider, CancellationToken.None);
                }
            }
        }
    }
    public async Task SwitchNextColor(object trafficLight)
    {
        TrafficLight currentTrafficLight = trafficLight as TrafficLight;
        if (trafficLight != null)
        {
            if (currentTrafficLight.Color == Colors.Red)
            {
                currentTrafficLight.Color = Colors.Yellow;
                // currentTrafficLight.ColorSwitchTimer.Change(2000, Timeout.Infinite);
                currentTrafficLight.IsSwitchingDown = true;
            }
            else if (currentTrafficLight.Color == Colors.Yellow && currentTrafficLight.IsSwitchingDown)
            {
                //   currentTrafficLight.ColorSwitchTimer.Change(2000, Timeout.Infinite);
                currentTrafficLight.Color = Colors.Green;
                currentTrafficLight.IsSwitchingDown = false;
            }
            else if (currentTrafficLight.Color == Colors.Yellow && !currentTrafficLight.IsSwitchingDown)
            {
                // currentTrafficLight.ColorSwitchTimer.Change(2000, Timeout.Infinite);
                currentTrafficLight.Color = Colors.Red;
            }
            else if (currentTrafficLight.Color == Colors.Green)
            {
                //currentTrafficLight.ColorSwitchTimer.Change(2000, Timeout.Infinite);
                currentTrafficLight.Color = Colors.Yellow;
            }
            try
            {//TODO fix sending params
                TrafficLightEntity hubLight = new TrafficLightEntity() { Id = currentTrafficLight.Id, Color = currentTrafficLight.Color, Date = currentTrafficLight.Date };
                await _hubContext.Clients.All.SendAsync("ReceiveColor", hubLight);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Changes?.Invoke();
        }
    }
    private async Task SaveTrafficLightToDBAsync(TrafficLight currentTrafficLight, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        try
        {
            var services = serviceProvider.CreateScope().ServiceProvider;
            var repository = services.GetRequiredService<TrafficLightRepository>();
            var lightsContext = services.GetRequiredService<TraficLightsContext>();
            var firstTraficLight = lightsContext.Lights.Where(l => l.Id == currentTrafficLight.Id).FirstOrDefault();
            firstTraficLight.Color = currentTrafficLight.Color;
            firstTraficLight.Date = DateTime.Now;
            await lightsContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }*/
    /*
            public async Task SetCurrentColorFromDBAsync(TrafficLight currentTrafficLight, CancellationToken cancellationToken)
            {
                var traficLights = await _repository.GetByIdAsync(currentTrafficLight.Id, cancellationToken).ConfigureAwait(false);
                if (traficLights.Color == Colors.Red)
                {
                    currentTrafficLight.Color = Colors.Red;
                }
                else if (traficLights.Color == Colors.Yellow)
                {
                    currentTrafficLight.Color = Colors.Yellow;
                }
                else
                {
                    currentTrafficLight.Color = Colors.Green;
                }
    //TODO return Task.CompletedTask
                return ;
            }*/
//#endregion
    #region Comments
    /*
      public Task StartAsync(CancellationToken cancellationToken)
{
    _log.LogInformation("RecureHostedService is Starting");
    _timer = new Timer(DoWork,null,TimeSpan.Zero, TimeSpan.FromSeconds(5));
    return Task.CompletedTask;
}

public Task StopAsync(CancellationToken cancellationToken)
{
    _log.LogInformation("RecureHostedService is Stopping");
    _timer?.Change(Timeout.Infinite, 0);
    return Task.CompletedTask;
}
private void DoWork(object state)
{
    _log.LogInformation("Timed Background Service is working.");
}
     */
    #endregion
}
}

         
      //   */
//    }
//}