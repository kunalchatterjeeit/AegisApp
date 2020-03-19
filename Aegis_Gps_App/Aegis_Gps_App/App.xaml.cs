using Plugin.Connectivity;
using Plugin.Geolocator.Abstractions;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Aegis_Gps_App
{
    public partial class App : Application
    {
        public const string baseUrl = "http://api.aegiscrm.in/api/";
        public const string attendance_CurrentMonthUri = "Attendance/Attendance_CurrentMonth";
        public const string attendance_MakeAttendanceUri = "Attendance/MakeAttendance";
        public const string doLoginUri = "Login/DoLogin";
        public const string doAutoLoginUri = "Login/DoAutoLogin";
        public const string doRegisterDeviceUri = "Login/RegisterDevice";
        public const string accountDetailsUri = "Account/GetAttendanceState";
        public const string holidayListUri = "Account/GetHolidayList";
        public const string upcomingLeaveListUri = "Account/GetUpcomingLeave";
        public const string claimListUri = "Account/GetPendingClaim";
        public const string getLoyalityPointListUri = "Account/GetLoyalityPoint";
        public const string getDocketUri = "Account/GetDocket";
        public const string getTonerUri = "Account/GetToner";
        public const string getStockSnapsUri = "Account/GetStockSnaps";
        public const string getStockDetailsUri = "Account/GetStockDetails";
        public const string getIsAuthorizedUri = "Account/IsAuthorized";

        private Position _position;

        public App(string deviceId, bool nevigate)
        {
            InitializeComponent();
            GetPosition();
            lblDeviceId.Text = deviceId;
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static bool CheckInternetConnection()
        {
            bool retValue = false;
            if (CrossConnectivity.IsSupported)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    retValue = true;
                }
            }
            else
            {
                retValue = true;
            }
            return retValue;
        }

        public async void GetPosition()
        {
            Location location = new Location();
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(10));
                location = await Geolocation.GetLocationAsync(request);
            }
            catch
            {
                location = await Geolocation.GetLastKnownLocationAsync();
            }
            if (location != null && !location.IsFromMockProvider)
            {
                _position = new Position(location.Latitude, location.Longitude);
                lblLatitude.Text = _position.Latitude.ToString();
                lblLongitude.Text = _position.Longitude.ToString();
            }
        }

    }
}
