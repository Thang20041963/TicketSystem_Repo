using System.Windows.Controls;
using System.Windows.Navigation;
using Ticket_System_App.ViewModels;
using Ticket_System_App.Views;

namespace Ticket_System_App.Views.Pages
{
    public partial class TicketDetailPage : Page
    {
        private readonly TicketDetailViewModel _vm;
        private readonly HomeWindow _home;
        private readonly int _ticketId;

        public TicketDetailPage(HomeWindow home, int ticketId)
        {
            InitializeComponent();
            _home = home;
            _ticketId = ticketId;
            _vm = new TicketDetailViewModel(ticketId);
            DataContext = _vm;

            Loaded += async (_, _) => await _vm.LoadAsync();
            Unloaded += async (_, _) => await _vm.UnloadAsync();
        }

        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }
    }
}
