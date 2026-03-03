using System.Collections.ObjectModel;
using System.Windows.Input;
using Ticket_System_App.Helpers;
using Ticket_System_App.Models;
using Ticket_System_App.Services;

namespace Ticket_System_App.ViewModels
{
    public class TicketDetailViewModel : BaseViewModel
    {
        private readonly int _ticketId;

        public ObservableCollection<CommentModel> Comments { get; } = new();
        public ObservableCollection<StatusHistoryModel> StatusHistories { get; } = new();
        public ObservableCollection<UserModel> Supporters { get; } = new();

        private TicketDetailModel? _ticket;
        public TicketDetailModel? Ticket { get => _ticket; set => SetProperty(ref _ticket, value); }

        private string _newComment = string.Empty;
        public string NewComment { get => _newComment; set => SetProperty(ref _newComment, value); }

        private string _selectedStatus = string.Empty;
        public string SelectedStatus { get => _selectedStatus; set => SetProperty(ref _selectedStatus, value); }

        private UserModel? _selectedSupporter;
        public UserModel? SelectedSupporter { get => _selectedSupporter; set => SetProperty(ref _selectedSupporter, value); }

        private string _statusMessage = string.Empty;
        public string StatusMessage { get => _statusMessage; set => SetProperty(ref _statusMessage, value); }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        public bool IsAdminOrSupporter => HttpClientService.Instance.Role is "ADMIN" or "SUPPORTER";
        public bool IsAdmin => HttpClientService.Instance.Role == "ADMIN";

        public List<string> StatusOptions { get; } = new() { "OPEN", "IN_PROGRESS", "RESOLVED", "CLOSED" };

        public ICommand AddCommentCommand { get; }
        public ICommand UpdateStatusCommand { get; }
        public ICommand AssignCommand { get; }

        public TicketDetailViewModel(int ticketId)
        {
            _ticketId = ticketId;

            AddCommentCommand = new RelayCommand(
                async _ => await AddCommentAsync(),
                _ => !string.IsNullOrWhiteSpace(NewComment));

            UpdateStatusCommand = new RelayCommand(
                async _ => await UpdateStatusAsync(),
                _ => !string.IsNullOrWhiteSpace(SelectedStatus));

            AssignCommand = new RelayCommand(
                async _ => await AssignAsync(),
                _ => SelectedSupporter != null);

            // SignalR – receive real-time comments
            SignalRService.Instance.NewCommentReceived += OnNewCommentReceived;
        }

        public async Task LoadAsync()
        {
            try
            {
                IsLoading = true;
                var detail = await TicketApiService.Instance.GetByIdAsync(_ticketId);
                Ticket = detail;
                SelectedStatus = detail.Status;

                Comments.Clear();
                foreach (var c in detail.Comments) Comments.Add(c);

                StatusHistories.Clear();
                foreach (var h in detail.StatusHistories) StatusHistories.Add(h);

                // Load danh sách supporters (chỉ cho ADMIN/SUPPORTER)
                if (IsAdminOrSupporter)
                {
                    var supporters = await UserApiService.Instance.GetSupportersAsync();
                    Supporters.Clear();
                    foreach (var s in supporters) Supporters.Add(s);

                    // Pre-select current assignee nếu có
                    if (detail.AssigneeId.HasValue)
                        SelectedSupporter = Supporters.FirstOrDefault(s => s.Id == detail.AssigneeId.Value);
                }

                await SignalRService.Instance.JoinTicketAsync(_ticketId);
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

        public async Task UnloadAsync()
        {
            SignalRService.Instance.NewCommentReceived -= OnNewCommentReceived;
            await SignalRService.Instance.LeaveTicketAsync(_ticketId);
        }

        private async Task AddCommentAsync()
        {
            try
            {
                var added = await CommentApiService.Instance.AddAsync(_ticketId, NewComment);
                // Comment will arrive via SignalR; add locally as fallback if it doesn't
                NewComment = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi thêm comment: {ex.Message}";
            }
        }

        private async Task UpdateStatusAsync()
        {
            try
            {
                await TicketApiService.Instance.UpdateStatusAsync(_ticketId, SelectedStatus);
                StatusMessage = "Cập nhật trạng thái thành công!";
                await LoadAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi: {ex.Message}";
            }
        }

        private async Task AssignAsync()
        {
            if (SelectedSupporter == null) return;
            try
            {
                await TicketApiService.Instance.AssignAsync(_ticketId, SelectedSupporter.Id);
                StatusMessage = $"Đã gán ticket cho {SelectedSupporter.UserName}!";
                await LoadAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi gán ticket: {ex.Message}";
            }
        }

        private void OnNewCommentReceived(CommentModel comment)
        {
            // Marshal to UI thread
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                // Avoid duplicates
                if (Comments.All(c => c.Id != comment.Id))
                    Comments.Add(comment);
            });
        }
    }
}
