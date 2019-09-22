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

using MaterialDesignThemes.Wpf;
using MaterialDesignColors;

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
        Enlace enlace;
        
        public MainWindow()
        {
            InitializeComponent();
            enlace = new Enlace();
            actualizacion_color();
           



        }
        public void actualizacion_color()
        {
            enlace.conectar();
            enlace.tablas();
            String[] s = new string[2];
            s = enlace.consultaPersonalizacion();
            enlace.cerrar();
            if (!string.IsNullOrEmpty(s[1]))
            {
                Uri uri = new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor." + s[1] + ".xaml");
                Application.Current.Resources.MergedDictionaries[0].Source = uri;
            }
        }

        private void btnRegistarAlumnos_Click(object sender, RoutedEventArgs e)
        {
            rn = new RegistroNuevos(enlace, this);
            rn.Show();
            this.Hide();

        }

        private void btnRegistrarAsistencia_Click(object sender, RoutedEventArgs e)
        {
            ra = new RegistroAsistencia(enlace, this);
            ra.Show();
            this.Hide();

        }

        private void btnListado_Click(object sender, RoutedEventArgs e)
        {
            ls = new ListadoES(enlace, this);
            ls.Show();
            this.Hide();

        }

        private void btnPersonalizar_Click(object sender, RoutedEventArgs e)
        {

            ps = new Personalizacion(enlace, this);
            ps.Show();
            this.Hide();
        }

        private void WInicio_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(1);

        }

        

       
        
    }
}
