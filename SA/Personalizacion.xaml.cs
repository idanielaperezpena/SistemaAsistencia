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
    /// Lógica de interacción para Personalizacion.xaml
    /// </summary>
    public partial class Personalizacion : Window
    {
        public Personalizacion()
        {
            InitializeComponent();
        }
        private void btnRegresarMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Hide();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnAplicar_Click(object sender, RoutedEventArgs e)
        {
            
            Enlace enlace = new Enlace();
            if (rdioAmarillo.IsChecked == true)
            {
                enlace.color("Amber");
                
            }
            if (rdioAzul.IsChecked == true)
            {
                enlace.color("Indigo");
            }
            if (rdioGris.IsChecked == true)
            {
                enlace.color("Grey");
            }
            if (rdioMorado.IsChecked == true)
            {
                enlace.color("DeepPurple");
            }
            if (rdioNaranja.IsChecked == true)
            {
                enlace.color("DeepOrgange");
            }
            if (rdioRojo.IsChecked == true)
            {
                enlace.color("Red");
            }
            if (rdioRosa.IsChecked == true)
            {
                enlace.color("Pink");
            }
            if (rdioTurquesa.IsChecked == true)
            {
                enlace.color("Teal");
            }
            if (rdioVerde.IsChecked == true)
            {
                enlace.color("Green");
            }
           


            
            
            
           
            

        }
    }
}
