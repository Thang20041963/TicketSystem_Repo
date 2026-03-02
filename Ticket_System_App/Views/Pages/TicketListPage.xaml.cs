using System.Windows.Controls;
using Ticket_System_App.Views;
using Ticket_System_App.ViewModels;

namespace Ticket_System_App.Views.Pages
{
    public partial class TicketListPage : Page
    {
        private readonly TicketListViewModel _vm;
        private readonly HomeWindow _home;

        public TicketListPage(HomeWindow home, bool myTicketsOnly)
        {
            InitializeComponent();
            _home = home;
            _vm = new TicketListViewModel(myTicketsOnly);
            DataContext = _vm;
            _vm.OpenDetailRequested += id => _home.NavigateToDetail(id);
            Loaded += async (_, _) => await _vm.LoadAsync();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView lv && lv.SelectedItem is Ticket_System_App.Models.TicketModel ticket)
            {
                _vm.OpenDetail(ticket.Id);
                lv.SelectedItem = null;
            }
        }
    }
}
