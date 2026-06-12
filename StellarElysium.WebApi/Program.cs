using Microsoft.AspNetCore.Mvc;
using StellarElysium.Application.Interfaces.Localization;
using StellarElysium.Domain.Dtos.Shared;
using StellarElysium.Domain.Enums.Localization;
using StellarElysium.Domain.Localization;
using StellarElysium.Infrastructure;
using StellarElysium.WebApi.Localization;
using StellarElysium.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRequestLanguageProvider, RequestLanguageProvider>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var localizer = context.HttpContext.RequestServices.GetRequiredService<IApiMessageLocalizer>();
        var languageProvider = context.HttpContext.RequestServices.GetRequiredService<IRequestLanguageProvider>();
        var language = languageProvider.GetCurrentLanguage();
        var invalidFieldMessage = localizer.Get(ApiMessageKey.InvalidField, language);

        var errors = context.ModelState
            .Where(item => item.Value?.Errors.Count > 0)
            .ToDictionary(
                item => item.Key,
                item => item.Value!.Errors.Select(_ => invalidFieldMessage).ToArray());

        return new BadRequestObjectResult(new ApiErrorResponse
        {
            Code = ApiMessageKey.InvalidRequest.ToString(),
            Message = localizer.Get(ApiMessageKey.InvalidRequest, language),
            Language = language.ToCultureCode(),
            Errors = errors
        });
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.Logger.LogInformation("API started. Environment: {Environment}", app.Environment.EnvironmentName);

app.UseMiddleware<ApiExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
