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
    public partial class ClaimList : ContentPage
    {
        private static string deviceId;
        private static int userId;

        public ClaimList()
        {
            InitializeComponent();
            deviceId = ((Label)Application.Current.FindByName("lblDeviceId")).Text;
            userId = Convert.ToInt32(((Label)Application.Current.FindByName("lblUserId")).Text);

            MyListView.ItemsSource = GetClaimLists().Result;
        }

        private async Task<List<ClaimModel>> GetClaimLists()
        {
            List<ClaimModel> retValue = new List<ClaimModel>();
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
                    HttpResponseMessage response = await client.PostAsync(App.claimListUri, content).ConfigureAwait(false);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        retValue = JsonConvert.DeserializeObject<List<ClaimModel>>(result);
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
    }
}
