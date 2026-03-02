using System.Windows;
using System.Windows.Input;
using Ticket_System_App.ViewModels;
using Ticket_System_App.Views.Pages;

namespace Ticket_System_App.Views
{
    public partial class HomeWindow : Window
    {
        private readonly HomeViewModel _vm;

        public HomeWindow()
        {
            InitializeComponent();
            _vm = new HomeViewModel();
            DataContext = _vm;

            _vm.LogoutRequested += OnLogoutRequested;

            // Navigate to default page based on role
            if (_vm.IsEmployee)
                NavigateToMyTickets();
            else
                NavigateToAllTickets();
        }

        private void NavMyTickets_Click(object sender, RoutedEventArgs e) => NavigateToMyTickets();
        private void NavAllTickets_Click(object sender, RoutedEventArgs e) => NavigateToAllTickets();
        private void NavAddTicket_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new AddTicketPage(this));

        private void NavigateToMyTickets()
            => MainFrame.Navigate(new TicketListPage(this, myTicketsOnly: true));

        private void NavigateToAllTickets()
            => MainFrame.Navigate(new TicketListPage(this, myTicketsOnly: false));

        public void NavigateToDetail(int ticketId)
            => MainFrame.Navigate(new TicketDetailPage(this, ticketId));

        private void OnLogoutRequested()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();
    }
}
