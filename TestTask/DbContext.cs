using MongoDB.Bson;
using MongoDB.Driver;
using TestTask.Models;

namespace TestTask
{
    public class DbContext
    {
        private IMongoDatabase mongoDatabase;
        private IMongoCollection<Substation> substationCollection;


        public DbContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            mongoDatabase = client.GetDatabase("TestTask");
            substationCollection = mongoDatabase.GetCollection<Substation>("substations");
        }

        public IMongoCollection<Substation> SubstationCollect
        {
            get
            {
                return mongoDatabase.GetCollection<Substation>("substations");
            }
        }

        public Substation GetSubstation(int? id)
        {
            return substationCollection.Find(x => x.SubstationPromedId == id).FirstOrDefault();
        }
        public void Delete(int? id)
        {
            substationCollection.DeleteOne(x => x.SubstationPromedId == id);
        }
        public List<Substation> GetSubstations()
        {
            return substationCollection.Find(new BsonDocument()).ToList();
        }
        public void SaveOrUpdate(Substation substation)
        {
            Substation substationDb = substationCollection.Find(x => x.SubstationPromedId == substation.SubstationPromedId).FirstOrDefault();
            Station stationDb = substationDb.Station;
            Station station = substation.Station;
            
            if (!station.Equals(stationDb))
            {
                List<Substation> substations = GetSubstations().Where(x => x.Station.PromedId == station.PromedId).ToList();
                substations.ForEach(x => x.Station = station);
                foreach(Substation sub in substations)
                    substationCollection.ReplaceOne(x => x.SubstationPromedId == sub.SubstationPromedId, sub);

            }
            if (substationDb == null)
            {
                substationCollection.InsertOne(substation);
            }
            else
            {
                substationCollection.ReplaceOne(x => x.SubstationPromedId == substationDb.SubstationPromedId, substation);
            }
        }

        public bool CheckError(Substation substation)
        {
            var substationDb = substationCollection.Find(x => x.SubstationId == substation.SubstationId).FirstOrDefault();
            if(substationDb != null)
            {
                return true;
            }
            return false;
        }

    }
}
