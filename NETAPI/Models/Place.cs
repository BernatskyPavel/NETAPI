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

namespace NETAPI.Models
{
    public class Place
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        [BsonElement("_id")]
        public BsonObjectId Id { get; set; }
        [BsonElement("name")]
        [BsonRequired]
        public BsonString Name { get; set; }
        [BsonElement("description")]
        public BsonString Description { get; set; }
        [BsonElement("photos")]
        public BsonArray Photos { get; set; }
        [BsonElement("coordinates")]
        [BsonRequired]
        public BsonArray Coordinates { get; set; }
    }
}