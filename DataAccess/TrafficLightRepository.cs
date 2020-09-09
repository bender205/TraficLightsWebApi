using System.Linq;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TraficLightsRazorPages.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TraficLightsRazorPages.Data
{
    public class TrafficLightRepository
    {
        private readonly TraficLightsContext _databaseContext;
        public TrafficLightRepository(TraficLightsContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<TrafficLightEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _databaseContext.Lights.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        // TODO there was public async void
        public async Task AddTrafficLightAsync(TrafficLightEntity traficLight, CancellationToken cancellationToken)
        {
            await _databaseContext.AddAsync(traficLight);
            //TODO is it a good practice? 
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<int> GetMaxTraficLightsIdAsync(CancellationToken cancellationToken)
        {
            return await _databaseContext.Lights.MaxAsync(t => t.Id, cancellationToken);
        }
        public IEnumerable<TrafficLightEntity> GetAllTraficLights()
        {
            return _databaseContext.Lights;
        }
    }
}
