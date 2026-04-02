using FluentValidation;

namespace DevelopmentTask.Api.Filters
{
    public sealed class ValidationFilter<TModel> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<TModel>>();

            if (validator is null)
            {
                return await next(context);
            }

            var model = context.Arguments.OfType<TModel>().First();
            var result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage });

                return Results.ValidationProblem(
                    errors.ToDictionary(
                        x => x.PropertyName,
                        x => new[] { x.ErrorMessage }
                    )
                );
            }

            return await next(context);
        }
    }
}
