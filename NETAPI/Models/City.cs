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
using System.Numerics;
using Newtonsoft.Json;

namespace NETAPI.Models
{
    public class City
    {
        public const string IDFIELD = "_id";
        public const string NAMEFIELD = "name";
        public const string ABOUTFIELD = "description";
        public const string PHOTOFIELD = "photos";
        public const string POSFIELD = "coordinates";

        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement(elementName: IDFIELD)]
        public BsonObjectId Id { get; set; }
        [BsonElement(elementName: NAMEFIELD)]
        [BsonRequired]
        public string Name { get; set; }
        [BsonElement(elementName: ABOUTFIELD)]
        public string Description { get; set; }
        [BsonElement(elementName: PHOTOFIELD)]
        public List<string> Photos { get; set; }
        [BsonElement(elementName: POSFIELD)]
        [JsonIgnore]
        public List<double> Coordinates { get; set; }

    }
}
