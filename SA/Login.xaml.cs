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
        MainWindow main;
        Enlace enlace;
        bool confirmacion;
        string clave = "1234";

        public Login( Enlace enlace, MainWindow main)
        {
            InitializeComponent();
            this.enlace = enlace;
            this.main = main;
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            if (txtclave.Password == clave)
            {
                confirmacion = true;
               RegistroNuevos rn = new RegistroNuevos(enlace, main);
                rn.Show();
                this.Close();
            }
            else
            {
                confirmacion = false;
                MessageBox.Show("Clave incorrecta, intente de nuevo");
            }
          
        }

        private void Login_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (confirmacion == false)
            {
                main.Show();
            }
           

        }
    }
}
