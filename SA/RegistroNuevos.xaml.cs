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
using System.Data;
using System.Data.SQLite;

namespace SA
{
    /// <summary>
    /// Lógica de interacción para RegistroNuevos.xaml
    /// </summary>
    public partial class RegistroNuevos : Window
    {
        Enlace enlace;
        public RegistroNuevos()
        {
            InitializeComponent();
            enlace = new Enlace();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            adapter = enlace.consulta();
            DataTable table = new DataTable("ALUMNOS");
            adapter.Fill(table);
            dgAlumnos.ItemsSource = table.DefaultView;
            adapter.Update(table);

        }

        private void btnRegresarMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }



        private void btnGuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            enlace = new Enlace();
            //enlace.insertar();

            enlace.insertar(txtID.Text, txtNombre.Text, txtGrupo.Text, txtObservaciones.Text, "foto");
            MessageBox.Show("Registrado con éxito");
            limpiar();
        }

        public void limpiar()
        {
            txtID.Text = String.Empty;
            txtNombre.Text = String.Empty;
            txtGrupo.Text = String.Empty;
            txtObservaciones.Text = String.Empty;
            //foto
        }

       
    }
}
