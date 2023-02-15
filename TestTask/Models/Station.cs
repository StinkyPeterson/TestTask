using Microsoft.AspNetCore.Routing.Tree;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    [BsonIgnoreExtraElements]
    public class Station
    {
        public string? OidOrganization { get; set; } = "";
        public int? PromedId { get; set; } = null;
        [ValidatorStationAdisId]
        public int? StationAdisId { get; set; } = null;
        public string? StationName { get; set; } = "";

        public bool Equals(Station other)
        {
            if(other == null)
                return false;
            if (OidOrganization == other.OidOrganization && PromedId == other.PromedId && StationAdisId == other.StationAdisId && StationName == other.StationName)
                return true;
            else
                return false;
        }
    }
}
