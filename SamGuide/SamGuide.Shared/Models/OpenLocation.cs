using System.Collections.Generic;

namespace SamGuide.Models
{
    public class OpenLocation
    {
        public string Place_id { get; set; }
        public string Display_name { get; set; }
        public List<string> Boundingbox { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
    }
}
//{"place_id":"127770031",
//"licence":"Data © OpenStreetMap contributors, ODbL 1.0. http:\/\/www.openstreetmap.org\/copyright",
//"osm_type":"relation",
//"osm_id":"336074",
//"boundingbox":["52.0978507","52.3681531","20.8516882","21.2711512"],
//"lat":"52.2319237",
//    "lon":"21.0067265",
//    "display_name":"Warszawa, województwo mazowieckie, Polska",
//    "class":"place",
//    "type":"city",
//    "importance":0.86290184643018,
//    "icon":"http:\/\/nominatim.openstreetmap.org\/images\/mapicons\/poi_place_city.p.20.png"
//    }