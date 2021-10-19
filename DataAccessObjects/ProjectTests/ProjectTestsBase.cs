using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Transactions;

namespace ProjectTests
{
    [TestClass]
    public abstract class ProjectTestsBase
    {
        protected string ConnectionString { get; } = "Server=.\\SQLEXPRESS;Database=???;Trusted_Connection=True;";

        private TransactionScope transaction;

        [TestInitialize] // Gets called before every test runs
        public void Setup()
        {
            // Begin the transaction
            transaction = new TransactionScope(); // BEGIN TRANSACTION

            // Get the SQL Script to run
            string sql = File.ReadAllText("test-script.sql");

            // Execute the script
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup] // Gets called after every test runs
        public void Cleanup()
        {
            // Roll back the transaction
            if (transaction != null)
            {
                transaction.Dispose(); // ROLLBACK TRANSACTION
            }
        }

    }
}
