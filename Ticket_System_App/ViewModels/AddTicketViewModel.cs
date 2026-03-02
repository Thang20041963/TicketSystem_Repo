using System.Windows.Input;
using Ticket_System_App.Helpers;
using Ticket_System_App.Models;
using Ticket_System_App.Services;

namespace Ticket_System_App.ViewModels
{
    public class AddTicketViewModel : BaseViewModel
    {
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set { SetProperty(ref _title, value); OnPropertyChanged(nameof(CanSubmit)); }
        }

        private string _description = string.Empty;
        public string Description { get => _description; set => SetProperty(ref _description, value); }

        private string _selectedPriority = "MEDIUM";
        public string SelectedPriority { get => _selectedPriority; set => SetProperty(ref _selectedPriority, value); }

        private string _statusMessage = string.Empty;
        public string StatusMessage { get => _statusMessage; set => SetProperty(ref _statusMessage, value); }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        public bool CanSubmit => !string.IsNullOrWhiteSpace(Title) && !IsLoading;

        public List<string> Priorities { get; } = new() { "LOW", "MEDIUM", "HIGH", "URGENT" };

        public ICommand SubmitCommand { get; }
        public event Action? SubmitSucceeded;

        public AddTicketViewModel()
        {
            SubmitCommand = new RelayCommand(
                async _ => await SubmitAsync(),
                _ => CanSubmit);
        }

        private async Task SubmitAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = string.Empty;

                await TicketApiService.Instance.CreateAsync(new CreateTicketRequestModel
                {
                    Title = Title,
                    Description = string.IsNullOrWhiteSpace(Description) ? null : Description,
                    Priority = SelectedPriority
                });

                StatusMessage = "✅ Ticket đã được tạo thành công!";
                Title = string.Empty;
                Description = string.Empty;
                SelectedPriority = "MEDIUM";

                SubmitSucceeded?.Invoke();
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Lỗi: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
