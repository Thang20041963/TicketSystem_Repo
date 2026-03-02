using System.Windows.Controls;
using Ticket_System_App.Views;
using Ticket_System_App.ViewModels;

namespace Ticket_System_App.Views.Pages
{
    public partial class AddTicketPage : Page
    {
        private readonly AddTicketViewModel _vm;
        private readonly HomeWindow _home;

        public AddTicketPage(HomeWindow home)
        {
            InitializeComponent();
            _home = home;
            _vm = new AddTicketViewModel();
            DataContext = _vm;

            // After successful submit, navigate back to ticket list
            _vm.SubmitSucceeded += () =>
            {
                if (System.Windows.Navigation.NavigationService.GetNavigationService(this)?.CanGoBack == true)
                    System.Windows.Navigation.NavigationService.GetNavigationService(this)?.GoBack();
            };
        }
    }
}
