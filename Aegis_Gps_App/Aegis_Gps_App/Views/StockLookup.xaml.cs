using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aegis_Gps_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StockLookup : ContentPage
    {
        private static string deviceId;
        private static int userId;
        List<StockSnapModel> retValue = new List<StockSnapModel>();

        public StockLookup()
        {
            InitializeComponent();
            deviceId = ((Label)Application.Current.FindByName("lblDeviceId")).Text;
            userId = Convert.ToInt32(((Label)Application.Current.FindByName("lblUserId")).Text);

            MyListView.ItemsSource = GetStockLookups().Result;
        }

        private async Task<List<StockSnapModel>> GetStockLookups()
        {
            try
            {
                if (App.CheckInternetConnection())
                {
                    HttpClient client = new HttpClient() { BaseAddress = new Uri(App.baseUrl) };
                    StockSnapModel model = new StockSnapModel()
                    {
                        UserId = userId,
                        DeviceId = deviceId,
                        ItemName = string.Empty,
                    };

                    string jsonData = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.getStockSnapsUri, content).ConfigureAwait(false);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        retValue = JsonConvert.DeserializeObject<List<StockSnapModel>>(result);
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

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            await Navigation.PushAsync(new StockDetail(((StockSnapModel)e.Item).ItemId, ((StockSnapModel)e.Item).ItemType, ((StockSnapModel)e.Item).AssetLocationId));
            //MainLayout masterDetailPage = new MainLayout();
            //masterDetailPage.Detail = new NavigationPage(new StockDetail(((StockSnapModel)e.Item).ItemId, ((StockSnapModel)e.Item).ItemType, ((StockSnapModel)e.Item).AssetLocationId));
            //Application.Current.MainPage = masterDetailPage;
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string keyword = searchBarStock.Text;
            MyListView.ItemsSource = retValue.Where(p => p.ItemName.ToUpper().Contains(keyword.ToUpper()) || p.Location.ToUpper().Contains(keyword.ToUpper()));
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
