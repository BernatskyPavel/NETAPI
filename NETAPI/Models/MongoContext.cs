using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Configuration;


namespace NETAPI.Models
{
    public class MongoContext
    {
        IMongoDatabase database;
        IGridFSBucket gridFS;

        public MongoContext()
        {
            string connectionString = "mongodb://23.97.131.8:27017/TourAgencyDB";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            database = client.GetDatabase(connection.DatabaseName);
            gridFS = new GridFSBucket(database);
        }

        ~MongoContext()
        {

        }

        public IMongoCollection<Tour> Tours
        {
            get { return database.GetCollection<Tour>("Tours"); }
        }

        public IMongoCollection<User> Users
        {
            get { return database.GetCollection<User>("Users"); }
        }
        public IMongoCollection<TUsers> TUsers
        {
            get { return database.GetCollection<TUsers>("Tour_Users"); }
        }
        public IMongoCollection<TCities> TCities
        {
            get { return database.GetCollection<TCities>("Tour_Cities"); }
        }
        public IMongoCollection<TPlaces> TPlaces
        {
            get { return database.GetCollection<TPlaces>("Tour_Places"); }
        }
        public IMongoCollection<Place> Places
        {
            get { return database.GetCollection<Place>("Places"); }
        }
        public IMongoCollection<City> Cities
        {
            get { return database.GetCollection<City>("Cities"); }
        }
        public IMongoCollection<Guide> Guides
        {
            get { return database.GetCollection<Guide>("Guides"); }
        }
        public IMongoCollection<TGuides> TGuides
        {
            get { return database.GetCollection<TGuides>("Tour_Guides"); }
        }
        public class TourList
        {
            public IEnumerable<Tour> Tours { get; set; }
        }
    }
}
