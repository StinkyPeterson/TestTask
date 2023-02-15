using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Tree;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using static TestTask.Models.ValidatorStationAdisId;

namespace TestTask.Models
{
    [BsonIgnoreExtraElements]
    public class Substation
    {
        [BsonId]
        public int? SubstationPromedId { get; set; } = null;
        //[ValidatorSubstationId]
        public int? SubstationId { get; set; } = null;
        public string? SubstationName { get; set; } = "";
        public Station Station { get; set; } = new Station();

    }
}
