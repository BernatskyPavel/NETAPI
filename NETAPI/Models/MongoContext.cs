using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Configuration;
using NETAPI.Properties;

namespace NETAPI.Models
{
    public class MongoContext
    {
        private IMongoDatabase database;
        private IGridFSBucket gridFS;

        public MongoContext()
        {
            string connectionString = Resources.ConnectionString;
            MongoUrlBuilder connection = new MongoUrlBuilder(url: connectionString);
            MongoClient client = new MongoClient(connectionString: connectionString);
            database = client.GetDatabase(name: connection.DatabaseName);
            gridFS = new GridFSBucket(database: database);
        }

        public IMongoCollection<Tour> Tours => database.GetCollection<Tour>(name: Status.TOURSCOLLECTION);
        public IMongoCollection<User> Users => database.GetCollection<User>(name: Status.USERSCOLLECTION);
        public IMongoCollection<TUsers> TUsers => database.GetCollection<TUsers>(name: Status.TOURUSERSCOLLECTION);
        public IMongoCollection<TCities> TCities => database.GetCollection<TCities>(name: Status.TOURCITIESCOLLECTION);
        public IMongoCollection<TPlaces> TPlaces => database.GetCollection<TPlaces>(name: Status.TOURPLACESCOLLECTION);
        public IMongoCollection<Place> Places => database.GetCollection<Place>(name: Status.PLACESCOLLECTION);
        public IMongoCollection<City> Cities => database.GetCollection<City>(name: Status.CITIESCOLLECTION);
        public IMongoCollection<Guide> Guides => database.GetCollection<Guide>(name: Status.GUIDESCOLLECTION);
        public IMongoCollection<TGuides> TGuides => database.GetCollection<TGuides>(name: Status.TOURGUIDESCOLLECTION);
    }
}
