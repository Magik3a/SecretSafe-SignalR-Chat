using Data;
using System.Web.Mvc;

namespace SecretSafe.Controllers
{
    public class BaseController<TContext> : Controller
        where TContext : SecretSafeDbContext
    {
        protected readonly TContext dbContext;

        public BaseController(TContext dbContext)
        {
            this.dbContext = dbContext;
        }


        protected override void OnException(ExceptionContext filterContext)
        {
            // TODO: Add exception handling with some kind of logger

            base.OnException(filterContext);
        }
    }
}