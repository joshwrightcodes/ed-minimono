// <copyright file="Lesson.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

using Wright.Demo.MiniMono.Domain.Common.AuditableEntities;

namespace Wright.Demo.MiniMono.Domain.Lessons;

using Wright.Demo.MiniMono.Domain.Common;
using Wright.Demo.MiniMono.Domain.Courses;

/// <summary>
/// A individual block of training that can be taken as part of a <see cref="Course"/> or individually.
/// </summary>
/// <param name="Name"></param>
public record Lesson(string Name) : BaseAuditableEntity
{
	/// <summary>
	/// Gets or sets the unique identifier of the <see cref="Lesson"/>.
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the <see cref="Lesson"/>.
	/// </summary>
	public string Name { get; set; } = Name;

	/// <summary>
	/// Gets or sets the current status of the <see cref="Lesson"/>.
	/// </summary>
	public LessonStatus Status { get; set; }

	/// <summary>
	/// Gets a collection of <see cref="Course">courses</see> that the lesson is part of.
	/// </summary>
	public ICollection<Course> Courses { get; } = new HashSet<Course>();
}