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
            enlace.conectar();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            adapter = enlace.consulta();
            DataTable table = new DataTable("ALUMNOS");
            adapter.Fill(table);
            dgAlumnos.ItemsSource = table.DefaultView;
            adapter.Update(table);
            enlace.cerrar();
        }

        private void btnRegresarMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void btnGuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            // enlace = new Enlace(); ya tenias tu enlace global e inicializada en el constructor
            enlace.conectar();
            DataRowView row = dgAlumnos.SelectedItem as DataRowView;
            if (!String.IsNullOrWhiteSpace(txtID.Text) || !String.IsNullOrWhiteSpace(txtNombre.Text) || !String.IsNullOrWhiteSpace(txtGrupo.Text))
            {
                if (row == null)
                {
                    int contador = enlace.consulta_existencia(txtID.Text);
                    if (contador == 0)
                    {
                        enlace.insertar(txtID.Text, txtNombre.Text, txtGrupo.Text, txtObservaciones.Text, "foto");
                        MessageBox.Show("Alumno registrado con éxito.");
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("El ID de alumno ya existe.");
                    }
                }

                else
                {
                    if (txtID.Text == row["ID"].ToString())
                    {
                        enlace.actualizar(txtNombre.Text, txtGrupo.Text, txtObservaciones.Text, "foto", txtID.Text);
                        MessageBox.Show("Datos de alumno actualizados.");
                        limpiar();
                    }
                    else
                    {
                        if (txtNombre.Text == row["NOMBRE"].ToString())
                        {
                            MessageBoxResult result = MessageBox.Show("Ya hay un alumno registrado con este nombre, ¿Desea continuar?", "Registro", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            switch (result)
                            {
                                case MessageBoxResult.OK:
                                    enlace.insertar(txtID.Text, txtNombre.Text, txtGrupo.Text, txtObservaciones.Text, "foto");
                                    MessageBox.Show("Alumno xx registrado con éxito.");
                                    limpiar();
                                    break;

                                case MessageBoxResult.Cancel:
                                    break;
                            }
                        }
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Hay campos necesarios vacíos");
            }

            dgAlumnos.ItemsSource = null;
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            adapter = enlace.consulta();
            DataTable table = new DataTable("ALUMNOS");
            adapter.Fill(table);
            dgAlumnos.ItemsSource = table.DefaultView;
            adapter.Update(table);
            enlace.cerrar();
        }

        public void limpiar()
        {
            txtID.Text = String.Empty;
            txtNombre.Text = String.Empty;
            txtGrupo.Text = String.Empty;
            txtObservaciones.Text = String.Empty;
            //foto
        }

        private void dgAlumnos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = dgAlumnos.SelectedItem as DataRowView;
            if (row != null)
            {
                txtID.Text = row["ID"].ToString();
                txtNombre.Text = row["NOMBRE"].ToString();
                txtGrupo.Text = row["GRADO_GRUPO"].ToString();
                txtObservaciones.Text = row["OBSERVACIONES"].ToString();
            } 
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //  TODO DANN
            //Environment.Exit(1);
        }     
    }
}
