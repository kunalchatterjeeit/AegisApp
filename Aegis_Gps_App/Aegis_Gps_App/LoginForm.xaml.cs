using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aegis_Gps_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginForm : ContentPage
    {
        private static string deviceId;
        public LoginForm()
        {
            try
            {
                InitializeComponent();
                deviceId = ((Label)Application.Current.FindByName("lblDeviceId")).Text;
                NavigationPage.SetHasNavigationBar(this, false);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Exit");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                System.Environment.Exit(0);
            }
        }

        private async Task<LoginModel> DoLogin()
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(App.baseUrl)
            };

            LoginModel model = new LoginModel()
            {
                UserName = txtUserName.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                DeviceId = deviceId
            };

            string jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(App.doLoginUri, content).ConfigureAwait(false);

            var result = await response.Content.ReadAsStringAsync();
            model = JsonConvert.DeserializeObject<LoginModel>(result);
            return model;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (App.CheckInternetConnection())
                {
                    LoginModel model = await DoLogin();
                    if (model.ResponseCode == (int)HttpStatusCode.OK && model.Message.ToLower().Equals("success"))
                    {
                        ((Label)Application.Current.FindByName("lblUserId")).Text = model.UserId.ToString();
                        Application.Current.MainPage = new NavigationPage(new MainLayout());
                    }
                    else if (model.ResponseCode == (int)HttpStatusCode.OK && model.Message.ToLower().Contains("login succeeded"))
                    {
                        ((Label)Application.Current.FindByName("lblUserId")).Text = model.UserId.ToString();
                        await DisplayAlert("Message", model.Message, "Ok");
                        Application.Current.MainPage = new NavigationPage(new MainLayout());
                    }
                    else
                    {
                        await DisplayAlert("Message", model.Message, "Ok");
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
                
            }
        }
    }
}