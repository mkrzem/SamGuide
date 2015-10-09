using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Geolocation;
using System.Collections.Generic;
using Windows.UI;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using System;

#if WINDOWS_APP
using Bing.Maps;
#elif WINDOWS_PHONE_APP
    using Windows.UI.Xaml.Controls.Maps;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Shapes;
#endif

namespace SamGuide.BingMapUnification
{
    public class MapView : MapViewBase
    {

        //private string location;
#if WINDOWS_APP
        private Map map;
        private MapLayer pinLayer;
        private MapShapeLayer shapeLayer;
#elif WINDOWS_PHONE_APP
        private MapControl map;
#endif
        #region Ctor

        public MapView()
        {
#if WINDOWS_APP
            map = new Map();

            shapeLayer = new MapShapeLayer();
            pinLayer = new MapLayer();

            map.ShapeLayers.Add(shapeLayer);
            map.Children.Add(pinLayer);
#elif WINDOWS_PHONE_APP
            map = new MapControl();            
#endif

            this.Children.Add(map);
        }

        #endregion

        #region Dependency Properties        
        public bool ShowTraffic
        {
            get { return (bool)GetValue(ShowTrafficProperty); }
            set { SetValue(ShowTrafficProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowTraffic.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowTrafficProperty =
            DependencyProperty.Register("ShowTraffic", typeof(bool), typeof(MapView), new PropertyMetadata(null, ShowTrafficChanged));

        private static void ShowTrafficChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapView view = d as MapView;
#if WINDOWS_APP
            if (view.map.ShowTraffic != (bool)e.NewValue)
            {
                view.map.ShowTraffic = (bool)e.NewValue;
            }
#elif WINDOWS_PHONE_APP            
            if (view.map.TrafficFlowVisible != (bool)e.NewValue)
	        {
		        view.map.TrafficFlowVisible = (bool)e.NewValue; 
	        }
#endif
        }

        public BasicGeoposition Location
        {
            get { return (BasicGeoposition)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LocationProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(BasicGeoposition), typeof(MapView), new PropertyMetadata(null, LocationChanged));

        private static void LocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                MapView view = d as MapView;
                view.SetView((BasicGeoposition)e.NewValue, 10.0);
            }
        }
        #endregion

#region Properties
        //public string Location
        //{
        //    get { return location; }
        //    set
        //    {
        //        if (location != value)
        //        {
        //            location = value;
        //            RaisePropertyChanged(nameof(Location));
        //        }
        //    }
        //}
        public string Credentials
        {
            get
            {
#if WINDOWS_APP
                return map.Credentials;
#elif WINDOWS_PHONE_APP
                return string.Empty;
#endif
            }
            set
            {
#if WINDOWS_APP
                if (!string.IsNullOrEmpty(value))
                {
                    map.Credentials = value;
                }
#endif
                RaisePropertyChanged(nameof(Credentials));
            }
        }

        public string MapServiceToken
        {
            get
            {
#if WINDOWS_APP
                return string.Empty;
#elif WINDOWS_PHONE_APP
                return map.MapServiceToken;
#endif
            }
            set
            {
#if WINDOWS_PHONE_APP
                if (!string.IsNullOrEmpty(value))
                {
                    map.MapServiceToken = value;
                }
#endif
                RaisePropertyChanged(nameof(MapServiceToken));
            }
        }

        public double Zoom
        {
            get
            {
                return map.ZoomLevel;
            }
            set
            {
                map.ZoomLevel = value;
                RaisePropertyChanged(nameof(Zoom));
            }
        }

        public Geopoint Center
        {
            get
            {
#if WINDOWS_APP
                return map.Center.ToGeopoint();
#elif WINDOWS_PHONE_APP
                return map.Center;
#endif
            }
            set
            {
#if WINDOWS_APP
                map.Center = value.ToLocation();
#elif WINDOWS_PHONE_APP
                map.Center = value;
#endif

                RaisePropertyChanged(nameof(Center));
            }
        }

//        public bool ShowTraffic
//        {
//            get
//            {
//#if WINDOWS_APP
//                return map.ShowTraffic;
//#elif WINDOWS_PHONE_APP
//                return map.TrafficFlowVisible;
//#endif
//            }
//            set
//            {
//#if WINDOWS_APP
//                map.ShowTraffic = value;
//#elif WINDOWS_PHONE_APP
//                map.TrafficFlowVisible = value;
//#endif

//                RaisePropertyChanged(nameof(ShowTraffic));
//            }
//        }
        #endregion

        #region Helper Methods
        public void SetView(BasicGeoposition center, double zoom)
        {
#if WINDOWS_APP
            map.SetView(center.ToLocation(), zoom);
            RaisePropertyChanged(nameof(Center));
            RaisePropertyChanged(nameof(Zoom));
#elif WINDOWS_PHONE_APP
            map.Center = new Geopoint(center);
            map.ZoomLevel = zoom;
#endif
        }

        public void AddPushpin(BasicGeoposition location, string text)
        {
#if WINDOWS_APP
            var pin = new Pushpin()
            {
                Text = text
            };
            MapLayer.SetPosition(pin, location.ToLocation());
            pinLayer.Children.Add(pin);
#elif WINDOWS_PHONE_APP
            var pin = new Grid()
            {
                Width = 24,
                Height = 24,
                Margin = new Windows.UI.Xaml.Thickness(-12)
            };

            pin.Children.Add(new Ellipse()
            {
                Fill = new SolidColorBrush(Colors.DodgerBlue),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 3,
                Width = 24,
                Height = 24
            });

            pin.Children.Add(new TextBlock()
            {
                Text = text,
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            });

            MapControl.SetLocation(pin, new Geopoint(location));
            map.Children.Add(pin);
#endif
        }

        public void AddPolyline(List<BasicGeoposition> locations, Color strokeColor, double strokeThickness)
        {
#if WINDOWS_APP
            var line = new MapPolyline()
            {
                Locations = locations.ToLocationCollection(),
                Color = strokeColor,
                Width = strokeThickness
            };

            shapeLayer.Shapes.Add(line);
#elif WINDOWS_PHONE_APP
            var line = new MapPolyline()
            {
                Path = new Geopath(locations),
                StrokeColor = strokeColor,
                StrokeThickness = strokeThickness
            };

            map.MapElements.Add(line);
#endif
        }

        public void AddPolygon(List<BasicGeoposition> locations, Color fillColor, Color strokeColor, double strokeThickness)
        {
#if WINDOWS_APP
            var line = new MapPolygon()
            {
                Locations = locations.ToLocationCollection(),
                FillColor = fillColor
            };

            shapeLayer.Shapes.Add(line);
#elif WINDOWS_PHONE_APP
            var line = new MapPolygon()
            {
                Path = new Geopath(locations),
                FillColor = fillColor,
                StrokeColor = strokeColor,
                StrokeThickness = strokeThickness
            };

            map.MapElements.Add(line);
#endif
        }

        public void ClearMap()
        {
#if WINDOWS_APP
            shapeLayer.Shapes.Clear();
            pinLayer.Children.Clear();
#elif WINDOWS_PHONE_APP
            map.MapElements.Clear();
            map.Children.Clear();
#endif
        } 
#endregion
    }
}
