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

namespace SA
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RegistroNuevos rn = new RegistroNuevos();
        RegistroAsistencia ra = new RegistroAsistencia();
        ListadoES ls = new ListadoES();
        public MainWindow()
        {
            InitializeComponent();
            

        }

        private void btnRegistarAlumnos_Click(object sender, RoutedEventArgs e)
        {
            rn.Show();
            this.Hide();
            
        }

        private void btnRegistrarAsistencia_Click(object sender, RoutedEventArgs e)
        {
            ra.Show();
            this.Hide();

        }

        private void btnListado_Click(object sender, RoutedEventArgs e)
        {
            ls.Show();
            this.Hide();
           
        }

        private void btnPersonalizar_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("hola 3");
        }
    }
}
