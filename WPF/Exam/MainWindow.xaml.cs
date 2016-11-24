using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.SQLite;
using System.Data;

namespace Exam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string dataBase = "MyDatabase.sqlite";
        private const string createQuery = "CREATE  TABLE  IF NOT EXISTS 'Footballs' ('lastName' VARCHAR, 'firstName' VARCHAR, 'birthday' DATETIME, 'position' VARCHAR, 'club' VARCHAR)";
        private const string selectQuery = "SELECT * FROM Footballs";

        public MainWindow()
        {
            InitializeComponent();
            GetDataToTree();
        }

        private void GetDataToTree()
        {
            SQLiteConnectionStringBuilder conString = new SQLiteConnectionStringBuilder();
            conString.DataSource = dataBase;
            using (SQLiteConnection connect = new SQLiteConnection(conString.ConnectionString))
            {
                connect.Open();
                SQLiteCommand command = new SQLiteCommand(createQuery, connect);
                command.ExecuteNonQuery();

                command = new SQLiteCommand(selectQuery, connect);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //reader.IsDBNull();
                        TreeViewItem item = new TreeViewItem();
                        item.Header = reader["lastName"] + " " + reader["firstName"];
                        for (int i = 2; i < reader.FieldCount; i++)
                        {
                            item.Items.Add(reader.GetString(i));
                        }
                        treeView.Items.Add(item);
                    }
                }
                command.Dispose();

                //string sql = "INSERT INTO Main (surname, club) values ('Ярмоленко', 'Динамо')";
                //command = new SQLiteCommand(sql, connect);
                //command.ExecuteNonQuery();

                //DataSet dataSet = new DataSet();
                //SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM Main", connect);
                //dataAdapter.Fill(dataSet);
                //dataGrid.ItemsSource = dataSet.Tables[0].DefaultView;
            }
        }
    }
}
