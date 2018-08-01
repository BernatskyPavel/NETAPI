using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETAPI.Models
{
    public class TCities
    {
        public const string IDFIELD = "_id";
        public const string TIDFIELD = "tour_id";
        public const string CIDFIELD = "city_id";
        public const string ARRIVALFIELD = "from";
        public const string DEPARTUREFIELD = "to";
        public const string STATUSFIELD = "status";

        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement(elementName: IDFIELD)]
        public BsonObjectId Id { get; set; }
        [BsonElement(elementName: TIDFIELD)]
        [BsonRequired]
        public BsonObjectId TId { get; set; }
        [BsonElement(elementName: CIDFIELD)]
        [BsonRequired]
        public BsonObjectId CId { get; set; }
        [BsonElement(elementName: ARRIVALFIELD)]
        [JsonIgnore]
        public DateTime? Start { get; set; }
        [BsonElement(elementName: DEPARTUREFIELD)]
        [JsonIgnore]
        public DateTime? End { get; set; }
        [BsonElement(elementName: STATUSFIELD)]
        [BsonRequired]
        public string Status { get; set; }
        [BsonIgnore]
        public string Name { get; set; }
        [BsonIgnore]
        public string Description { get; set; }
        [BsonIgnore]
        [JsonIgnore]
        public List<string> Photos { get; set; }
        [BsonIgnore]
        [JsonIgnore]
        public List<double> Coordinates { get; set; }

        TCities()
        {
            Description = Name = string.Empty;
            Photos = new List<string>();
            Coordinates = new List<double>();
        }
    }
}
