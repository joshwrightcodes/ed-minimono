// <copyright file="Course.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

using Wright.Demo.MiniMono.Domain.Common.AuditableEntities;

namespace Wright.Demo.MiniMono.Domain.Courses;

using Wright.Demo.MiniMono.Domain.Common;
using Wright.Demo.MiniMono.Domain.Lessons;

/// <summary>
/// A course that a user can take.
/// </summary>
public record Course(string Name) : BaseAuditableEntity
{
	/// <summary>
	/// Gets or sets the unique identifier of the <see cref="Course"/>.
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the <see cref="Course"/>.
	/// </summary>
	public string Name { get; set; } = Name;

	/// <summary>
	/// Gets or sets the current status of the <see cref="Course"/>.
	/// </summary>
	public CourseStatus Status { get; set; } = CourseStatus.Draft;

	/// <summary>
	/// Gets a collection of <see cref="Lesson">lessons</see> that are included in this course.
	/// </summary>
	public ICollection<Lesson> Lessons { get; } = new HashSet<Lesson>();
}