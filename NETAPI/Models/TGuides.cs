using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETAPI.Models
{
    public class TGuides
    {
        public const string IDFIELD = "_id";
        public const string GIDFIELD = "guide_id";
        public const string TIDFIELD = "tour_id";

        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement(elementName: IDFIELD)]
        public BsonObjectId Id { get; set; }
        [BsonElement(elementName: GIDFIELD)]
        [BsonRequired]
        public BsonObjectId GId { get; set; }
        [BsonElement(elementName: TIDFIELD)]
        [BsonRequired]
        public BsonObjectId TId { get; set; }
        [BsonIgnore]
        public string Name { get; set; }

    }
}
