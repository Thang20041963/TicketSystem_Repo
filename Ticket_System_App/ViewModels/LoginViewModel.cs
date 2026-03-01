using System.Windows;
using System.Windows.Input;
using Ticket_System_App.Helpers;
using Ticket_System_App.Services;

namespace Ticket_System_App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged(nameof(IsNotLoading));
            }
        }

        public bool IsNotLoading => !IsLoading;

        public ICommand LoginCommand { get; }

        // Event để View lắng nghe khi login thành công
        public event Action? LoginSucceeded;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(
                async _ => await ExecuteLoginAsync(),
                _ => IsNotLoading && !string.IsNullOrWhiteSpace(Username)
            );
        }

        private async Task ExecuteLoginAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                await ApiService.Instance.LoginAsync(Username, Password);

                // Login thành công → thông báo cho View
                LoginSucceeded?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
