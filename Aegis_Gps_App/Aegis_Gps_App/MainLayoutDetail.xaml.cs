using Model;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aegis_Gps_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainLayoutDetail : ContentPage
    {
        private static string deviceId;
        private static int userId;
        private static string attendanceMode;
        private static string latitude;
        private static string longitude;

        public MainLayoutDetail()
        {
            try
            {
                InitializeComponent();
                deviceId = ((Label)Application.Current.FindByName("lblDeviceId")).Text;
                userId = Convert.ToInt32(((Label)Application.Current.FindByName("lblUserId")).Text);
                latitude = ((Label)Application.Current.FindByName("lblLatitude")).Text;
                longitude = ((Label)Application.Current.FindByName("lblLongitude")).Text;
                GetAccountDetails();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Exit");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                System.Environment.Exit(0);
            }
        }

        private async void AttendanceButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                AttendanceModel model = MakeAttendance().Result;
                if (model.ResponseCode == (int)HttpStatusCode.OK)
                {
                    await DisplayAlert("Message", model.Message, "Ok");
                    Application.Current.MainPage = new NavigationPage(new MainLayout());
                }
                else if (model.ResponseCode == (int)HttpStatusCode.BadRequest)
                {
                   throw new Exception(model.Message);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Exit");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                System.Environment.Exit(0);
            }
            finally
            {

            }
        }

        private void GetAccountDetails()
        {
            try
            {
                if (App.CheckInternetConnection())
                {
                    HttpClient client = new HttpClient() { BaseAddress = new Uri(App.baseUrl) };
                    AccountModel model = new AccountModel()
                    {
                        UserId = userId,
                        DeviceId = deviceId
                    };

                    string jsonData = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    Task<HttpResponseMessage> response = client.PostAsync(App.accountDetailsUri, content);//.ConfigureAwait(false);
                    response.Wait();

                    if (response.IsCompleted && response.Result != null && response.Result.IsSuccessStatusCode)
                    {
                        var result = response.Result.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(result.Result))
                        {
                            model = JsonConvert.DeserializeObject<AccountModel>(result.Result);
                            if (model != null)
                            {
                                imageProfile.Source = model.ImageProfile;
                                lblDesignation.Text = string.Format("Designation: {0}", model.Designation);
                                lblEmployeeName.Text = string.Format("Hello {0}", model.EmployeeName);
                                lblLastLogin.Text = string.Format("Last logged in: {0}", model.LastLogin);
                                lblLoyaltyPoint.Text = model.LoyaltyPoint.ToString();
                                lblReportsTo.Text = string.Format("Reporting person: {0}", model.ReportsTo);
                                attendanceMode = model.AttendanceState.ToUpper();

                                if (model.LoyaltyPoint >= 60)
                                {
                                    lblLoyaltyPoint.TextColor = Color.FromHex("#2ae02a");
                                }
                                else if (model.LoyaltyPoint >= 30)
                                {
                                    lblLoyaltyPoint.TextColor = Color.FromHex("#f27608");
                                }
                                else
                                {
                                    lblLoyaltyPoint.TextColor = Color.FromHex("#ff0000");
                                }

                                if (model.AttendanceState.ToUpper().Equals("OUT"))
                                {
                                    attendanceButton.Image = "drawable/start.png";
                                }
                                else if (model.AttendanceState.ToUpper().Equals("IN"))
                                {
                                    attendanceButton.Image = "drawable/stop.png";
                                }
                            }
                        }
                    }
                }
                else
                {
                    DisplayAlert("Alert", "No internet connection", "Exit");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Exit");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                System.Environment.Exit(0);
            }
            finally
            {

            }
        }

        private async Task<AttendanceModel> MakeAttendance()
        {
            AttendanceModel model = new AttendanceModel();
            if (!string.IsNullOrEmpty(latitude)&& !string.IsNullOrEmpty(longitude))
            {
                if (App.CheckInternetConnection())
                {
                    HttpClient client = new HttpClient() { BaseAddress = new Uri(App.baseUrl) };
                    model = new AttendanceModel()
                    {
                        UserId = userId,
                        DeviceId = deviceId,
                        Latitude = latitude,
                        Longitude = longitude,
                        AttendanceMode = attendanceMode
                    };

                    string jsonData = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.attendance_MakeAttendanceUri, content).ConfigureAwait(false);
                    var result = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<AttendanceModel>(result);
                }
                else
                {
                    model.ResponseCode = (int)HttpStatusCode.BadRequest;
                    model.Message = "No internet connection.";
                }
            }
            else
            {
                model.ResponseCode = (int)HttpStatusCode.BadRequest;
                model.Message = "GPS location not accessible. Please check setting.";
            }
            return model;
        }
    }
}