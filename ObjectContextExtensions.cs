using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace Providers
{
    public static class ObjectContextExtensions
    {
        public static void DoTransaction(this ObjectContext context, Action<DbTransaction> action)
        {
            DoTransaction(context, false, action);
        }

        public static void DoTransaction(this ObjectContext context, bool manualCommit, Action<DbTransaction> action)
        {
            context.Connection.Open();
            try
            {
                using (var tx = context.Connection.BeginTransaction())
                {
                    try
                    {
                        action(tx);
                        if (!manualCommit)
                            tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                context.Connection.Close();
            }
        }
    }
}