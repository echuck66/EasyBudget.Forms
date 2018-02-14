//
//  Copyright 2018  CrawfordNET Solutions, LLC
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using EasyBudget.Models.DataModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace EasyBudget.Data
{
    public class EasyBudgetContext : DbContext
    {
        private string _dbFilePath;

        public DbSet<BudgetCategory> BudgetCategory { get; set; }

        public DbSet<CheckingAccount> CheckingAccount { get; set; }

        public DbSet<CheckingDeposit> CheckingDeposit { get; set; }

        public DbSet<CheckingWithdrawal> CheckingWithdrawal { get; set; }

        public DbSet<ExpenseItem> ExpenseItem { get; set; }

        public DbSet<IncomeItem> IncomeItem { get; set; }

        public DbSet<SavingsAccount> SavingsAccount { get; set; }

        public DbSet<SavingsDeposit> SavingsDeposit { get; set; }

        public DbSet<SavingsWithdrawal> SavingsWithdrawal { get; set; }

        public DbSet<BankAccountFundsTransfer> BankAccountFundsTransfer { get; set; }

        public EasyBudgetContext(string dbFilePath)
        {
            _dbFilePath = dbFilePath;
            this.EnsureDatabaseIsCreated();
            Database.EnsureCreated();
            //Database.Migrate();
        }

        private void EnsureDatabaseIsCreated()
        {
            if (!string.IsNullOrEmpty(_dbFilePath))
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(_dbFilePath))
                {
                    conn.CreateTable<BudgetCategory>();
                    conn.CreateTable<CheckingAccount>();
                    conn.CreateTable<CheckingDeposit>();
                    conn.CreateTable<CheckingWithdrawal>();
                    conn.CreateTable<ExpenseItem>();
                    conn.CreateTable<IncomeItem>();
                    conn.CreateTable<SavingsAccount>();
                    conn.CreateTable<SavingsDeposit>();
                    conn.CreateTable<SavingsWithdrawal>();
                    conn.CreateTable<BankAccountFundsTransfer>();
                }
            }
        }
        // Used for Unit Testing
        public EasyBudgetContext(DbContextOptions<EasyBudgetContext> options)
            : base(options)
        { 
            
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
                var connectionStringBuilder = new SqliteConnectionStringBuilder
                {
                    DataSource = _dbFilePath
                };

                var connectionString = connectionStringBuilder.ToString();
                var connection = new SqliteConnection(connectionString);

                optionsBuilder.UseSqlite(connection);

                //optionsBuilder.UseSqlite("Filename ={" + _dbFilePath + "}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
