// <copyright file="ConfigureServices.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Infrastructure;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Common;
using Application.Common.Identity;
using Identity;
using Persistence;
using Services;
using Wright.Demo.MiniMono.Infrastructure.Persistence.AuditableEntities;

public static class ConfigureServices
{
	private const string InMemoryDatabaseOptionName = "UseInMemoryDatabase";
	private const string InMemoryDatabaseName = "Wright.Demo.MiniMono";
	private const string NpgsqlConnectionStringName = "DefaultConnection";

	/// <summary>
	/// Registers Infrastructure Layer dependencies.
	/// </summary>
	/// <param name="services">Service Collection to register dependencies with.</param>
	/// <param name="configuration">Application Configuration.</param>
	/// <returns>Updated Service Collection.</returns>
	public static IServiceCollection AddInfrastructureLayer(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(configuration);

		services.AddScoped<AuditableEntitySaveChangesInterceptor>();

		// Configure Application DbContext
		if (configuration.GetValue<bool>(InMemoryDatabaseOptionName))
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseInMemoryDatabase(InMemoryDatabaseName));
		}
		else
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseNpgsql(
					configuration.GetConnectionString(NpgsqlConnectionStringName),
					b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
		}

		services.AddScoped<IApplicationDbContext>(provider =>
			provider.GetService<ApplicationDbContext>() ?? throw new InvalidOperationException());

		// Add Auth Services
		services
			.AddDefaultIdentity<ApplicationUser>()
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>();

		services.AddIdentityServer()
			.AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

		services.AddAuthentication()
			.AddIdentityServerJwt();

		services.AddTransient<IIdentityService, IdentityService>();

		// Add Services
		services.AddTransient<IDateTimeOffset, DateTimeOffsetService>();

		return services;
	}
}