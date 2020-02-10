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
    public sealed partial class AddCareTakerPage : Page
    {
        private const string BASE_URL = @"http://localhost:51622/";
        public AddCareTakerPage()
        {
            this.InitializeComponent();
            LoadCareGivers();
        }

        private async void LoadCareGivers()
        {
            try
            {
                string URL = BASE_URL + "CareGivers";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var giverList = JsonConvert.DeserializeObject<List<CareGiver>>(content);

                    cmbGiver.ItemsSource = giverList;
                    cmbGiver.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public async System.Threading.Tasks.Task AddTaker()
        {
            try
            {
                HttpClient client = new HttpClient();
                //Fetches Care Giver to later use as data for Care Taker
                CareGiver selectedGiver = (CareGiver)cmbGiver.SelectedItem;
                CareTaker newCareTaker = new CareTaker();
                //Sets input as value for new Care Taker
                newCareTaker.firstName = txtFirstName.Text;
                newCareTaker.lastName = txtLastName.Text;
                newCareTaker.careGiverId = selectedGiver.Id;

                string jsonString = JsonConvert.SerializeObject(newCareTaker);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                string URL = BASE_URL + "CareTakers";
                var response = await client.PostAsync(URL, content);
                var responseString = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(responseString);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        //Save button for adding Care Taker
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            await AddTaker();
            var dialog = new MessageDialog("New care recipient has been registered");
        }
        //Menu field
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
