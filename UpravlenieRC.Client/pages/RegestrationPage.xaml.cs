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

namespace UpravlenieRC.Client.pages
{
    /// <summary>
    /// Логика взаимодействия для RegestrationPage.xaml
    /// </summary>
    public partial class RegestrationPage : Page
    {
        private int failedAttempts = 0;
        public RegestrationPage()
        {
            InitializeComponent();
        }
        public RegestrationPage(int failedAttempts)
        {
            this.failedAttempts = failedAttempts;
            InitializeComponent();
        }
    }
}
