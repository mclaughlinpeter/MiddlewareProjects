using Microsoft.AspNetCore.Builder;

namespace ExtendedMiddleware
{
    public static class NumberCheckerMiddlewareExtensions
    {
        public static IApplicationBuilder UseNumberChecker(this IApplicationBuilder app)
        {
            return app.UseMiddleware<NumberCheckerMiddleware>();
        }
    }
}