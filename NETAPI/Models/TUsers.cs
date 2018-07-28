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
    public class TUsers
    {
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement("_id")]
        public BsonObjectId Id { get; set; }
        [BsonElement("user_id")]
        [BsonRequired]
        public BsonObjectId UId { get; set; }
        [BsonElement("tour_id")]
        [BsonRequired]
        public BsonObjectId TId { get; set; }

    }
}

