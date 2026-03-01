using System.Windows;
using System.Windows.Input;
using Ticket_System_App.ViewModels;

namespace Ticket_System_App.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginWindow()
        {
            InitializeComponent();

            _viewModel = new LoginViewModel();
            DataContext = _viewModel;

            // Khi login thành công → mở MainWindow, đóng LoginWindow
            _viewModel.LoginSucceeded += OnLoginSucceeded;

            // Focus vào ô username khi mở
            Loaded += (s, e) => UsernameTextBox.Focus();
        }

        // PasswordBox không hỗ trợ binding → dùng event thủ công
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
        }

        private void OnLoginSucceeded()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        // Kéo cửa sổ bằng title bar
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        // Nút đóng
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
