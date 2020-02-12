using Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Aegis_Gps_App
{
    public partial class MainPage : ContentPage
    {
        Image splashImage;
        ActivityIndicator activityIndicator;
        private static string deviceId;
        public MainPage()
        {
            try
            {
                InitializeComponent();

                deviceId = ((Label)Application.Current.FindByName("lblDeviceId")).Text;
                NavigationPage.SetHasNavigationBar(this, false);

                AbsoluteLayout sub = new AbsoluteLayout();
                splashImage = new Image
                {
                    Source = "drawable/icon.png",
                    HeightRequest = 100,
                };
                AbsoluteLayout.SetLayoutFlags(splashImage, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(splashImage, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                sub.Children.Add(splashImage);

                this.BackgroundColor = Color.FromHex("#3498DB");
                this.Content = sub;

                activityIndicator = new ActivityIndicator()
                {
                    IsVisible = true,
                    IsEnabled = true,
                    IsRunning = true,
                    Color = Color.FromHex("#BF0001"),
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Exit");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                System.Environment.Exit(0);
            }
        }

        protected async override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                await splashImage.ScaleTo(1, 2000);
                await splashImage.ScaleTo(0.9, 1500, Easing.Linear);
                await splashImage.ScaleTo(180, 1200, Easing.Linear);
                this.BackgroundColor = Color.FromHex("#0F243E");
                this.Content = activityIndicator;
                if (App.CheckInternetConnection())
                {
                    LoginModel model = await DoAutoLogin();
                    if (model.ResponseCode == (int)HttpStatusCode.OK && model.Message.ToLower().Equals("success"))
                    {
                        ((Label)Application.Current.FindByName("lblUserId")).Text = model.UserId.ToString();
                        Application.Current.MainPage = new NavigationPage(new MainLayout());
                    }
                    else
                    {
                        await DisplayAlert("Message", model.Message, "Ok");
                        Application.Current.MainPage = new NavigationPage(new LoginForm());
                    }
                }
                else
                {
                    await DisplayAlert("Alert", "No internet connection", "Exit");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    System.Environment.Exit(0);
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
                activityIndicator.IsVisible = false;
                activityIndicator.IsEnabled = false;
                activityIndicator.IsRunning = false;
            }
        }

        private async Task<LoginModel> DoAutoLogin()
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(App.baseUrl)
            };

            LoginModel model = new LoginModel()
            {
                DeviceId = deviceId
            };

            string jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(App.doAutoLoginUri, content).ConfigureAwait(false);

            var result = await response.Content.ReadAsStringAsync();
            model = JsonConvert.DeserializeObject<LoginModel>(result);
            return model;
        }
    }
}
