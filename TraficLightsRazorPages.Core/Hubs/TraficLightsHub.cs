﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TraficLightsRazorPages.Core.TrafficLights.Queries;
using TraficLightsRazorPages.Models;

namespace TraficLightsRazorPages.Core.Hubs
{
    public class TraficLightsHub : Hub
    {
        private readonly IMediator _mediator;

        public TraficLightsHub(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task SendColor()
        {      
           await _mediator.Send(new ChangeColorCommand());
        }
    }
}
