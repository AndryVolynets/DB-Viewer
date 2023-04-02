using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace Teach.Kernel
{
    internal sealed class DatabaseConnection : IDisposable
    {
        public static DatabaseConnection Instance { get { return lazy.Value; } }

        public SqlConnection GetConnection()
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                TryExecute(() =>
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                });
            }

            return connection;
        }

        public void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                TryExecute(() => connection.Close());
            }
        }

        private static readonly Lazy<DatabaseConnection> lazy = new Lazy<DatabaseConnection>(() => new DatabaseConnection());
        private SqlConnection connection;
        private string connectionString;

        private bool disposedValue;

        private DatabaseConnection()
        {
            TryExecute(() =>
            {
                connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnect"].ConnectionString;
                connection = new SqlConnection(connectionString);
                connection.Open();
            });
        }

        ~DatabaseConnection()
        {
            Dispose(disposing: false);
        }

        private void TryExecute(Action action)
        {
            try
            {
                action();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening database connection: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    CloseConnection();
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
