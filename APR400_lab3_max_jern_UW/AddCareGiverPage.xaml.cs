using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using APR400_lab3_max_jern_UW.Model;
using System.Text;
using Windows.UI.Popups;

namespace APR400_lab3_max_jern_UW
{
    public sealed partial class AddCareGiverPage : Page
    {
        private const string BASE_URL = @"http://localhost:51622/";
        public AddCareGiverPage()
        {
            this.InitializeComponent();
            LoadCareCenter();
        }

        private async void LoadCareCenter()
        {
            try
            {
                string URL = BASE_URL + "CareCenters";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var centerList = JsonConvert.DeserializeObject<List<CareCenter>>(content);

                    cmbCenter.ItemsSource = centerList;
                    cmbCenter.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public async System.Threading.Tasks.Task AddGiver()
        {
            try
            {
                HttpClient client = new HttpClient();
                //Fetches Care Center to later use as data for Care Giver
                CareCenter selectedCenter = (CareCenter)cmbCenter.SelectedItem;
                CareGiver newCareGiver = new CareGiver();
                //Sets input as value for new Care Giver
                newCareGiver.firstName = txtFirstName.Text;
                newCareGiver.lastName = txtLastName.Text;
                newCareGiver.role = txtRole.Text;
                newCareGiver.careCenterId = selectedCenter.Id;

                string jsonString = JsonConvert.SerializeObject(newCareGiver);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                string URL = BASE_URL + "CareGivers";
                var response = await client.PostAsync(URL, content);
                var responseString = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(responseString);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        //Button for adding Care Giver
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            await AddGiver();
            var dialog = new MessageDialog("New care giver has been registered");
            await dialog.ShowAsync();
        }
        //Menu Field
        #region Navigation
        private void MnuCareGive_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CareGiverPage));
        }
        private void MnuCareTake_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CareTakerPage));
        }
        private void MnuAddGive_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddCareGiverPage));
        }
        private void MnuAddTake_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddCareTakerPage));
        }
        #endregion
    }
}
