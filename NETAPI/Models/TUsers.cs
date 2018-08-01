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
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace NETAPI.Models
{
    public class TUsers
    {
        public const string UIDFIELD = "user_id";
        public const string TIDFIELD = "tour_id";
        public const string IDFIELD = "_id";

        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement(elementName: IDFIELD)]
        public BsonObjectId Id { get; set; }
        [BsonElement(elementName: UIDFIELD)]
        [JsonProperty(PropertyName = UIDFIELD)]
        [BsonRequired]
        public BsonObjectId UId { get; set; }
        [BsonElement(elementName: TIDFIELD)]
        [JsonProperty(PropertyName = TIDFIELD)]
        [BsonRequired]
        public BsonObjectId TId { get; set; }
        

    }
}

