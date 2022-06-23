// <copyright file="AuditableEntityExtensions.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Infrastructure.Persistence.AuditableEntities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Wright.Demo.MiniMono.Domain.Common.AuditableEntities;

/// <summary>
/// Extension methods for handling updates to entities implementing <see cref="BaseAuditableEntity"/>.
/// </summary>
public static class AuditableEntityExtensions
{
	public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
		entry.References.Any(r =>
			r.TargetEntry?.Metadata.IsOwned() == true &&
			r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}