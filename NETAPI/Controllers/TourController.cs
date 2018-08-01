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
using System.Web;
using Microsoft.Net.Http;
using Microsoft.Extensions.Caching.Memory;

namespace NETAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private MongoContext context = new MongoContext();
        private const int LIMIT = 50;
        private readonly IMemoryCache _cache;

        public TourController(IMemoryCache cache)
        {
            _cache = cache;
        }

        // GET: api/Tour
        [HttpGet]
        public async Task<List<Tour>> Get([FromQuery] int page = 1)
        {
            try
            {
                var prelist = await context.Tours.Find(filter: new FilterDefinitionBuilder<Tour>().Ne(Tour.STATUSFIELD, Status.TOURPAST)).SortByDescending(x => x.Created).Skip(LIMIT * (1 - page)).Limit(LIMIT).ToListAsync();
                if (prelist.Count == Decimal.Zero)
                {
                    throw new Exception();
                }
                List<BsonObjectId> tids = new List<BsonObjectId>();
                prelist.ForEach(x => tids.Add(x.Id));
                List<TGuides> clist = await context.TGuides.Find(filter: new FilterDefinitionBuilder<TGuides>().AnyIn(TGuides.TIDFIELD, tids)).ToListAsync() ?? new List<TGuides>();
                List<BsonObjectId> gids = new List<BsonObjectId>();
                clist.ForEach(x => gids.Add(x.GId));
                List<Guide> glist = await context.Guides.Find(filter: new FilterDefinitionBuilder<Guide>().AnyIn(Guide.IDFIELD, gids)).ToListAsync() ?? new List<Guide>();
                List<BsonObjectId> uids = new List<BsonObjectId>();
                glist.ForEach(x => uids.Add(x.UId));
                List<User> ulist = await context.Users.Find(filter: new FilterDefinitionBuilder<User>().AnyIn(NETAPI.Models.User.IDFIELD, uids)).ToListAsync() ?? new List<User>();
                clist.ForEach(x =>
                {
                    x.GId = glist.Where(g => g.Id == x.GId).FirstOrDefault()?.UId;
                });
                prelist.ForEach(x =>
                {
                    x.StringifyDate();
                    var buff = clist.Where(g => g.TId == x.Id).FirstOrDefault();
                    if (buff != null)
                    {
                        x.Guide = ulist.Where(u => u.Id == buff.GId).FirstOrDefault()?.Name;
                    }
                });
                return prelist;
            }
            catch (Exception)
            { Response.StatusCode = StatusCodes.Status500InternalServerError; return null; }
        }

        // GET: api/Tour/5
        [HttpGet("{id}")]
        public async Task<Tour> Get(string id)
        {
            try
            {
                Tour tour = await context.Tours.Find(filter: new BsonDocument(Tour.IDFIELD, new BsonObjectId(new ObjectId(id)))).FirstOrDefaultAsync();
                if (tour == null)
                {
                    throw new Exception();
                }
                tour.StringifyDate();
                TGuides tguide = await context.TGuides.Find(filter: new BsonDocument(TGuides.TIDFIELD, tour.Id)).FirstOrDefaultAsync();
                Guide guide = null;
                if (tguide != null)
                {
                    guide = await context.Guides.Find(filter: new BsonDocument(Guide.IDFIELD, tguide.GId)).FirstOrDefaultAsync();
                }

                User user = null;
                if (guide != null)
                {
                    user = await context.Users.Find(filter: new BsonDocument(Models.User.IDFIELD, guide.UId)).FirstOrDefaultAsync();
                }

                if (user != null)
                {
                    tour.Guide = user.Name;
                }
                return tour;
            }
            catch (Exception)
            { Response.StatusCode = StatusCodes.Status500InternalServerError; return null; }
        }

        [HttpGet("{id}/city")]
        public async Task<List<TCities>> GetCities(string id)
        {
            try
            {
                List<TCities> list = await context.TCities.Find(filter: new BsonDocument(TCities.TIDFIELD, new BsonObjectId(new ObjectId(id)))).SortBy(x => x.Start).ToListAsync();
                if (list.Count == Decimal.Zero)
                {
                    throw new Exception();
                }
                List<BsonObjectId> tids = new List<BsonObjectId>();
                list.ForEach(x => tids.Add(x.CId));
                tids.Distinct();
                FilterDefinition<City> filter = new FilterDefinitionBuilder<City>().AnyIn<BsonObjectId>(City.IDFIELD, tids);
                List<City> clist = await context.Cities.Find(filter: filter).ToListAsync();
                list.ForEach(x =>
                {
                    City city = clist.Where(y => y.Id == x.CId).First();
                    x.Coordinates = city.Coordinates;
                    x.Description = city.Description;
                    x.Photos = city.Photos;
                    x.Name = city.Name;
                });
                return list;
            }
            catch (Exception)
            { Response.StatusCode = StatusCodes.Status500InternalServerError; return null; }
        }

        [HttpGet("{id}/guide")]
        public async Task<Guide> GetGuide(string id)
        {
            try
            {
                var tguide = await context.TGuides.Find(filter: new BsonDocument(TGuides.TIDFIELD, new BsonObjectId(new ObjectId(id)))).FirstOrDefaultAsync();
                if (tguide == null)
                {
                    throw new Exception();
                }
                Guide guide = await context.Guides.Find(filter: new BsonDocument(Guide.IDFIELD, tguide.GId)).FirstOrDefaultAsync();
                User user = null;
                if (guide != null)
                {
                    user = await context.Users.Find(filter: new BsonDocument(Models.User.IDFIELD, guide.UId)).FirstOrDefaultAsync();
                }
                if (user != null)
                {
                    guide.Name = user.Name;
                    guide.TId = new BsonObjectId(new ObjectId(id));
                }
                return guide;
            }
            catch (Exception)
            { Response.StatusCode = StatusCodes.Status500InternalServerError; return null; }
        }
        [Route("{id}/city/{cid}")]
        [HttpGet]
        public async Task<List<TPlaces>> GetPlaces(string id, string cid)
        {
            try
            {
                List<BsonElement> elements = new List<BsonElement>
                {
                    new BsonElement(TCities.TIDFIELD, new BsonObjectId(new ObjectId(id))),
                    new BsonElement(TCities.IDFIELD, new BsonObjectId(new ObjectId(cid)))
                };
                TCities list = await context.TCities.Find(filter: new BsonDocument(elements)).FirstOrDefaultAsync();
                if (list == null)
                {
                    return null;
                }
                List<TPlaces> plist = new List<TPlaces>();
                plist = await context.TPlaces.Find(filter: new BsonDocument(new BsonElement(TPlaces.FIDFIELD, list.Id))).ToListAsync();
                if (plist.Count == Decimal.Zero)
                {
                    return null;
                }

                List<BsonObjectId> tids = new List<BsonObjectId>();
                plist.ForEach(x => tids.Add(x.PId));


                List<Place> pplist = new List<Place>();
                pplist = await context.Places.Find(filter: new FilterDefinitionBuilder<Place>().AnyIn<BsonObjectId>(Place.IDFIELD, tids)).ToListAsync();

                plist.ForEach(p =>
                {
                    p.StringifyDate();
                    var buff = pplist.Where(x => x.Id == p.PId).FirstOrDefault();
                    p.Name = buff?.Name;
                    p.Coordinates = $"{buff?.Coordinates[0].ToString()}:{buff?.Coordinates[1].ToString()}";
                    p.Description = buff?.Description;
                    p.Photos = buff?.Photos;
                    p.SetPhoto();
                    p.CId = cid;
                });


                return plist;
            }
            catch (Exception)
            { Response.StatusCode = StatusCodes.Status500InternalServerError; return null; }
        }

        [HttpPost]
        public async Task Post([FromBody] TourReg user)
        {
            try
            {
                if (user.TId == null || user.UId == null)
                {
                    throw new Exception("Missing Parameter");
                }
                TUsers reg = new TUsers();
                reg.UId = new BsonObjectId(new ObjectId(user.UId));
                reg.TId = new BsonObjectId(new ObjectId(user.TId));
                Tour usertour = await context.Tours.Find(filter: new BsonDocument(Tour.IDFIELD, new BsonObjectId(new ObjectId(user.TId)))).FirstOrDefaultAsync();
                if (usertour == null)
                {
                    throw new Exception("Database Error!");
                }
                if (!usertour.IsFreeSeats)
                {
                    throw new Exception("No Free Seats!");
                }
                if (usertour.Start < DateTime.Now || usertour.Status == Status.TOURPAST || usertour.Status == Status.TOURPRESENT)
                {
                    throw new Exception("Unavailable");
                }
                List<TUsers> list = await context.TUsers.Find(filter: new BsonDocument(TUsers.UIDFIELD, new BsonObjectId(new ObjectId(user.UId)))).ToListAsync();
                if (list.Where(t => t.TId.ToString() == user.TId).FirstOrDefault() != null)
                {
                    throw new Exception("Already Exist!");
                }
                List<BsonObjectId> tids = new List<BsonObjectId>();
                if (list.Count != Decimal.Zero)
                {
                    list.ForEach(u => tids.Add(u.TId));
                    List<Tour> tlist = await context.Tours.Find(filter: new FilterDefinitionBuilder<Tour>().AnyIn<BsonObjectId>(Tour.IDFIELD, tids)).ToListAsync();
                    tlist.RemoveAll(x => x.Status == Status.TOURPAST);

                    bool check = tlist.TrueForAll(x => x.Start > usertour.End || x.End < usertour.Start);
                    if (check)
                    {
                        await context.Tours.UpdateOneAsync<Tour>(x => x.Id == usertour.Id, new UpdateDefinitionBuilder<Tour>().Set(Tour.AVAILABLEFIELD, usertour.Free - 1));
                        await context.TUsers.InsertOneAsync(reg);
                    }
                    else throw new Exception("User have another tour in this time!");
                }
                else
                {
                    await context.Tours.UpdateOneAsync(x => x.Id == usertour.Id, new UpdateDefinitionBuilder<Tour>().Set(Tour.AVAILABLEFIELD, usertour.Free - 1));
                    await context.TUsers.InsertOneAsync(reg);
                }
            }
            catch (Exception exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await HttpContext.Response.WriteAsync(exception.Message.ToString());
            }

        }
    }
}
