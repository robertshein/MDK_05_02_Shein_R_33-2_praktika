using AppKeyPass.Contexts;
using System.Windows;
using System.Windows.Controls;

namespace AppKeyPass.Pages
{
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private async void BtnRegister(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbLogin.Text))
            {
                MessageBox.Show("Необходимо указать логин пользователя");
                return;
            }
            if (string.IsNullOrEmpty(tbPassword.Password))
            {
                MessageBox.Show("Необходимо указать пароль пользователя");
                return;
            }
            if (tbPassword.Password != tbPasswordConfirm.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            bool success = await UserContext.Register(tbLogin.Text, tbPassword.Password);
            if (success)
            {
                MessageBox.Show("Пользователь успешно создан");
                MainWindow.Init.OpenPages(new Pages.Login());
            }
            else
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
            }
        }

        private void BtnBack(object sender, RoutedEventArgs e)
        {
            MainWindow.Init.OpenPages(new Pages.Login());
        }
    }
}
