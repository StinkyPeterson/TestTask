using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestTask.Models;
using System.Web;
using ExcelDataReader;
using System;
using MongoDB.Driver;
using MongoDB.Bson;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult AddSubstation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ListOfSubstations()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ListOfSubstations(IFormFile file, [FromServices] Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            DbContext dbContext = new DbContext();
            if (file == null)
                return ListOfSubstations();
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            using(FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            var substations = GetSubstationList(file.FileName);
            IMongoCollection<Substation> substationCollect = dbContext.SubstationCollect;
            if(substations.Count > 0)
                substationCollect.InsertMany(substations);

            return ListOfSubstations();
        }

        private List<Substation> GetSubstationList(string fileName)
        {
            DbContext dbContext = new DbContext();
            List<Substation> substations = new List<Substation>();
            var fName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(fName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                    while (reader.Read())
                    {
                        Substation substation = new Substation();
                        Station station = new Station();
                        station.OidOrganization = reader.IsDBNull(0) == true ? null : reader.GetValue(0).ToString();
                        station.PromedId = reader.IsDBNull(1) == true ? null : int.Parse(reader.GetValue(1).ToString());
                        station.StationAdisId = reader.IsDBNull(2) == true ? null : int.Parse(reader.GetValue(2).ToString());
                        station.StationName = reader.IsDBNull(3) == true ? null : reader.GetValue(3).ToString();
                        substation.SubstationPromedId = reader.IsDBNull(4) == true ? null : int.Parse(reader.GetValue(4).ToString());
                        substation.SubstationId = reader.IsDBNull(5) == true ? null : int.Parse(reader.GetValue(5).ToString());
                        substation.SubstationName = reader.IsDBNull(6) == true ? null : reader.GetValue(6).ToString();
                        substation.Station = station;    
                        if (dbContext.SubstationCollect.Find(new BsonDocument("_id",substation.SubstationPromedId)).Count() == 0)
                            substations.Add(substation);

                    }
                }
            }
            return substations;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}