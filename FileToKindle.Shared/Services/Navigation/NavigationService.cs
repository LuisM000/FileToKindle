using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileToKindle.Shared.Services;
using FileToKindle.Shared.ViewModels;
using FileToKindle.Shared.Views;
using Xamarin.Forms;

[assembly: Dependency(typeof(NavigationService))]
namespace FileToKindle.Shared.Services
{
    public class NavigationService : INavigationService
    {
        readonly Dictionary<Type, Type> mappings;

        public NavigationService()
        {
            mappings = new Dictionary<Type, Type>();
            CreatePageViewModelMappings();
        }


        public Task NavigateBackAsync() => Application.Current.MainPage.Navigation.PopModalAsync();
        

        public Task NavigateToAsync<TViewModel>(params object[] parameters) where TViewModel : ViewModelBase => InternalNavigateToAsync(typeof(TViewModel), parameters);


        private async Task InternalNavigateToAsync(Type viewModelType, params object[] parameters)
        {
            var page = CreateAndBindPage(viewModelType);
            (page.BindingContext as ViewModelBase).Initialize(parameters);

            if (page is MainPage)
            {
                Application.Current.MainPage = page;
            }
            else
            {
                if(Device.RuntimePlatform == Device.macOS)
                {
                    await Task.Delay(1);
                }

                await Application.Current.MainPage.Navigation.PushModalAsync(page);
            }            
        }

        private Page CreateAndBindPage(Type viewModelType)
        {
            var pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            var page = Activator.CreateInstance(pageType) as Page;
            var viewModel = Activator.CreateInstance(viewModelType) as ViewModelBase;
            page.BindingContext = viewModel;

            return page;
        }


        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return mappings[viewModelType];
        }

        private void CreatePageViewModelMappings()
        {
            mappings.Add(typeof(MainViewModel), typeof(MainPage));
            mappings.Add(typeof(EmailViewModel), typeof(EmailPage));
            mappings.Add(typeof(ProcessViewModel), typeof(ProcessPage));
        }
    }
}
