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
        public const string IDFIELD = "_id";
        public const string NAMEFIELD = "name";
        public const string EMAILFIELD = "email";
        public const string PASSFIELD = "hpassword";

        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement(elementName: IDFIELD)]
        public BsonObjectId Id { get; set; }
        [BsonElement(elementName: NAMEFIELD)]
        [BsonRequired]
        public string Name { get; set; }
        [BsonElement(elementName: EMAILFIELD)]
        [BsonRequired]
        public string Email { get; set; }
        [BsonElement(elementName: PASSFIELD)]
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
            List<TUsers> tusers = await context.TUsers.Find(filter: new BsonDocument(TUsers.UIDFIELD, new BsonObjectId(new ObjectId(Id.ToString())))).ToListAsync();
            tusers.ForEach((x) => { Tours.Add(x.TId); });
        }
    }
}

