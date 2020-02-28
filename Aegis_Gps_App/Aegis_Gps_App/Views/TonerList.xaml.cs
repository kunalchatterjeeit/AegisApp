using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aegis_Gps_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TonerList : ContentPage
    {
        private static string deviceId;
        private static int userId;

        public TonerList()
        {
            InitializeComponent();
            deviceId = ((Label)Application.Current.FindByName("lblDeviceId")).Text;
            userId = Convert.ToInt32(((Label)Application.Current.FindByName("lblUserId")).Text);

            MyListView.ItemsSource = GetTonerLists().Result;
        }

        private async Task<List<TonerModel>> GetTonerLists()
        {
            List<TonerModel> retValue = new List<TonerModel>();
            try
            {
                if (App.CheckInternetConnection())
                {
                    HttpClient client = new HttpClient() { BaseAddress = new Uri(App.baseUrl) };
                    BaseModel model = new BaseModel()
                    {
                        UserId = userId,
                        DeviceId = deviceId
                    };

                    string jsonData = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.getTonerUri, content).ConfigureAwait(false);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        retValue = JsonConvert.DeserializeObject<List<TonerModel>>(result);
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
            return retValue;
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            MainLayout masterDetailPage = new MainLayout();
            masterDetailPage.Detail = new NavigationPage(new MainLayoutDetail());
            Application.Current.MainPage = masterDetailPage;
            return true;
        }
    }
}
