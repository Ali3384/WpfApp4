using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Win32;
using System.Windows.Documents;
using System.Xml.Linq;
using System.IO;
using iText.IO.Image;
using System.Windows.Media;

namespace WpfApp4.Pages
{
    /// <summary>
    /// Логика взаимодействия для WelcomePage.xaml
    /// </summary>
    public partial class FinalPage : Page
    {

        bool isconnection = false;
        string side;
        public string onepiece { get; set; }
        string connectionString = Properties.Settings.Default.connection;
        string choosenLock;
        string choosenOnepieceStriker;
        string choosenMainStriker;
        string choosenCentralStriker;
        public int qtystrikers;
        public bool check;
        string extension;
        DataTable dataTable = new DataTable();
        public FinalPage()
        {
            InitializeComponent();
            checkisneedstriker();

            Page4 page4 = new Page4();

            if (check == true)
            {
                qtystrikers = 3;
                extension = Properties.Resources.extensionadd1;
            }
            else
            {
                qtystrikers = 2;
                extension = Properties.Resources.extensionadd2;
            }
            systemlabel.Content = page4.choosenSystem;
            onepiece = page4.str;
            getLock();
            getCentralStriker();
            getMainStriker();
            getOnePieceStriker();
            getonepiece();
            makeFinalTable();
            fillData();


        }

        private void butsettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WelcomePage());
            closing();
        }

        public void closing()
        {


            // Truncate tables
            TruncateTable(connectionString, "choosenlocks");
            TruncateTable(connectionString, "choosenplate");
            TruncateTable(connectionString, "choosensystem");
            TruncateTable(connectionString, "choosenmainstrikers");
            TruncateTable(connectionString, "choosenonepiecestrikers");
            TruncateTable(connectionString, "choosencentralstrikers");
            TruncateTable(connectionString, "finaltable");
            TruncateTable(connectionString, "isonepiece");
        }
        private void fillData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Item_Code, Quantity, Item_Description FROM finaltable";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                   
                    adapter.Fill(dataTable);
                    dataTable.Columns["Item_Code"].ColumnName = Properties.Resources.column_code;
                    dataTable.Columns["Quantity"].ColumnName = Properties.Resources.column_qty;
                    dataTable.Columns["Item_Description"].ColumnName = Properties.Resources.column_description;

                    finaltable.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while filling data: " + ex.Message);
            }
        }

        private void TruncateTable(string connectionString, string tableName)
        {
            string query = $"TRUNCATE TABLE {tableName}";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }
        }
        private void checkisneedstriker()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM finaltable WHERE Item_Description = @ItemDescription";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    byte[] addextension = Encoding.UTF8.GetBytes(Properties.Resources.extension);
                    command.Parameters.AddWithValue("@ItemDescription", addextension);

                    try
                    {
                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        check = count > 0;
                    }
                    catch (MySqlException ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }

        }
        private void makeFinalTable()
        {

            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string insertQuery = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@lock, @addlock, '1')";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    byte[] addlock = Encoding.UTF8.GetBytes(Properties.Resources.lock_add);
                    insertCmd.Parameters.AddWithValue("@lock", choosenLock);
                    insertCmd.Parameters.AddWithValue("addlock", addlock);
                    insertCmd.ExecuteNonQuery();
                }

                if (onepiece == "Single Strikers")
                {
                    string insert3Query = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@mainstrikers, @extension, @qtystrikers)";
                    using (MySqlCommand insertCmd = new MySqlCommand(insert3Query, connection))
                    {
                        byte[] addextension = Encoding.UTF8.GetBytes(extension);
                        insertCmd.Parameters.AddWithValue("@mainstrikers", choosenMainStriker);
                        insertCmd.Parameters.AddWithValue("@extension", addextension);
                        insertCmd.Parameters.AddWithValue("@qtystrikers", qtystrikers);
                        insertCmd.ExecuteNonQuery();
                    }
                    string insert2Query = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@centralstriker, @addcentralstriker, '1')";
                    using (MySqlCommand insertCmd = new MySqlCommand(insert2Query, connection))
                    {
                        byte[] addcentralstriker = Encoding.UTF8.GetBytes(Properties.Resources.centralstrikeradd);
                        insertCmd.Parameters.AddWithValue("@centralstriker", choosenCentralStriker);
                        insertCmd.Parameters.AddWithValue("@addcentralstriker", addcentralstriker);
                        insertCmd.ExecuteNonQuery();
                    }
                }
                else
                {

                    if (check == true)
                    {
                        string insert4Query = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@onepiecestriker, @onepiecestrikeradd, '1')";
                        using (MySqlCommand insertCmd = new MySqlCommand(insert4Query, connection))
                        {
                            byte[] onepiecestrikeradd = Encoding.UTF8.GetBytes(Properties.Resources.onepiecestrikeradd);
                            insertCmd.Parameters.AddWithValue("@onepiecestriker", choosenOnepieceStriker);
                            insertCmd.Parameters.AddWithValue("@onepiecestrikeradd", onepiecestrikeradd);
                            insertCmd.ExecuteNonQuery();
                        }
                        string insert3Query = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@mainstrikers, @addextensionstriker, '1')";
                        using (MySqlCommand insertCmd = new MySqlCommand(insert3Query, connection))
                        {
                            byte[] addextensionstriker = Encoding.UTF8.GetBytes(Properties.Resources.extensionstrikeradd);
                            insertCmd.Parameters.AddWithValue("@mainstrikers", choosenMainStriker);
                            insertCmd.Parameters.AddWithValue("@addextensionstriker", addextensionstriker);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insert4Query = "INSERT INTO finaltable (Item_Code, Item_Description, Quantity) VALUES (@onepiecestriker, @onepiecestrikeradd, '1')";
                        using (MySqlCommand insertCmd = new MySqlCommand(insert4Query, connection))
                        {
                            byte[] onepiecestrikeradd = Encoding.UTF8.GetBytes(Properties.Resources.onepiecestrikeradd);
                            insertCmd.Parameters.AddWithValue("@onepiecestriker", choosenOnepieceStriker);
                            insertCmd.Parameters.AddWithValue("@onepiecestrikeradd", onepiecestrikeradd);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting data into choosenmainstrikers table: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void getLock()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Lock_Code FROM choosenlocks";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        choosenLock = result.ToString();

                    }
                    else
                    {
                        MessageBox.Show("No lock found.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while retrieving and inserting system info: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void getonepiece()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Result FROM isonepiece";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        onepiece = result.ToString();

                    }
                    else
                    {
                        MessageBox.Show("No result found.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while retrieving and inserting system info: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void getMainStriker()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Striker_Code FROM choosenmainstrikers";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        choosenMainStriker = result.ToString();

                    }
                    else
                    {
                        MessageBox.Show("No system found.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while retrieving and inserting system info: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void getCentralStriker()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Striker_Code FROM choosencentralstrikers";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        choosenCentralStriker = result.ToString();

                    }
                    else
                    {
                        MessageBox.Show("No system found.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while retrieving and inserting system info: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void getOnePieceStriker()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string systemQuery = "SELECT Striker_Code FROM choosenonepiecestrikers";
                using (MySqlCommand plateCmd = new MySqlCommand(systemQuery, connection))
                {
                    object result = plateCmd.ExecuteScalar(); // ExecuteScalar to get a single value
                    if (result != null)
                    {
                        // Assuming the Strike_Plate is of string type, you can adjust accordingly
                        choosenOnepieceStriker = result.ToString();

                    }
                    else
                    {
                        MessageBox.Show("No system found.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while retrieving and inserting system info: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void export_pdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create a SaveFileDialog to prompt the user for the file location
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    // Create a PdfWriter
                    PdfWriter writer = new PdfWriter(filePath);

                    // Create a PdfDocument
                    PdfDocument pdf = new PdfDocument(writer);

                    // Create a Document
                    Document document = new Document(pdf);

                    // Add header
                    

                    // Add header image

                    using (MemoryStream ms = new MemoryStream())
                    {
                        Properties.Resources.header.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // Assuming the image is PNG format, adjust if necessary
                        byte[] imageBytes = ms.ToArray();

                        // Create ImageData from byte array
                        ImageData imageData = ImageDataFactory.Create(imageBytes);

                        // Create iText Image element
                        iText.Layout.Element.Image headerImage = new iText.Layout.Element.Image(imageData);

                        // Add the image to the document
                        document.Add(headerImage);
                    }
                    iText.Layout.Element.Paragraph header = new iText.Layout.Element.Paragraph("Date: " + DateTime.Now.ToString("yyyy-MM-dd") + "\n" +
                                                        "Order for: FUHR Polska\n\n")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFontSize(16);
                    document.Add(header);
                    // Create table
                    iText.Layout.Element.Table pdfTable = new iText.Layout.Element.Table(dataTable.Columns.Count);

                    // Add headers
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        pdfTable.AddHeaderCell(column.ColumnName);
                    }

                    // Add rows
                    foreach (DataRow row in dataTable.Rows)
                    {
                        foreach (object item in row.ItemArray)
                        {
                            pdfTable.AddCell(item.ToString());
                        }
                    }

                    document.Add(pdfTable);

                    // Close the Document
                    document.Close();

                    MessageBox.Show("PDF file exported successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to PDF: " + ex.ToString());
            }
        }

        private void export_pdf_Копировать_Click(object sender, RoutedEventArgs e)
        {
            finaltable.IsReadOnly = false;
            finaltable.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
        }
        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected row
            DataRowView selectedRow = (DataRowView)finaltable.SelectedItem;

            if (selectedRow != null)
            {
                // Get the index of the selected row
                int rowIndex = dataTable.Rows.IndexOf(selectedRow.Row);

                // Increase the quantity by 1
                int currentQuantity = int.Parse(selectedRow.Row[Properties.Resources.column_qty].ToString());
                currentQuantity++;
                selectedRow.Row[Properties.Resources.column_qty] = currentQuantity;

                // Update the DataTable
                dataTable.AcceptChanges();
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected row
            DataRowView selectedRow = (DataRowView)finaltable.SelectedItem;

            if (selectedRow != null)
            {
                // Get the index of the selected row
                int rowIndex = dataTable.Rows.IndexOf(selectedRow.Row);

                // Decrease the quantity by 1, but ensure it doesn't go below 0
                int currentQuantity = int.Parse(selectedRow.Row[Properties.Resources.column_qty].ToString());
                if (currentQuantity > 0)
                {
                    currentQuantity--;
                    selectedRow.Row[Properties.Resources.column_qty] = currentQuantity;

                    // Update the DataTable
                    dataTable.AcceptChanges();
                }
            }
        }
    }
}
