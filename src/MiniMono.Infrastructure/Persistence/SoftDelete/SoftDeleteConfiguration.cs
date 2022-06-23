// <copyright file="SoftDeleteConfiguration.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Infrastructure.Persistence.SoftDelete;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Base configuration for support of soft-delete functionality.
/// </summary>
/// <typeparam name="TEntity">The entity type to be configured.</typeparam>
public abstract class SoftDeleteConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
	where TEntity : class
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		if (builder is null)
		{
			throw new ArgumentNullException(nameof(builder));
		}

		builder.Property<string>(SoftDeleteShadowProperties.DeletedBy);

		builder.Property<DateTimeOffset?>(SoftDeleteShadowProperties.Deleted);

		builder.HasQueryFilter(e => !EF.Property<DateTimeOffset?>(e, SoftDeleteShadowProperties.Deleted).HasValue);
	}
}