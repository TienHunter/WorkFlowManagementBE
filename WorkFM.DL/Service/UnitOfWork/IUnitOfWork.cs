using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.DL.Service.UnitOfWork
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        #region Property
        DbConnection Connection { get; }
        DbTransaction Transaction { get; }
        #endregion

        #region Methods

        /// <summary>
        /// mở transaction
        /// </summary>
        /// created by: vdtien (22/10/2023)
        void BeginTransaction();

        /// <summary>
        /// mở transaction async
        /// </summary>
        /// <returns></returns>
        /// created by: vdtien (22/10/2023)
        Task BeginTransactionAsync();

        /// <summary>
        /// commit transaction
        /// </summary>
        /// created by: vdtien (22/10/2023)
        void Commit();

        /// <summary>
        /// commit transaction async
        /// </summary>
        /// <returns></returns>
        /// created by: vdtien (22/10/2023)
        Task CommitAsync();

        /// <summary>
        /// rollback transaction
        /// </summary>
        /// created by: vdtien (22/10/2023)
        void Rollback();

        /// <summary>
        /// rollback transaction async
        /// </summary>
        /// <returns></returns>
        /// created by: vdtien (22/10/2023)
        Task RollbackAsync();

        #endregion


    }
}
