using Microsoft.AspNetCore.Components;

namespace WaHub.Blazor.Services
{
    public class NavigationService
    {
        private readonly NavigationManager _navigationManager;

        public NavigationService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void Push(string uri)
        {
            _navigationManager.NavigateTo(uri);
        }

        public void Replace(string uri)
        {
            _navigationManager.NavigateTo(uri, replace: true);
        }

        public string GetCurrentPath()
        {
            var uri = new Uri(_navigationManager.Uri);
            return uri.AbsolutePath;
        }

        public bool IsExternalRoute(string path)
        {
            var externalRoutes = new[] { "/", "/login", "/register", "/docs", "/contact" };
            return externalRoutes.Contains(path);
        }
    }
}
