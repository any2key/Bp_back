using Bp_back.Context;
using Microsoft.EntityFrameworkCore;

namespace Bp_back
{
    public class BpEx:AppDbContext
    {
        public BpEx() : base()
        {

        }
        internal static void Run(Action<BpEx> dbAction)
        {
            Run(db =>
            {
                dbAction(db);
                db.Database.CloseConnection();
                return true;
            });
        }

        internal static TResult Run<TResult>(Func<BpEx, TResult> dbFunction)
        {
            using (var db = new BpEx())
            {
                try
                {
                    return dbFunction(db);
                }
                catch (AggregateException)
                {
                    throw;
                }
            }
        }

        public override void Dispose()
        {
            Database.CloseConnection();
            base.Dispose();
        }
    }
}
