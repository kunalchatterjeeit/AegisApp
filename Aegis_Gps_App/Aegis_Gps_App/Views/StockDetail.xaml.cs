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
    public partial class StockDetail : ContentPage
    {
        private static string deviceId;
        private static int userId;

        public StockDetail(string itemId, string itemType, string assetLocationId)
        {
            InitializeComponent();
            deviceId = ((Label)Application.Current.FindByName("lblDeviceId")).Text;
            userId = Convert.ToInt32(((Label)Application.Current.FindByName("lblUserId")).Text);

            MyListView.ItemsSource = GetStockDetails(itemId, itemType, assetLocationId).Result;
        }

        private async Task<List<StockDetailModel>> GetStockDetails(string itemId, string itemType, string assetLocationId)
        {
            List<StockDetailModel> retValue = new List<StockDetailModel>();
            try
            {
                if (App.CheckInternetConnection())
                {
                    HttpClient client = new HttpClient() { BaseAddress = new Uri(App.baseUrl) };
                    StockDetailModel model = new StockDetailModel()
                    {
                        UserId = userId,
                        DeviceId = deviceId,
                        ItemId = itemId,
                        ItemType = itemType,
                        AssetLocationId = assetLocationId
                    };

                    string jsonData = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.getStockDetailsUri, content).ConfigureAwait(false);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        retValue = JsonConvert.DeserializeObject<List<StockDetailModel>>(result);
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
