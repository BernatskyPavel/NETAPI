using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters;
using System.ComponentModel;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Numerics;

namespace NETAPI.Models
{
    public class User
    {
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement("_id")]
        public BsonObjectId Id { get; set; }
        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }
        [BsonElement("email")]
        [BsonRequired]
        public string Email { get; set; }
        [BsonElement("hpassword")]
        [BsonRequired]
        [JsonIgnore]
        public string HPass { get; set; }
        [BsonIgnore]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
        [BsonIgnore]
        [JsonProperty(PropertyName = "Tours")]
        private List<BsonObjectId> Tours { get; set; }
        public async Task SetTours(MongoContext context)
        {
            Tours = new List<BsonObjectId>();
            List<TUsers> tusers = await context.TUsers.Find(filter: new BsonDocument("user_id", new BsonObjectId(new ObjectId(Id.ToString())))).ToListAsync();
            tusers.ForEach((x) => { Tours.Add(x.TId); });
        }
    }
}

