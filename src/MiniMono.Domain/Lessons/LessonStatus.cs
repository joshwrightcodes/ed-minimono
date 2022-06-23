// <copyright file="LessonStatus.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Domain.Lessons;

/// <summary>
/// Statuses for a <see cref="Lesson"/>.
/// </summary>
public enum LessonStatus
{
	/// <summary>
	/// Lesson is in a draft state and can only be viewed by users with publishing permissions.
	/// </summary>
	Draft,

	/// <summary>
	/// Lesson is in a published state and can be viewed by all authenticated users.
	/// </summary>
	Published,

	/// <summary>
	/// Lesson is in an archived state and can only be viewed by administrators.
	/// </summary>
	Archived,
}