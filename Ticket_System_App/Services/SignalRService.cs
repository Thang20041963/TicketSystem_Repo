using Microsoft.AspNetCore.SignalR.Client;
using Ticket_System_App.Models;

namespace Ticket_System_App.Services
{
    public class SignalRService
    {
        private static SignalRService? _instance;
        public static SignalRService Instance => _instance ??= new SignalRService();

        private HubConnection? _connection;

        public event Action<CommentModel>? NewCommentReceived;

        private SignalRService() { }

        public async Task StartAsync()
        {
            if (_connection != null) return;

            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7200/hubs/ticket", options =>
                {
                    options.AccessTokenProvider = () =>
                        Task.FromResult<string?>(HttpClientService.Instance.Token);
                })
                .WithAutomaticReconnect()
                .Build();

            _connection.On<CommentModel>("NewComment", comment =>
            {
                NewCommentReceived?.Invoke(comment);
            });

            await _connection.StartAsync();
        }

        public async Task StopAsync()
        {
            if (_connection == null) return;
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _connection = null;
        }

        public async Task JoinTicketAsync(int ticketId)
        {
            if (_connection?.State == HubConnectionState.Connected)
                await _connection.InvokeAsync("JoinTicketAsync", ticketId);
        }

        public async Task LeaveTicketAsync(int ticketId)
        {
            if (_connection?.State == HubConnectionState.Connected)
                await _connection.InvokeAsync("LeaveTicketAsync", ticketId);
        }
    }
}
