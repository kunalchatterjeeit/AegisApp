using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aegis_Gps_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainLayout : MasterDetailPage
    {
        public MainLayout()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Exit");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                System.Environment.Exit(0);
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainLayoutMasterMenuItem;
            if (item == null)
                return;
            Type page = item.TargetType;
           
            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }
    }
}