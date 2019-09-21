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

namespace SA
{
    /// <summary>
    /// Lógica de interacción para Excel.xaml
    /// </summary>
    public partial class Excel : Window
    {
        // Nuevo DataTable
        DataTable table;
        DataColumn column;
        string filename;
        Enlace enlace;
        //cosas del excel
        Excels.Application xlApp;
        Excels.Workbook xlWorkbook;
        Excels._Worksheet xlWorksheet;
        Excels.Range xlRange;

        public Excel( Enlace enlace)
        {
            this.enlace = enlace;
            InitializeComponent();
            
            table = new DataTable("Alumnos Cargados");
            // Creando primera columna
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ID";
            column.Caption = "ID";
            column.ReadOnly = true;
            column.Unique = true;

            // agregandola a la tabla
            table.Columns.Add(column);

            // Creando segunda columna
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "NombreCompleto";
            column.AutoIncrement = false;
            column.Caption = "Nombre Completo";
            column.ReadOnly = true;
            column.Unique = false;
            table.Columns.Add(column);

            // Creando tercera columna
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "GradoGrupo";
            column.AutoIncrement = false;
            column.Caption = "Grado y Grupo";
            column.ReadOnly = true;
            column.Unique = false;
            table.Columns.Add(column);

            // Creando cuarta columna
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Tutor";
            column.AutoIncrement = false;
            column.Caption = "Tutor";
            column.ReadOnly = true;
            column.Unique = false;
            table.Columns.Add(column);

            // Creando quinta columna
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Telefono";
            column.AutoIncrement = false;
            column.Caption = "Teléfono";
            column.ReadOnly = true;
            column.Unique = false;
            table.Columns.Add(column);

            // Creando sexta columna
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Observaciones";
            column.AutoIncrement = false;
            column.Caption = "Observaciones";
            column.ReadOnly = true;
            column.Unique = false;
            table.Columns.Add(column);
        }
        private void cerrar_excel()
        {
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            if(xlRange!=null)
            Marshal.ReleaseComObject(xlRange);
            if(xlWorksheet!=null)
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            if (xlWorkbook != null) {
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);
            }

            //quit and release
            if (xlApp!=null)
            {
                 xlApp.Quit();
                 Marshal.ReleaseComObject(xlApp);
            }
           
        }
        private void carga_excel(object sender, DoWorkEventArgs e)
        {
            try
            {
                //LO DE EXCEL

                //Create COM Objects. Create a COM object for everything that is referenced

                xlApp = new Excels.Application();
                xlWorkbook = xlApp.Workbooks.Open(filename);
                xlWorksheet = xlWorkbook.Sheets[1];
                xlRange = xlWorksheet.UsedRange;
                //contamos las columnas y filas :D
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                DataRow row;
                for (int i = 1; i <= rowCount + 1; i++)
                {
                    row = table.NewRow();


                    for (int j = 1; j <= colCount; j++)
                    {




                        //write the value to the console
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        {

                            row[j - 1] = xlRange.Cells[i, j].Value2.ToString();
                        }

                    }

                    table.Rows.Add(row);

                }
                this.Dispatcher.BeginInvoke(new Action(() =>
                {

                    dgExcel.ItemsSource = table.DefaultView;

                }));

                cerrar_excel();
            }
            catch (Exception)
            {
                cerrar_excel();
            }


            
        
        }

        
            private void btnCargarArchivo_Click(object sender, RoutedEventArgs e) {

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "Libro de Excel (*.xlsx)|*.xlsx|Libro de excel 97-2003 (*.xls)|*.xls";
            
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {

                barra.Visibility = Visibility.Visible;
                txtCargando.Visibility = Visibility.Visible;
                filename = dlg.FileName;
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

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result= MessageBox.Show("¿Está seguro que desea cargar estos datos?\n Eso borraría los datos existentes para cargar esta nueva información", "Alerta de carga de datos", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                bool error = false;
                enlace.conectar();
                enlace.borrado();
                foreach (DataRowView r in dgExcel.ItemsSource)
                {
                    
                    int i=enlace.insertar(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[3].ToString(), r[4].ToString(), r[5].ToString());
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
                if (error == true)
                {
                    MessageBox.Show("Hubo filas que no se pudieron insertar.\nRevise el formato y vuelva a intentarlo o ingreselos manualmente desde Registro de Alumnos.", "Error de inserción", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Datos cargados exitosamente.", "Carga de datos finalizada", MessageBoxButton.OK, MessageBoxImage.Information);

                }

            }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
        
 }

