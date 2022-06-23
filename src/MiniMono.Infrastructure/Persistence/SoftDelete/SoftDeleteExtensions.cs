// <copyright file="SoftDeleteExtensions.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Infrastructure.Persistence.SoftDelete;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Wright.Demo.MiniMono.Application.Common;
using Wright.Demo.MiniMono.Application.Common.Identity;

/// <summary>
/// Extension methods for applying soft delete logic to Entity Framework Core entities.
/// </summary>
public static class SoftDeleteExtensions
{
	/// <summary>
	/// Applies Soft Delete logic to changed entities.
	/// </summary>
	/// <param name="context">DbContext.</param>
	/// <param name="dateTimeOffset">Service for obtaining the current date/time.</param>
	/// <param name="currentUserService">Service for obtaining the current user.</param>
	/// <exception cref="ArgumentNullException">
	/// Method throws <see cref="ArgumentNullException"/> when encountering a <c>null</c> value in
	/// <paramref name="context"/>, <paramref name="currentUserService"/>, or <paramref name="dateTimeOffset"/>.
	/// </exception>
	public static void UpdateSoftDeleteStatuses(
		this DbContext context,
		IDateTimeOffset dateTimeOffset,
		ICurrentUser currentUserService)
	{
		ArgumentNullException.ThrowIfNull(context);
		ArgumentNullException.ThrowIfNull(dateTimeOffset);
		ArgumentNullException.ThrowIfNull(currentUserService);

		foreach (EntityEntry entry in context.ChangeTracker.Entries()
			.Where(e => e.Properties.Any(p => p.Metadata.Name == SoftDeleteShadowProperties.Deleted)))
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.CurrentValues[SoftDeleteShadowProperties.Deleted] = null;
					entry.CurrentValues[SoftDeleteShadowProperties.DeletedBy] = null;
					break;
				case EntityState.Deleted:
					entry.State = EntityState.Modified;
					entry.CurrentValues[SoftDeleteShadowProperties.Deleted] = dateTimeOffset.UtcNow;
					entry.CurrentValues[SoftDeleteShadowProperties.DeletedBy] = currentUserService.UserId;
					break;
			}
		}
	}
}