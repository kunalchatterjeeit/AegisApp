using Aegis_Gps_App.Views;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aegis_Gps_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainLayoutMaster : ContentPage
    {
        public ListView ListView;
        private static string deviceId;
        private static int userId;

        public MainLayoutMaster()
        {
            try
            {
                InitializeComponent();
                deviceId = ((Label)Application.Current.FindByName("lblDeviceId")).Text;
                userId = Convert.ToInt32(((Label)Application.Current.FindByName("lblUserId")).Text);

                //BindingContext = new MainLayoutMasterViewModel();
                ListView = MenuItemsListView;

                List<MainLayoutMasterMenuItem> menuList = new List<MainLayoutMasterMenuItem>
            {
                new MainLayoutMasterMenuItem {Name = "Home",Image = "home.png", TargetType = typeof(MainLayout)},
                new MainLayoutMasterMenuItem {Name = "Holiday list",Image = "holiday.png", TargetType = typeof(HolidayList)},
                new MainLayoutMasterMenuItem {Name = "My upcoming leaves",Image = "leave.png", TargetType = typeof(UpcomingLeaveList)},
                new MainLayoutMasterMenuItem {Name = "My claims",Image = "claim.png", TargetType = typeof(ClaimList)},
                new MainLayoutMasterMenuItem {Name = "My loyalty points",Image = "loyalty.png", TargetType = typeof(LoyaltyPointList)},
                new MainLayoutMasterMenuItem {Name = "My attendances",Image = "attendance.png", TargetType = typeof(AttendanceList)},
                new MainLayoutMasterMenuItem {Name = "Docket requests",Image = "docket.png", TargetType = typeof(DocketList)},
                new MainLayoutMasterMenuItem {Name = "Toner requests",Image = "toner.png", TargetType = typeof(TonerList)},
            };

                if (IsAuthorized(UtilityCode.STOCK_LOOKUP).Result)
                {
                    menuList.Add(new MainLayoutMasterMenuItem { Name = "Stock lookup", Image = "stocklookup.png", TargetType = typeof(StockLookup) });
                }

                MenuItemsListView.ItemsSource = menuList;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Exit");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                System.Environment.Exit(0);
            }
        }

        private async Task<bool> IsAuthorized(string utilityCode)
        {
            AuthorizationModel retValue = new AuthorizationModel();
            try
            {
                if (App.CheckInternetConnection())
                {
                    HttpClient client = new HttpClient() { BaseAddress = new Uri(App.baseUrl) };
                    AuthorizationModel model = new AuthorizationModel()
                    {
                        UserId = userId,
                        DeviceId = deviceId,
                        UtilityCode = utilityCode,
                    };

                    string jsonData = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(App.getIsAuthorizedUri, content).ConfigureAwait(false);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        retValue = JsonConvert.DeserializeObject<AuthorizationModel>(result);
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
            return retValue.ReturnValue;
        }

        class MainLayoutMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainLayoutMasterMenuItem> MenuItems { get; set; }

            public MainLayoutMasterViewModel()
            {

            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}