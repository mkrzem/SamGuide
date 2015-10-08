using Newtonsoft.Json;
using SamGuide.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace SamGuide.Helpers
{
    public static class LocationFinder
    {
        private static string locationApi = @"http://nominatim.openstreetmap.org/search?";
        public async static Task<BasicGeoposition> GetLocationCoordinates(string location)
        {            
            string query = "q=" + location + "&format=json";
            string response;

            WebRequest webRequest = WebRequest.Create(locationApi + query);
            webRequest.Method = "GET";
            webRequest.ContentType = "application/x-www-form-urlencoded";

            using (WebResponse webResponse = await webRequest.GetResponseAsync())
            using (StreamReader dataReader = new StreamReader(webResponse.GetResponseStream()))
            {
                response = await dataReader.ReadToEndAsync();
            }

            List<OpenLocation> foundLocations = JsonConvert.DeserializeObject(response) as List<OpenLocation>;
            return new BasicGeoposition() { Latitude = Convert.ToDouble(foundLocations?[0].lat), Longitude = Convert.ToDouble(foundLocations?[0].lon) };            
        }
    }
}
