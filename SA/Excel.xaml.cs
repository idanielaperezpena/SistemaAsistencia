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
using System.Runtime.InteropServices;
using System.Data;
using Excels=Microsoft.Office.Interop.Excel;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Data.OleDb;

namespace SA
{
    /// <summary>
    /// Lógica de interacción para Excel.xaml
    /// </summary>
    public partial class Excel : Window
    {
        // Nuevo DataTable
        DataTable table;
        DataGrid dg;
       string filename;
        Enlace enlace;
        bool error = false;
        //cosas del excel
        //Excels.Application xlApp;
        //Excels.Workbook xlWorkbook;
        //Excels._Worksheet xlWorksheet;
        //Excels.Range xlRange;

        public Excel( Enlace enlace)
        {
            this.enlace = enlace;
            InitializeComponent();
            
            table = new DataTable("Alumnos Cargados");
            



        }
        
        private  void guarda_bd(object sender, DoWorkEventArgs e)
        {
            
            enlace.conectar();
            enlace.borrado();
            foreach (DataRowView r in dg.ItemsSource)
            {

                int i = enlace.insertar(r[0].ToString().ToUpper(), r[1].ToString().ToUpper(), r[2].ToString().ToUpper(), r[3].ToString().ToUpper(), r[4].ToString().ToUpper(), r[5].ToString().ToUpper());
                if (i == 0)
                {
                    r.Delete();
                }
                else
                {
                    error = true;
                }

            }

            enlace.cerrar();
            

        }


        private void carga_excel(object sender, DoWorkEventArgs e)
        {
            
            DataTable dt = new DataTable("x");
          
            string filePath = filename;
            string conString = string.Empty;
            FileInfo fi = new FileInfo(filePath);
            string extension = fi.Extension;

            switch (extension)
            {
                case ".xls": //Excel 97-03.
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                case ".xlsx": //Excel 07 and above.
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }

            //create datatable object

            conString = string.Format(conString, filePath);

            //Use OldDb to read excel
            using (OleDbConnection connExcel = new OleDbConnection(conString))
            {
                using (OleDbCommand cmdExcel = new OleDbCommand())
                {
                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                    {
                        cmdExcel.Connection = connExcel;

                        //Get the name of First Sheet.
                        connExcel.Open();
                        DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        //Read Data from First Sheet.
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        odaExcel.SelectCommand = cmdExcel;
                        odaExcel.Fill(table);
                        connExcel.Close();
                    }
                }
            }

            
            //bind datatable with GridView
            this.Dispatcher.BeginInvoke(new Action(() =>
                    {

                        dgExcel.ItemsSource = table.DefaultView;

                    }));

        }


        private void btnCargarArchivo_Click(object sender, RoutedEventArgs e) {

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "Libro de Excel (*.xlsx)|*.xlsx|Libro de excel 97-2003 (*.xls)|*.xls";
            
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {

                filename = dlg.FileName;

                barra.Visibility = Visibility.Visible;
                txtCargando.Visibility = Visibility.Visible;
                textbRuta.Text = filename;
                btnCargarArchivo.IsEnabled = false;
                btnGuardar.IsEnabled = false;
                btnRegresar.IsEnabled = false;
                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += carga_excel;
                worker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
                worker.RunWorkerAsync();


            }

          

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            barra.Visibility = Visibility.Hidden;
            txtCargando.Visibility = Visibility.Hidden;
            btnCargarArchivo.IsEnabled = true;
            btnGuardar.IsEnabled = true;
            btnRegresar.IsEnabled = true;
        }
        private void backgroundWorker_RunWorkerCompleted_bd(object sender, RunWorkerCompletedEventArgs e)
        {
            barra.Visibility = Visibility.Hidden;
            txtCargando.Visibility = Visibility.Hidden;
            btnCargarArchivo.IsEnabled = true;
            btnGuardar.IsEnabled = true;
            btnRegresar.IsEnabled = true;
            if (error == true)
            {
                MessageBox.Show("Hubo filas que no se pudieron insertar.\nRevise el formato y vuelva a intentarlo o ingreselos manualmente desde Registro de Alumnos.", "Error de inserción", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Datos cargados exitosamente.", "Carga de datos finalizada", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            dgExcel.Items.Refresh();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult result= MessageBox.Show("¿Está seguro que desea cargar estos datos?\n Eso borraría los datos existentes para cargar esta nueva información.", "Alerta de carga de datos", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                dg = dgExcel;
                barra.Visibility = Visibility.Visible;
                txtCargando.Visibility = Visibility.Visible;
               
                btnCargarArchivo.IsEnabled = false;
                btnGuardar.IsEnabled = false;
                btnRegresar.IsEnabled = false;
                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += guarda_bd;
                worker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted_bd;
                worker.RunWorkerAsync();
                

            }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
        
 }

