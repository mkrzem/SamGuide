using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SamGuide.Helpers;
using System.Windows.Input;
using Windows.Devices.Geolocation;

namespace SamGuide.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        private BasicGeoposition location = new BasicGeoposition();
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainPageViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        public BasicGeoposition Location
        {
            get { return location; }
            set
            {
                location = value;
                RaisePropertyChanged(nameof(Location));
            }
        }

        public ICommand GoToLocation
        {
            get
            {
                return new RelayCommand<object>(async (param) =>
                {
                    Location = await LocationFinder.GetLocationCoordinates(param.ToString());                    
                });
            }
        }
    }
}