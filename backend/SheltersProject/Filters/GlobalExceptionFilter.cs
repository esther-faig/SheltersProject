using BLL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SheltersProject.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;

            // 1. בחירת קוד סטטוס לפי סוג החריגה (הראשונה שמתאימה נבחרת)
            int status = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                BusinessRuleException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            // 2. רישום ללוג עם כל הפרטים (שגיאה צפויה = Warning, תקלה אמיתית = Error)
            _logger.Log(
                status >= 500 ? LogLevel.Error : LogLevel.Warning,
                ex, "Exception at {Path}", context.HttpContext.Request.Path);

            // 3. בשגיאה לא צפויה לא חושפים פרטים פנימיים ללקוח
            string message = status == 500
                ? "אירעה שגיאה לא צפויה. נא לנסות שוב מאוחר יותר."
                : ex.Message;

            // 4. תשובה במבנה אחיד (ProblemDetails), כמו שגיאות הוולידציה
            context.Result = new ObjectResult(new ProblemDetails
            {
                Status = status,
                Title = message
            })
            {
                StatusCode = status
            };

            // 5. סימון שהשגיאה טופלה
            context.ExceptionHandled = true;
        }
    }
}