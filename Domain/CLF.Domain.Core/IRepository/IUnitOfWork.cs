using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Domain.Core.IRepository
{
    public interface IUnitOfWork
    {
        DbContext Context { get; }
        /// <summary>  
        ///     获取 当前单元操作是否已被提交  
        /// </summary>  
        bool IsCommitted { get; }
        /// <summary>  
        ///     提交当前单元操作的结果  
        /// </summary>  
        /// <returns></returns>  
        int Commit();
        /// <summary>  
        ///     把当前单元操作回滚成未提交状态  
        /// </summary>  
        void Rollback();
    }
}
