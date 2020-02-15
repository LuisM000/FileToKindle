using System;
using System.Threading.Tasks;
using FileToKindle.Shared.Services;
using Xamarin.Forms;

namespace FileToKindle.Shared.ViewModels
{
    public class ViewModelBase : MvvmHelpers.BaseViewModel
    {
        protected readonly INavigationService NavigationService;

        public ViewModelBase()
        {
            NavigationService = DependencyService.Get<INavigationService>();
        }

        public virtual void Initialize(params object[] navigationData)
        {

        }
    }
}