// <copyright file="ApplicationDbContext.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

using Wright.Demo.MiniMono.Domain.Courses;
using Wright.Demo.MiniMono.Domain.Lessons;

namespace Wright.Demo.MiniMono.Infrastructure.Persistence;

using System.Reflection;
using Duende.IdentityServer.EntityFramework.Options;
using MediatR;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Application.Common;
using Identity;
using AuditableEntities;
using Services.DomainEvents;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
{
	private readonly IMediator _mediator;
	private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

	public ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options,
		IOptions<OperationalStoreOptions> operationalStoreOptions,
		IMediator mediator,
		AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
		: base(options, operationalStoreOptions)
	{
		_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		_auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor
			?? throw new ArgumentNullException(nameof(auditableEntitySaveChangesInterceptor));
	}

	/// <inheritdoc />
	public DbSet<Course> Courses => Set<Course>();

	/// <inheritdoc />
	public DbSet<Lesson> Lessons => Set<Lesson>();

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		await _mediator.DispatchDomainEvents(this);

		return await base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		ArgumentNullException.ThrowIfNull(builder);

		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		base.OnModelCreating(builder);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
}