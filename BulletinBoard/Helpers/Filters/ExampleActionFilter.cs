using Microsoft.AspNetCore.Mvc.Filters;

namespace BulletinBoard.Helpers.Filters
{
    public class ExampleActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // do something before the action executes
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}
