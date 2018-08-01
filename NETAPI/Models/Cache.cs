using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NETAPI.Models
{
    [Serializable]
    public class Cache
    {
        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class TourChanges
        {
            private readonly BsonObjectId _id;
            private readonly string _newStatus;

            public BsonObjectId Id { get => _id; }
            public string Status { get => _newStatus; }

            public TourChanges(BsonObjectId id, string newStatus)
            {
                _id = id;
                _newStatus = newStatus;
            }
        }
        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class CityChanges
        {
            private readonly BsonObjectId _id;
            private readonly string _newStatus;

            public BsonObjectId Id { get => _id; }
            public string Status { get => _newStatus; }

            public CityChanges(BsonObjectId id, string newStatus)
            {
                _id = id;
                _newStatus = newStatus;
            }
        }
        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class PlaceChanges
        {
            private readonly BsonObjectId _id;
            private readonly string _newStatus;


            public BsonObjectId Id { get => _id; }
            public string Status { get => _newStatus; }

            public PlaceChanges(BsonObjectId id, string newStatus)
            {
                _id = id;
                _newStatus = newStatus;
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TourChanges> TourChgs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CityChanges> CityChgs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<PlaceChanges> PlaceChgs { get; set; }
        [JsonIgnore]
        public DateTime Created { get; }

        public Cache()
        {
            TourChgs = new List<TourChanges>();
            CityChgs = new List<CityChanges>();
            PlaceChgs = new List<PlaceChanges>();
            Created = DateTime.Now;
        }
    }


}
