using System;
using System.Threading.Tasks;
using FileToKindle.Shared.ViewModels;
using Xamarin.Forms;

namespace FileToKindle.Shared.Services
{
    public interface INavigationService 
    {
        Task NavigateBackAsync();

        public Task NavigateToAsync<TViewModel>(params object[] parameters) where TViewModel : ViewModelBase;
    }
}
