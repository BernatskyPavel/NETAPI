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
    public class Tour
    {
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement("_id")]
        public BsonObjectId Id { get; set; }
        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("allSeats")]
        [BsonRequired]
        [JsonIgnore]
        public int All { get; set; }
        [BsonElement("status")]
        [BsonRequired]
        public string Status { get; set; }
        [BsonElement("freeSeats")]
        [BsonRequired]
        public int Free { get; set; }
        [BsonElement("price")]
        [BsonRequired]
        public int Price { get; set; }
        [BsonElement("from")]
        [BsonRequired]
        [JsonIgnore]
        public DateTime? Start { get; set; }
        [BsonElement("to")]
        [BsonRequired]
        [JsonIgnore]
        public DateTime? End { get; set; }
        [BsonElement("created")]
        [BsonRequired]
        [JsonIgnore]
        public DateTime? Created { get; set; }
        [BsonIgnore]
        public string Date { get; set; }

        public void StringifyDate()
        {
            Date = string.Empty;
            Date += Start.HasValue ? $"{Start.GetValueOrDefault().ToShortDateString()} : {Start.GetValueOrDefault().ToShortTimeString()}" : string.Empty;
            Date += " - ";
            Date += End.HasValue ? $"{End.GetValueOrDefault().ToShortDateString()} : {End.GetValueOrDefault().ToShortTimeString()}" : string.Empty;
        }
    }
}

/*
 * 
 */