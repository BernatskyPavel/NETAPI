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
    public class Guide
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        [BsonElement("_id")]
        public BsonObjectId Id { get; set; }
        [BsonElement("name")]
        [BsonRequired]
        public BsonString Name { get; set; }
        [BsonElement("phone")]
        [BsonRequired]
        public BsonString Phone { get; set; }
        [BsonElement("schedule")]
        [BsonRequired]
        public BsonString Schedule { get; set; }
    }

    /*
     
     _id
:
5b56fed8a4d9e305c8f3e562
name
:
"guideName0"
phone
:
"guidePhone0"
schedule
:
"guideSchedule0"
     */
}
