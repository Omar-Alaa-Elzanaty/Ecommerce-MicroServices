using eCommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.SharedLibrary.Middleware
{
    public class GlobalExceptionHandler(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string message = "sorry, internal server error occurred. kindly try again.";
            int statusCode = StatusCodes.Status500InternalServerError;
            string title = "Error";

            try
            {
                await next(context);

                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many requests. Please try again later.";
                    statusCode = StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, message, statusCode);
                }

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "Unauthorized access. Please login to continue.";
                    await ModifyHeader(context, title, message, statusCode);
                }

                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Access";
                    message = "Forbidden access. You do not have permission to access this resource.";
                    statusCode = StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of time";
                    message = "Request timeout... try again";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }

                await ModifyHeader(context, title, message, statusCode);
            }
        }

        private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            await context.Response.WriteAsJsonAsync(new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = title
            }, CancellationToken.None);
        }
    }
}
