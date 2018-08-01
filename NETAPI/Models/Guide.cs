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
using Newtonsoft.Json;

namespace NETAPI.Models
{
    public class Guide
    {
        public const string IDFIELD = "_id";
        public const string PHONEFIELD = "phone";
        public const string UIDFIELD = "user_id";

        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement(elementName: IDFIELD)]
        public BsonObjectId Id { get; set; }
        [BsonElement(elementName: PHONEFIELD)]
        [BsonRequired]
        public string Phone { get; set; }
        [BsonElement(elementName: UIDFIELD)]
        [BsonRequired]
        [JsonIgnore]
        public BsonObjectId UId { get; set; }
        [BsonIgnore]
        public BsonObjectId TId { get; set; }
        [BsonIgnore]
        public string Name { get; set; }
    }
}
