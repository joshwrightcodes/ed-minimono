// <copyright file="SoftDeleteShadowProperties.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Infrastructure.Persistence.SoftDelete;

/// <summary>
/// Shadow Properties to use when using Soft Delete functionality.
/// </summary>
public static class SoftDeleteShadowProperties
{
	/// <summary>
	/// Gets the name of the shadow property that contains when
	/// the record has been deleted, if it hasn't been soft deleted
	/// the value in the shadow property will be <c>null</c>.
	/// </summary>
	public const string Deleted = nameof(Deleted);

	/// <summary>
	/// Gets the name of the shadow property that contains the user id
	/// of the user who deleted the record.
	/// </summary>
	public const string DeletedBy = nameof(DeletedBy);
}