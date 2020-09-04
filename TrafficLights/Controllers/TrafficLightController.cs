using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrafficLights.Data;
using TrafficLights.Models;

namespace TrafficLights.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class TrafficLightController : Controller
    {
        ApplicationContext db;
        public TrafficLightController(ApplicationContext context)
        {
            db = context;
            if (!db.TrafficLights.Any())
            {
                db.TrafficLights.Add(new TrafficLightEntity { Color = "red" });
                db.TrafficLights.Add(new TrafficLightEntity { Color = "yellow" });
                db.TrafficLights.Add(new TrafficLightEntity { Color = "green" });
                db.SaveChanges();
            }
        }
        [HttpGet]
        public IEnumerable<TrafficLightEntity> Get()
        {
            return db.TrafficLights.ToList();
        }

        [HttpGet("{id}")]
        public TrafficLightEntity Get(int id)
        {
            TrafficLightEntity product = db.TrafficLights.FirstOrDefault(x => x.Id == id);
            return product;
        }

        [HttpPost]
        public IActionResult Post(TrafficLightEntity product)
        {
            if (ModelState.IsValid)
            {
                db.TrafficLights.Add(product);
                db.SaveChanges();
                return Ok(product);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put(TrafficLightEntity product)
        {
            if (ModelState.IsValid)
            {
                db.Update(product);
                db.SaveChanges();
                return Ok(product);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            TrafficLightEntity trafficLight = db.TrafficLights.FirstOrDefault(x => x.Id == id);
            if (trafficLight != null)
            {
                db.TrafficLights.Remove(trafficLight);
                db.SaveChanges();
            }
            return Ok(trafficLight);
        }
    }
}

