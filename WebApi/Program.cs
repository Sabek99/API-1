using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApi.Authorization;
using WebApi.Helpers;
using WebApi.Services;
using WebApi.Services.AnswerServices;
using WebApi.Services.QuestionServices;
using WebApi.Services.QuestionTagServices;
using WebApi.Services.RequestServices;
using WebApi.Services.TageServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ITagService, TagService>();
builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.AddTransient<IQuestionTagService, QuestionTagService>();
builder.Services.AddTransient<IAnswerService, AnswerService>();
builder.Services.AddTransient<IRequestService, RequesServices>();


void AddSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,

                },
                new List<string>()
            }
        });

        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "SuperMate API",

        });
        
    });
}
// add services to DI container
    

var services = builder.Services;

services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


services.AddEndpointsApiExplorer();
services.AddCors();
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


services.AddControllers().AddJsonOptions(options =>
{
    //Ignore cyclic reference errors
    options.JsonSerializerOptions.ReferenceHandler = 
        ReferenceHandler.IgnoreCycles;
}); 

AddSwagger(services);
services.AddSwaggerGen();
// configure automapper with all automapper profiles from this assembly
services.AddAutoMapper(typeof(Program));

// configure strongly typed settings object
services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// configure DI for application services
services.AddScoped<IJwtUtils, JwtUtils>();
services.AddScoped<IUserService, UserService>();
services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});




var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SuperMate API");
});


//app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();


// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();
    app.UseCors("Open");
    app.MapControllers();
   // app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseRouting();
    app.MapControllers();
    app.Run();
}
    