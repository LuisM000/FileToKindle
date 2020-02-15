using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileToKindle.Shared.Models;
using FileToKindle.Shared.Services;
using FileToKindle.Shared.ViewModels;
using Xamarin.Forms;

namespace FileToKindle.Shared
{
    public partial class App : Application
    {
        readonly INavigationService navigationService = DependencyService.Get<INavigationService>();

        public App()
        {
            InitializeComponent();

            navigationService.NavigateToAsync<MainViewModel>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
