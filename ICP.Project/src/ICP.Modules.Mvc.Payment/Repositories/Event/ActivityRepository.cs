using ICP.Infrastructure.Abstractions.DbUtil;

namespace ICP.Modules.Mvc.Payment.Repositories.Event
{
    public class ActivityRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private IDbConnection db;

        public ActivityRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

    }
}
