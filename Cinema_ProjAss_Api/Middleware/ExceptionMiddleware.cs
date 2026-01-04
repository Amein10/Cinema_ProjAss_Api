using System.Net;
using Cinema_ProjAss_Application.Exceptions;

namespace Cinema_ProjAss_Api.Middleware
{
    /// <summary>
    /// Global middleware der oversætter Application exceptions til korrekte HTTP status codes.
    /// - ValidationException -> 400 BadRequest
    /// - NotFoundException   -> 404 NotFound
    /// - Alt andet           -> 500 InternalServerError
    /// </summary>
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = ex.Message
                });
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // I production ville man normalt ikke returnere "detail".
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Unexpected error",
                    detail = ex.Message
                });
            }
        }
    }
}
