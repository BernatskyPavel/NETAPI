using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NETAPI.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using System.Runtime.Serialization.Json;
using Microsoft.AspNetCore.Authorization;
using System.Configuration;
using Newtonsoft.Json;

namespace NETAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        MongoContext context = new MongoContext();
        const int LIMIT = 20;
        // GET: api/Tour
        [HttpGet]
        public async Task<List<Tour>> Get([FromQuery] int page = 1)
        {
            var prelist = await context.Tours.Find(filter: new BsonDocument()).SortByDescending(x=>x.Created).Skip(LIMIT * (1-page)).Limit(LIMIT).ToListAsync();
            prelist.ForEach(x => x.StringifyDate());
            return prelist;
        }

        // GET: api/Tour/5
        [HttpGet("{id}")]
        public async Task<string> Get(string id)
        {
            var tour = await context.Tours.Find(filter: new BsonDocument("_id", new BsonObjectId(new ObjectId(id)))).FirstOrDefaultAsync();
            if(tour == null) return JsonConvert.SerializeObject(null);
            tour.StringifyDate();
            return JsonConvert.SerializeObject(tour);
;       }

        [HttpGet("{id}/city")]
        public async Task<string> GetCities(string id)
        {
            List<TCities> list = await context.TCities.Find(filter: new BsonDocument("tour_id", new BsonObjectId(new ObjectId(id)))).ToListAsync();
            if(list.Count == 0) return JsonConvert.SerializeObject(null);
            List<BsonObjectId> tids = new List<BsonObjectId>();
            list.ForEach(x => tids.Add(x.CId));
            tids.Distinct();
            FilterDefinition<City> filter = new FilterDefinitionBuilder<City>().AnyIn<BsonObjectId>("_id", tids);
            List<City> clist = await context.Cities.Find(filter: filter).ToListAsync();
            list.ForEach(x =>
            {
                City city = clist.Where(y => y.Id == x.CId).First();
                x.Coordinates = city.Coordinates;
                x.Description = city.Description;
                x.Photos = city.Photos;
                x.Name = city.Name;
            });
            return JsonConvert.SerializeObject(list);
        }

        // POST: api/Tour
        [HttpPost]
        public async Task<Tour> Post([FromBody] Tour value)
        {
            await context.Tours.InsertOneAsync(value);
            return await context.Tours.Find(filter: new BsonDocument("name", new BsonString(value.Name))).FirstAsync();
        }

        // PUT: api/Tour/5
        [HttpPut("{id}")]
        public void Put([FromQuery]int id, [FromBody] string value)
        {

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete([FromQuery]int id)
        {
        }
    }
}
