using System.Windows.Input;
using Ticket_System_App.Helpers;
using Ticket_System_App.Services;

namespace Ticket_System_App.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly HttpClientService _http = HttpClientService.Instance;

        public string WelcomeText => $"Xin chào, {_http.Username}";
        public string RoleBadge => _http.Role ?? "";

        // Role-based UI visibility
        public bool IsEmployee => _http.Role == "EMPLOYEE";
        public bool IsAdminOrSupporter => _http.Role is "ADMIN" or "SUPPORTER";
        public bool IsAdmin => _http.Role == "ADMIN";

        public ICommand LogoutCommand { get; }
        public event Action? LogoutRequested;

        public HomeViewModel()
        {
            LogoutCommand = new RelayCommand(_ => ExecuteLogout());
        }

        private async void ExecuteLogout()
        {
            await SignalRService.Instance.StopAsync();
            HttpClientService.Instance.Logout();
            LogoutRequested?.Invoke();
        }
    }
}
