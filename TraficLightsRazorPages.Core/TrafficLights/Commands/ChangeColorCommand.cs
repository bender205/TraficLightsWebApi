﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TraficLightsRazorPages.Core.Workers;
using TraficLightsRazorPages.Models.Interfaces;

namespace TraficLightsRazorPages.Core.TrafficLights.Queries
{

    public class ChangeColorCommand : INotification
    {
    }

    public class ChangeColorCommandHandler : INotificationHandler<ChangeColorCommand>
    {
        private readonly TraficLightsWorker _trafficWorker;

        public ChangeColorCommandHandler(TraficLightsWorker traffic)
        {
            _trafficWorker = traffic;
        }

        public Task Handle(ChangeColorCommand notification, CancellationToken cancellationToken)
        {
            _trafficWorker.CurrentTrafficLight.ColorSwitchTimer.Change(Timeout.Infinite, Timeout.Infinite);


            if (_trafficWorker.CurrentTrafficLight.Color == Colors.Red)
            {
                _trafficWorker.CurrentTrafficLight.Color = Colors.Yellow;
                _trafficWorker.CurrentTrafficLight.IsSwitchingDown = true;
            }
            else if (_trafficWorker.CurrentTrafficLight.Color == Colors.Yellow && _trafficWorker.CurrentTrafficLight.IsSwitchingDown)
            {
                _trafficWorker.CurrentTrafficLight.Color = Colors.Green;
                _trafficWorker.CurrentTrafficLight.IsSwitchingDown = false;
            }

            else if (_trafficWorker.CurrentTrafficLight.Color == Colors.Yellow && !_trafficWorker.CurrentTrafficLight.IsSwitchingDown)
            {
                this._trafficWorker.CurrentTrafficLight.Color = Colors.Red;
            }
            else if (_trafficWorker.CurrentTrafficLight.Color == Colors.Green)
            {
                this._trafficWorker.CurrentTrafficLight.Color = Colors.Yellow;
            }

            _trafficWorker.InvokeChanges();
            _trafficWorker.CurrentTrafficLight.ColorSwitchTimer.Change(500, Timeout.Infinite);

            return Task.CompletedTask;
        }
    }
}
