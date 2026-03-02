using System.Collections.ObjectModel;
using System.Windows.Input;
using Ticket_System_App.Helpers;
using Ticket_System_App.Models;
using Ticket_System_App.Services;

namespace Ticket_System_App.ViewModels
{
    public class TicketListViewModel : BaseViewModel
    {
        private readonly bool _myTicketsOnly;

        public ObservableCollection<TicketModel> Tickets { get; } = new();

        private string _filterStatus = string.Empty;
        public string FilterStatus
        {
            get => _filterStatus;
            set { SetProperty(ref _filterStatus, value); }
        }

        private string _filterPriority = string.Empty;
        public string FilterPriority
        {
            get => _filterPriority;
            set { SetProperty(ref _filterPriority, value); }
        }

        // String collections for ComboBox ItemsSource (avoids ComboBoxItem binding issues)
        public List<string> StatusOptions { get; } = new() { "", "OPEN", "IN_PROGRESS", "RESOLVED", "CLOSED" };
        public List<string> PriorityOptions { get; } = new() { "", "LOW", "MEDIUM", "HIGH", "URGENT" };

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { SetProperty(ref _isLoading, value); OnPropertyChanged(nameof(IsNotLoading)); }
        }
        public bool IsNotLoading => !IsLoading;

        private string _statusMessage = string.Empty;
        public string StatusMessage { get => _statusMessage; set => SetProperty(ref _statusMessage, value); }

        public bool IsAdminOrSupporter => HttpClientService.Instance.Role is "ADMIN" or "SUPPORTER";

        public ICommand RefreshCommand { get; }
        public ICommand FilterCommand { get; }

        public event Action<int>? OpenDetailRequested;

        public TicketListViewModel(bool myTicketsOnly)
        {
            _myTicketsOnly = myTicketsOnly;
            RefreshCommand = new RelayCommand(async _ => await LoadAsync());
            FilterCommand  = new RelayCommand(async _ => await LoadAsync());
        }

        public async Task LoadAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = string.Empty;
                List<TicketModel> result;

                if (_myTicketsOnly)
                    result = await TicketApiService.Instance.GetMyTicketsAsync();
                else
                    result = await TicketApiService.Instance.GetAllAsync(
                        string.IsNullOrEmpty(FilterStatus) ? null : FilterStatus,
                        string.IsNullOrEmpty(FilterPriority) ? null : FilterPriority);

                Tickets.Clear();
                foreach (var t in result) Tickets.Add(t);

                if (Tickets.Count == 0)
                    StatusMessage = "Không có ticket nào.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void OpenDetail(int ticketId) => OpenDetailRequested?.Invoke(ticketId);
    }
}
