using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Xamarin.Essentials;
using Plugin.Geolocator.Abstractions;

namespace Aegis_Gps_App.Droid
{
    [Activity(Label = "Aegis", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static string deviceId = Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
         private Position _position;
        protected override void OnCreate(Bundle savedInstanceState)
        {            
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            if (CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
            {
                RequestPermissions(new string[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation }, 0);
            }
            if (CheckSelfPermission(Manifest.Permission.Internet) != (int)Permission.Granted)
            {
                RequestPermissions(new string[] { Manifest.Permission.Internet }, 0);
            }
            GetPosition();
        }


        public async void GetPosition()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(10));
            var location = await Geolocation.GetLocationAsync(request);
            if (location != null && !location.IsFromMockProvider)
            {
                _position = new Position(location.Latitude, location.Longitude);
                LoadApplication(new App(deviceId, _position));
            }
            else
            {
                throw new Exception("Mocked GPS location detected.");
            }
        }
    }
}