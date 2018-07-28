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
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement("_id")]
        public BsonObjectId Id { get; set; }
        [BsonElement("tour_id")]
        [BsonRequired]
        public BsonObjectId TId { get; set; }
        [BsonElement("city_id")]
        [BsonRequired]
        public BsonObjectId CId { get; set; }
        [BsonElement("from")]
        [JsonIgnore]
        public DateTime? Start { get; set; }
        [BsonElement("to")]
        [JsonIgnore]
        public DateTime? End { get; set; }
        [BsonElement("status")]
        [BsonRequired]
        public string Status { get; set; }
        [BsonIgnore]
        public string Name { get; set; }
        [BsonIgnore]
        public string Description { get; set; }
        [BsonIgnore]
        public List<string> Photos { get; set; }
        [BsonIgnore]
        public List<double> Coordinates { get; set; }

        TCities()
        {
            Description = Name = string.Empty;
            Photos = new List<string>();
            Coordinates = new List<double>();
        }
    }
}
