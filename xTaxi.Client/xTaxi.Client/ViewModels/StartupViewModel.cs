using System;
using System.ComponentModel;
using Xamarin.Forms;
using xTaxi.Client.Pages;

namespace xTaxi.Client.ViewModels
{
    public class StartupViewModel : INotifyPropertyChanged
    {
        public StartupViewModel()
        {
        }


        public async void CheckLogin()
        {
            try
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            catch (Exception e)
            {
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
