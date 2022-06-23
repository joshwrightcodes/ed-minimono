using Serilog;
using Wright.Demo.MiniMono.Api.Common;
using Wright.Demo.MiniMono.Api.Filters;
using Wright.Demo.MiniMono.Api.Services;
using Wright.Demo.MiniMono.Application;
using Wright.Demo.MiniMono.Application.Common.Identity;
using Wright.Demo.MiniMono.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ICurrentUser, CurrentUserService>();
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseExceptionHandler(err => err.UseCustomErrors(app.Environment));
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();