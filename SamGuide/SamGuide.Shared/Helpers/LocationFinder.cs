using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SamGuide.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;

namespace SamGuide.Helpers
{
    public static class LocationFinder
    {
        private static string locationApi = @"http://nominatim.openstreetmap.org/search?";
        public async static Task<BasicGeoposition> GetLocationCoordinates(string location)
        {            
            string query = string.Format("q={0}&format=json", location);
            string responseJson;
            BasicGeoposition geoposition = new BasicGeoposition();

            try
            {
                WebRequest webRequest = WebRequest.Create(locationApi + query);
                webRequest.Method = "GET";
                webRequest.ContentType = "application/x-www-form-urlencoded";

                using (WebResponse webResponse = await webRequest.GetResponseAsync())
                using (StreamReader dataReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    responseJson = await dataReader.ReadToEndAsync();
                }

                JObject searchResult = JObject.Parse(string.Format("{{'searchResult':{0}}}", responseJson));
                IList<JToken> locationsJson = searchResult["searchResult"].Children().ToList();
                OpenLocation foundLocation = JsonConvert.DeserializeObject<OpenLocation>(locationsJson?[0].ToString());
                geoposition = new BasicGeoposition() { Latitude = Convert.ToDouble(foundLocation?.Lat), Longitude = Convert.ToDouble(foundLocation?.Lon) };
            }
            catch (WebException webEx)
            {
                var dialog = new MessageDialog(webEx.Message);
                await dialog.ShowAsync();
            }
            catch (JsonException jsonEx)
            {
                var dialog = new MessageDialog(jsonEx.Message);
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }

            return geoposition;
        }
    }
}
