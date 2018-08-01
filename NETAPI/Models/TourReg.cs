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
    public class TourReg
    {
        [JsonProperty(PropertyName = "user_id")]
        public string UId { get; set; }
        [JsonProperty(PropertyName = "tour_id")]
        public string TId { get; set; }


    }
}

