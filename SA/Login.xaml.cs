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
using System.Windows.Shapes;

namespace SA
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        MainWindow mw;
        string clave = "1234";

        public Login()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            if (txtclave.Text == clave)
            {
                mw = new MainWindow();
                mw.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Clave incorrecta, intente de nuevo");
            }
          
        }

        private void Login_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(1);

        }
    }
}
