using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UpravlenieRC.Client.modules;

namespace UpravlenieRC.Client.pages.user
{
    /// <summary>
    /// Логика взаимодействия для ChangeUserProfilePage.xaml
    /// </summary>
    public partial class ChangeUserProfilePage : Page
    {
        ChangeProfileType type;
        public ChangeUserProfilePage(ChangeProfileType type)
        {
            this.type = type;
            InitializeComponent();
            switch (type)
            {
                case ChangeProfileType.Password:
                    TB_Changer.Visibility = Visibility.Hidden;
                    Info_Changer.Visibility = Visibility.Hidden;
                    TB_passwordNew.Visibility = Visibility.Visible;
                    Info_passwordNew.Visibility = Visibility.Visible;
                    Info_OldPassword.Text = "Старый пароль";
                    break;
                case ChangeProfileType.Email:
                    Info_Changer.Text = "Новый email";
                    Btn_accept.Margin = new Thickness(0, -80, 0, 0);
                    break;
                case ChangeProfileType.Phone:
                    Info_Changer.Text = "Новый телефон";
                    Btn_accept.Margin = new Thickness(0, -80, 0, 0);
                    break;
            }
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (type)
            {
                case ChangeProfileType.Password:
                    await AuthAndRegestration.ChangePasswordAsync(TB_password.Password, TB_passwordNew.Password);
                    break;
                case ChangeProfileType.Email:
                    await AuthAndRegestration.ChangeEmailAsync(TB_Changer.Text, TB_password.Password);
                    break;
                case ChangeProfileType.Phone:
                    await AuthAndRegestration.ChangePhoneAsync(TB_Changer.Text, TB_password.Password);
                    break;
            }
        }
    }
    public enum ChangeProfileType
    {
        Password,
        Email,
        Phone
    }
}
