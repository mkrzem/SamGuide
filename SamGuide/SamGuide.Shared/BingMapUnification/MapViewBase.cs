using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace SamGuide.BingMapUnification
{
    public class MapViewBase : Grid, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}