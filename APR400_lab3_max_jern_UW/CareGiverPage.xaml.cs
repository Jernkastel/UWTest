using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Net.Http;
using Newtonsoft.Json;
using APR400_lab3_max_jern_UW.Model;

namespace APR400_lab3_max_jern_UW
{
    public sealed partial class CareGiverPage : Page
    {
        private const string BASE_URL = @"http://localhost:51622/";
        public CareGiverPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadCareGivers();
        }
        public async void LoadCareGivers()
        {
            try
            {
                //Progress bar
                prgCareGiver.IsActive = true;
                string URL = BASE_URL + "CareGivers";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));
                prgCareGiver.IsActive = true;

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var careGiverList = JsonConvert.DeserializeObject<List<CareGiver>>(content);
                    //Databinding list of Care Givers
                    careGiverListBox.ItemsSource = careGiverList;
                    prgCareGiver.IsActive = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
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
