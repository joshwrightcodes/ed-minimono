// <copyright file="CourseStatus.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Domain.Courses;

/// <summary>
/// Statuses of a <see cref="Course"/>.
/// </summary>
public enum CourseStatus
{
	/// <summary>
	/// Course is in a draft state and can only be viewed by users with publishing permissions.
	/// </summary>
	Draft,

	/// <summary>
	/// Course is in a published state and can be viewed by all authenticated users.
	/// </summary>
	Published,

	/// <summary>
	/// Course is in an archived state and can only be viewed by administrators.
	/// </summary>
	Archived,
}