using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETAPI.Models
{
    public class TPlaces
    {
        public const string IDFIELD = "_id";
        public const string FIDFIELD = "f_id";
        public const string PIDFIELD = "place_id";
        public const string ARRIVALFIELD = "from";
        public const string DEPARTUREFIELD = "to";
        public const string STATUSFIELD = "status";

        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [BsonElement(elementName: IDFIELD)]
        public BsonObjectId Id { get; set; }
        [BsonElement(elementName: FIDFIELD)]
        [BsonRequired]
        public BsonObjectId FId { get; set; }
        [BsonElement(elementName: PIDFIELD)]
        [BsonRequired]
        public BsonObjectId PId { get; set; }
        [BsonElement(elementName: ARRIVALFIELD)]
        [JsonIgnore]
        public DateTime? Start { get; set; }
        [BsonElement(elementName: DEPARTUREFIELD)]
        [JsonIgnore]
        public DateTime? End { get; set; }
        [BsonElement(STATUSFIELD)]
        [BsonRequired]
        public string Status { get; set; }
        [BsonIgnore]
        public string Name { get; set; }
        [BsonIgnore]
        public string CId { get; set; }
        [BsonIgnore]
        public string Description { get; set; }
        [BsonIgnore]
        [JsonIgnore]
        public List<string> Photos { get; set; }
        [BsonIgnore]
        public string Coordinates { get; set; }

        [BsonIgnore]
        public string Date { get; set; }
        [BsonIgnore]
        public string Photo { get; set; }


        TPlaces()
        {
            Coordinates = Description = Name = string.Empty;
            Photos = new List<string>();
        }

        public void SetPhoto()
        {
            Photo = Photos.Count > UInt32.MinValue ? Photos.First() : string.Empty;
        }

        public void StringifyDate()
        {
            Date = string.Empty;
            Date += Start.HasValue ? $"{Start.GetValueOrDefault().ToShortDateString()} : {Start.GetValueOrDefault().ToShortTimeString()}" : string.Empty;
            Date += " - ";
            Date += End.HasValue ? $"{End.GetValueOrDefault().ToShortDateString()} : {End.GetValueOrDefault().ToShortTimeString()}" : string.Empty;
        }
    }
}
