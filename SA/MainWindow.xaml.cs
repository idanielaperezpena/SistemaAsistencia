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
        RegistroNuevos rn; 
        RegistroAsistencia ra; 
        ListadoES ls;
        Personalizacion ps;

        public MainWindow()
        {
            InitializeComponent();
            Enlace enlace = new Enlace();
            enlace.tablas();
            String[] s = new string[2];
            s = enlace.consultaPersonalizacion();
            if (!string.IsNullOrEmpty(s[1]))
            {
                Uri uri = new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor." + s[1] + ".xaml");
                Application.Current.Resources.MergedDictionaries[0].Source = uri;
            }
           


        }

        private void btnRegistarAlumnos_Click(object sender, RoutedEventArgs e)
        {
           rn= new RegistroNuevos();
            rn.Show();
            this.Hide();
            
        }

        private void btnRegistrarAsistencia_Click(object sender, RoutedEventArgs e)
        {
            ra = new RegistroAsistencia();
            ra.Show();
            this.Hide();

        }

        private void btnListado_Click(object sender, RoutedEventArgs e)
        {
            ls= new ListadoES();
            ls.Show();
            this.Hide();
           
        }

        private void btnPersonalizar_Click(object sender, RoutedEventArgs e)
        {

            ps = new Personalizacion();
            ps.Show();
            this.Hide();
        }
    }
}
