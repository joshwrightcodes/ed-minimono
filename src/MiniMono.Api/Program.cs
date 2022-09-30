// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

#pragma warning disable SA1200

using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using Wright.Demo.MiniMono.Api.Common.Conventions;
using Wright.Demo.MiniMono.Api.Common.Filters;
using Wright.Demo.MiniMono.Api.Services;
using Wright.Demo.MiniMono.Application;
using Wright.Demo.MiniMono.Application.Common.Identity;
using Wright.Demo.MiniMono.Infrastructure;
using Wright.Demo.MiniMono.Infrastructure.Persistence;

#pragma warning restore SA1200

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Register Application Layer Dependencies
builder.Services
	.AddApplicationLayer()
	.AddTransient<ICurrentUser, CurrentUserService>();

// Register Infrastructure Layer Dependencies
builder.Services.AddInfrastructureLayer(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
	.AddHttpContextAccessor()
	.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

// Register Controllers
builder.Services
	.AddControllers(options =>
	{
		options.Filters.Add<ApiExceptionFilterAttribute>();
		options.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
	})
	.AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
	.AddFluentValidation();

// Register Health Checks
builder.Services
	.AddHealthChecks()
	.AddDbContextCheck<ApplicationDbContext>();

// Register RazorPages
builder.Services.AddRazorPages();

// Register Options.
builder.Services
	.Configure<ForwardedHeadersOptions>(builder.Configuration.GetSection(nameof(ForwardedHeadersOptions)))
	.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

// CORS
builder.Services.AddCors(options =>
{
	var policy = new CorsPolicy();
	builder.Configuration.Bind(nameof(CorsPolicy), policy);
	options.AddPolicy(nameof(CorsPolicy), policy);
});

// Add Api Versioning
builder.Services
	.AddApiVersioning(options =>
	{
		options.ReportApiVersions = true;
		options.ApiVersionReader = ApiVersionReader.Combine(
			new MediaTypeApiVersionReader(), // Preferred Versioning
			new QueryStringApiVersionReader()); // Add this one for the HATEOAS stuff to stop complaining
		options.AssumeDefaultVersionWhenUnspecified = true;
		options.DefaultApiVersion = new ApiVersion(1, 0);
		options.Conventions.Add(new VersionByNamespaceConvention());
	})
	.AddVersionedApiExplorer(options =>
	{
		// add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
		// note: the specified format code will format the version as "'v'major[.minor][-status]"
		options.GroupNameFormat = "'v'VVV";

		// note: this option is only necessary when versioning by url segment. the SubstitutionFormat
		// can also be used to control the format of the API version in route templates
		options.SubstituteApiVersionInUrl = true;
	})
	.AddEndpointsApiExplorer();

// Register OpenAPI Services
builder.Services
	.AddSwaggerGen(options =>
	{
		options.SwaggerDoc("v1", builder.Configuration.GetValue<OpenApiInfo>("OpenApiInfo.V1"));
		options.SwaggerDoc("v2", builder.Configuration.GetValue<OpenApiInfo>("OpenApiInfo.V1"));
		options.AddSecurityDefinition("openId", new OpenApiSecurityScheme
		{
			Type = SecuritySchemeType.OpenIdConnect,
			OpenIdConnectUrl = new Uri("http://localhost/.well-known/openid-configuration"),
		});

		string filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
		options.IncludeXmlComments(filePath);

		options.OperationFilter<ApiVersionFilter>();

		// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters
		options.OperationFilter<AddHeaderOperationFilter>(
			"traceparent",
			"HTTP header field identifies the incoming request in a tracing system",
			false);
		options.OperationFilter<AddHeaderOperationFilter>(
			"tracestate",
			@"HTTP header to provide additional vendor-specific trace identification information across different " +
			"distributed tracing systems and is a companion header for the `traceparent` field",
			false);

		//options.OperationFilter<AddResponseHeadersFilter>(); // [SwaggerResponseHeader]
		options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization or use the generic method, e.g. c.OperationFilter<AppendAuthorizeToSummaryOperationFilter<MyCustomAttribute>>();

		// add Security information to each operation for OAuth2
		options.OperationFilter<SecurityRequirementsOperationFilter>();
		// or use the generic method, e.g. c.OperationFilter<SecurityRequirementsOperationFilter<MyCustomAttribute>>();

		// if you're using the SecurityRequirementsOperationFilter, you also need to tell Swashbuckle you're using OAuth2
		// options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
		// {
		//  Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
		//  In = ParameterLocation.Header,
		//  Name = "Authorization",
		//  Type = SecuritySchemeType.ApiKey,
		// });
	})
	.AddFluentValidationRulesToSwagger();

// Routing
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Open Telemetry
builder.Services.AddOpenTelemetryTracing(config =>
{
	config
		.AddConsoleExporter()
		// .AddZipkinExporter(zConfig =>
		// {
		// 	zConfig.Endpoint = new Uri(builder.Configuration.GetValue<string>("OpenTelemetry:Zipkin:Endpoint"));
		// })
		.AddSource(Assembly.GetExecutingAssembly().GetName().Name)
		.SetResourceBuilder(
			ResourceBuilder
				.CreateDefault()
				.AddService(serviceName: Assembly.GetExecutingAssembly().GetName().Name,
					serviceVersion: Assembly.GetExecutingAssembly().GetName().Version!.ToString()))
		.AddHttpClientInstrumentation()
		.AddAspNetCoreInstrumentation();
});

// Add Serilog
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

WebApplication app = builder.Build();

// Enable Proxying
app.UseForwardedHeaders();

// Enable Development Config
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

// Enable Health Checks
app.UseHealthChecks("/health");

// Enable OpenApi UI
app.UseStaticFiles();
app.UseSwagger(options =>
{
	options.RouteTemplate = "api-docs/{documentName}/specification.json";
});
app.UseSwaggerUI(options =>
{
	options.RoutePrefix = "api-docs";
	options.DocumentTitle = "Courses API";
	options.SwaggerEndpoint("/api-docs/v1/specification.json", "Courses API v1");
	options.SwaggerEndpoint("/api-docs/v2/specification.json", "Courses API v2");
});

// Enable CORS
app.UseCors(nameof(CorsPolicy));

// Enable Request Logging
app.UseSerilogRequestLogging();

// Enable HTTPS
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller}/{action=Index}/{id?}");

	endpoints.MapSwagger();

	endpoints.MapRazorPages();
});

app.Run();