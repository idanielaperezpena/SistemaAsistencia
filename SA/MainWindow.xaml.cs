using System;
using System.Windows;

namespace SA
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        RegistroAsistencia ra;
        ListadoES ls;
        Personalizacion ps;
        Enlace enlace;
        
        public MainWindow()
        {
            InitializeComponent();
            enlace = new Enlace();
            enlace.conectar();
            enlace.tablas();
            enlace.cerrar();
            actualizacion_color();
           



        }
        public void actualizacion_color()
        {
            enlace.conectar();
          
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
            Login login = new Login(enlace,this);
            login.Show();
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
