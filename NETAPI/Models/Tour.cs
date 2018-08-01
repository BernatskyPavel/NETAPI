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

        public const string IDFIELD = "_id";
        public const string CAPACITYFIELD = "allSeats";
        public const string AVAILABLEFIELD = "freeSeats";
        public const string ARRIVALFIELD = "from";
        public const string DEPARTUREFIELD = "to";
        public const string STATUSFIELD = "status";
        public const string NAMEFIELD = "name";
        public const string ABOUTFIELD = "description";
        public const string PHOTOFIELD = "photo";
        public const string PRICEFIELD = "price";
        public const string TIMESTAMPFIELD = "created";

        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement(elementName: IDFIELD)]
        public BsonObjectId Id { get; set; }
        [BsonElement(elementName: NAMEFIELD)]
        [BsonRequired]
        public string Name { get; set; }
        [BsonElement(elementName: ABOUTFIELD)]
        public string Description { get; set; }
        [BsonElement(elementName: CAPACITYFIELD)]
        [BsonRequired]
        [JsonIgnore]
        public int All { get; set; }
        [BsonElement(elementName: STATUSFIELD)]
        [BsonRequired]
        public string Status { get; set; }
        [BsonElement(elementName: PHOTOFIELD)]
        public string Photo { get; set; }
        [BsonElement(elementName: AVAILABLEFIELD)]
        [BsonRequired]
        public int Free { get; set; }
        [BsonElement(elementName: PRICEFIELD)]
        [BsonRequired]
        public int Price { get; set; }
        [BsonElement(elementName: ARRIVALFIELD)]
        [BsonRequired]
        [JsonIgnore]
        public DateTime? Start { get; set; }
        [BsonElement(elementName: DEPARTUREFIELD)]
        [BsonRequired]
        [JsonIgnore]
        public DateTime? End { get; set; }
        [BsonElement(elementName: TIMESTAMPFIELD)]
        [BsonRequired]
        [JsonIgnore]
        public DateTime? Created { get; set; }
        [BsonIgnore]
        public string Date { get; set; }
        [BsonIgnore]
        public string Guide { get; set; }

        public void StringifyDate()
        {
            Guide = string.Empty;
            Date = string.Empty;
            Date += Start.HasValue ? $"{Start.GetValueOrDefault().ToShortDateString()} : {Start.GetValueOrDefault().ToShortTimeString()}" : string.Empty;
            Date += " - ";
            Date += End.HasValue ? $"{End.GetValueOrDefault().ToShortDateString()} : {End.GetValueOrDefault().ToShortTimeString()}" : string.Empty;
        }
        [JsonIgnore]
        public bool IsFreeSeats => Free > Decimal.Zero;
    }
}

/*
 * 
 */