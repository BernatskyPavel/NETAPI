using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using NETAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.Memory;

namespace NETAPI
{
    public class MongoService : IHostedService, IDisposable
    {
        private Timer _timer;
        private MongoContext context = new MongoContext();
        private readonly IMemoryCache _distributedCache;
        private const int ServiceTimeSpan = 5;

        public MongoService(IMemoryCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
           TimeSpan.FromMinutes(ServiceTimeSpan));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var tourtask = context.Tours.FindAsync(filter: new FilterDefinitionBuilder<Tour>().AnyIn(field: Tour.STATUSFIELD, values: new List<string>() { Status.TOURPRESENT, Status.TOURFUTURE }));
            var citytask = context.TCities.FindAsync(filter: new FilterDefinitionBuilder<TCities>().AnyIn(field: TCities.STATUSFIELD, values: new List<string>() { Status.CITYPRESENT, Status.CITYFUTURE }));
            var placetask = context.TPlaces.FindAsync(filter: new FilterDefinitionBuilder<TPlaces>().AnyIn(field: TPlaces.STATUSFIELD, values: new List<string>() { Status.PLACEPRESENT, Status.PLACEFUTURE }));

            var tlist = await tourtask;
            var clist = await citytask;
            var plist = await placetask;

            List<Tour> tours = await tlist.ToListAsync();
            List<TCities> cities = await clist.ToListAsync();
            List<TPlaces> places = await plist.ToListAsync();

            List<BsonObjectId> present = new List<BsonObjectId>();
            List<BsonObjectId> past = new List<BsonObjectId>();
            List<BsonObjectId> future = new List<BsonObjectId>();
            DateTime current = DateTime.Now;
            Cache cache = new Cache();

            if (tours != null)
            {
                Dictionary<BsonObjectId, string> tchanges = new Dictionary<BsonObjectId, string>();
                present.Clear(); past.Clear(); future.Clear();

                tours.ForEach(x =>
                {
                    switch (x.Status)
                    {
                        case Status.TOURPRESENT:
                            if (x.End < current)
                            {
                                tchanges.Add(x.Id, Status.TOURPAST);
                                cache.TourChgs.Add(new Cache.TourChanges(id: x.Id, newStatus: Status.TOURPAST));
                            }
                            else if (x.Start > current)
                            {
                                tchanges.Add(x.Id, Status.TOURFUTURE);
                                cache.TourChgs.Add(new Cache.TourChanges(id: x.Id, newStatus: Status.TOURFUTURE));
                            }
                            break;
                        case Status.TOURFUTURE:
                            if (x.Start < current && x.End > current)
                            {
                                tchanges.Add(x.Id, Status.TOURPRESENT);
                                cache.TourChgs.Add(new Cache.TourChanges(id: x.Id, newStatus: Status.TOURPRESENT));
                            }
                            else if (x.End < current)
                            {
                                tchanges.Add(x.Id, Status.TOURPAST);
                                cache.TourChgs.Add(new Cache.TourChanges(id: x.Id, newStatus: Status.TOURPAST));
                            }
                            break;
                        default:
                            break;
                    }
                });

                NewMethod(present, past, future, tchanges);
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
                context.Tours.UpdateManyAsync(filter: new FilterDefinitionBuilder<Tour>().AnyIn(Tour.IDFIELD, present), update: new UpdateDefinitionBuilder<Tour>().Set(x => x.Status, Status.TOURPRESENT));
                context.Tours.UpdateManyAsync(filter: new FilterDefinitionBuilder<Tour>().AnyIn(Tour.IDFIELD, future), update: new UpdateDefinitionBuilder<Tour>().Set(x => x.Status, Status.TOURFUTURE));
                context.Tours.UpdateManyAsync(filter: new FilterDefinitionBuilder<Tour>().AnyIn(Tour.IDFIELD, past), update: new UpdateDefinitionBuilder<Tour>().Set(x => x.Status, Status.TOURPAST));
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
            }
            if (cities != null)
            {
                Dictionary<BsonObjectId, string> cchanges = new Dictionary<BsonObjectId, string>();
                present.Clear(); past.Clear(); future.Clear();
                cities.ForEach(x =>
                {
                    switch (x.Status)
                    {
                        case Status.CITYPRESENT:
                            if (x.End < current)
                            {
                                cchanges.Add(x.Id, Status.CITYPAST);
                                cache.CityChgs.Add(new Cache.CityChanges(id: x.Id, newStatus: Status.CITYPAST));
                            }
                            else if (x.Start > current)
                            {
                                cchanges.Add(x.Id, Status.CITYFUTURE);
                                cache.CityChgs.Add(new Cache.CityChanges(id: x.Id, newStatus: Status.CITYFUTURE));
                            }
                            break;
                        case Status.CITYFUTURE:
                            if (x.Start < current && x.End > current)
                            {
                                cchanges.Add(x.Id, Status.CITYPRESENT);
                                cache.CityChgs.Add(new Cache.CityChanges(id: x.Id, newStatus: Status.CITYPRESENT));
                            }
                            else if (x.End < current)
                            {
                                cchanges.Add(x.Id, Status.CITYPAST);
                                cache.CityChgs.Add(new Cache.CityChanges(id: x.Id, newStatus: Status.CITYPAST));
                            }
                            break;
                        default:
                            break;
                    }
                });

                NewMethod(present, past, future, cchanges);
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
                context.TCities.UpdateManyAsync(filter: new FilterDefinitionBuilder<TCities>().AnyIn(field: TCities.IDFIELD, values: present), update: new UpdateDefinitionBuilder<TCities>().Set(x => x.Status, Status.CITYPRESENT));
                context.TCities.UpdateManyAsync(filter: new FilterDefinitionBuilder<TCities>().AnyIn(field: TCities.IDFIELD, values: past), update: new UpdateDefinitionBuilder<TCities>().Set(x => x.Status, Status.CITYPAST));
                context.TCities.UpdateManyAsync(filter: new FilterDefinitionBuilder<TCities>().AnyIn(field: TCities.IDFIELD, values: future), update: new UpdateDefinitionBuilder<TCities>().Set(x => x.Status, Status.CITYFUTURE));
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
            }
            if (places != null)
            {
                Dictionary<BsonObjectId, string> pchanges = new Dictionary<BsonObjectId, string>();
                present.Clear(); past.Clear(); future.Clear();
                places.ForEach(x =>
                {
                    switch (x.Status)
                    {
                        case Status.PLACEPRESENT:
                            if (x.End < current)
                            {
                                pchanges.Add(x.Id, Status.PLACEPAST);
                                cache.PlaceChgs.Add(new Cache.PlaceChanges(id: x.Id, newStatus: Status.PLACEPAST));
                            }
                            else if (x.Start > current)
                            {
                                pchanges.Add(x.Id, Status.PLACEFUTURE);
                                cache.PlaceChgs.Add(new Cache.PlaceChanges(id: x.Id, newStatus: Status.PLACEFUTURE));
                            }
                            break;
                        case Status.PLACEFUTURE:
                            if (x.Start < current && x.End > current)
                            {
                                pchanges.Add(x.Id, Status.PLACEPRESENT);
                                cache.PlaceChgs.Add(new Cache.PlaceChanges(id: x.Id, newStatus: Status.PLACEPRESENT));
                            }
                            else if (x.End < current)
                            {
                                pchanges.Add(x.Id, Status.PLACEPAST);
                                cache.PlaceChgs.Add(new Cache.PlaceChanges(id:x.Id, newStatus: Status.PLACEPAST));
                            }
                            break;
                        default:
                            break;
                    }
                });

                NewMethod(present, past, future, pchanges);
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
                context.TPlaces.UpdateManyAsync(filter: new FilterDefinitionBuilder<TPlaces>().AnyIn(field: TPlaces.IDFIELD, values: present), update: new UpdateDefinitionBuilder<TPlaces>().Set(x => x.Status, Status.PLACEPRESENT));
                context.TPlaces.UpdateManyAsync(filter: new FilterDefinitionBuilder<TPlaces>().AnyIn(field: TPlaces.IDFIELD, values: past), update: new UpdateDefinitionBuilder<TPlaces>().Set(x => x.Status, Status.PLACEPAST));
                context.TPlaces.UpdateManyAsync(filter: new FilterDefinitionBuilder<TPlaces>().AnyIn(field: TPlaces.IDFIELD, values: future), update: new UpdateDefinitionBuilder<TPlaces>().Set(x => x.Status, Status.PLACEFUTURE));
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
                _distributedCache.Set(Properties.Resources.CacheString, cache, TimeSpan.FromMinutes(5));

            }

        }

        private static void NewMethod(List<BsonObjectId> present, List<BsonObjectId> past, List<BsonObjectId> future, Dictionary<BsonObjectId, string> tchanges)
        {
            present.Clear(); past.Clear(); future.Clear();
            tchanges.ToList().ForEach(x =>
            {
                switch (x.Value)
                {
                    case Status.TOURPRESENT:
                        present.Add(x.Key);
                        break;
                    case Status.TOURPAST:
                        past.Add(x.Key);
                        break;
                    case Status.TOURFUTURE:
                        future.Add(x.Key);
                        break;
                    default: break;
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
