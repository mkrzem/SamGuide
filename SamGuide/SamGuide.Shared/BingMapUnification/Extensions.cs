using System.Collections.Generic;
using Windows.Devices.Geolocation;

#if WINDOWS_APP
    using Bing.Maps;
#endif

namespace SamGuide
{
    public static class Extensions
    {
#if WINDOWS_APP

        public static LocationCollection ToLocationCollection(this IList<BasicGeoposition> pointList)
        {
            var locations = new LocationCollection();

            foreach (var point in pointList)
            {
                locations.Add(point.ToLocation());
            }

            return locations;
        }

        public static Geopoint ToGeopoint(this Location location)
        {
            return new Geopoint(new BasicGeoposition() { Latitude = location.Latitude, Longitude = location.Longitude });
        }

        public static Location ToLocation(this Geopoint location)
        {
            return new Location(location.Position.Latitude, location.Position.Longitude);
        }

        public static Location ToLocation(this BasicGeoposition location)
        {
            return new Location(location.Latitude, location.Longitude);
        }

#elif WINDOWS_PHONE_APP

        //Add any required Windows Phone Extensions

#endif
    }
}
