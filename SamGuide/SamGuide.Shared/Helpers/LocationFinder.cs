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
        public async static Task<string> GetLocationCoordinates(string location)
        {            
            byte[] query = Encoding.UTF8.GetBytes("q=" + location + "&format=json");            

            WebRequest webRequest = WebRequest.Create(locationApi);
            webRequest.Method = "GET";
            webRequest.ContentType = "application/x-www-form-urlencoded";

            using (Stream queryStream = await webRequest.GetRequestStreamAsync())
            {
                queryStream.Write(query, 0, query.Length);
            }

            using (WebResponse webResponse = await webRequest.GetResponseAsync())
            using (StreamReader dataReader = new StreamReader(webResponse.GetResponseStream()))
            {
                return await dataReader.ReadToEndAsync();
            }
        }
    }
}
