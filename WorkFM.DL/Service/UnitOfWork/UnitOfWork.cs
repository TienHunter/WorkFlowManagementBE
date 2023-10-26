using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.DL.Service.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Field

        private readonly DbConnection _connection;
        private DbTransaction? _transaction = null;

        #endregion

        #region Constructor
        public UnitOfWork(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }
        #endregion

        #region Property
        public DbConnection Connection => _connection;

        public DbTransaction? Transaction => _transaction;
        #endregion

        #region Methods

        /// <summary>
        /// mở transaction
        /// </summary>
        /// created by: vdtien(22/10/2023)
        public void BeginTransaction()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _transaction = _connection.BeginTransaction();
            }
            else
            {
                _connection.Open();
                _transaction = _connection.BeginTransaction();

            }
        }

        /// <summary>
        /// mở transaction async
        /// </summary>
        /// <returns></returns>
        /// created by: vdtien(22/10/2023)
        public async Task BeginTransactionAsync()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _transaction = await _connection.BeginTransactionAsync();
            }
            else
            {
                await _connection.OpenAsync();
                _transaction = await _connection.BeginTransactionAsync();

            }
        }

        /// <summary>
        /// commit transaction
        /// </summary>
        /// created by: vdtien(22/10/2023)
        public void Commit()
        {
            _transaction?.Commit();
        }

        /// <summary>
        /// commit transaction async
        /// </summary>
        /// <returns></returns>
        /// created by: vdtien(22/10/2023)
        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }

            await DisposeAsync();
        }

        /// <summary>
        /// giải phóng transaction và đóng connection
        /// </summary>
        /// created by: vdtien (22/10/2023)
        public void Dispose()
        {
            _transaction?.Dispose();
            _transaction = null;

            _connection.Close();
        }

        /// <summary>
        /// giải phóng transaction và đóng connection
        /// </summary>
        /// <returns></returns>
        /// created by: vdtien (22/10/2023)
        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            _transaction = null;
            await _connection.CloseAsync();
        }

        /// <summary>
        /// rollback transaction
        /// </summary>
        /// created by: vdtien (22/10/2023)
        public void Rollback()
        {
            _transaction?.Rollback();
            Dispose();
        }

        /// <summary>
        /// rollback transaction async
        /// </summary>
        /// <returns></returns>
        /// created by: vdtien (22/10/2023)
        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
            await DisposeAsync();

        }
        #endregion
    }
}
