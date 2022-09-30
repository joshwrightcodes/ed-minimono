// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Course.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Wright.Demo.MiniMono.Domain.Courses;

using Wright.Demo.MiniMono.Domain.Common.AuditableEntities;
using Wright.Demo.MiniMono.Domain.Lessons;

/// <summary>
/// A course that a user can take.
/// </summary>
public record Course(string Title) : BaseAuditableEntity
{
	/// <summary>
	/// Gets or sets the name of the <see cref="Course"/>.
	/// </summary>
	public string Title { get; set; } = Title;

	/// <summary>
	/// Gets or sets an optional description of the <see cref="Course"/>.
	/// </summary>
	public string? Description { get; set; }

	/// <summary>
	/// Gets or sets an optional external identifier for the course <see cref="Course"/> in a third party system.
	/// </summary>
	public string? ExternalId { get; set; }

	/// <summary>
	/// Gets or sets an optional url for a thumbnail image to represent the course.
	/// </summary>
	public Uri? Thumbnail { get; set; }

	/// <summary>
	/// Gets or sets when the course was published.
	/// </summary>
	public DateTimeOffset? PublishedOn { get; set; }

	/// <summary>
	/// Gets or sets the id of the account that owns the course.
	/// </summary>
	public Guid OwnerId { get; set; }

	/// <summary>
	/// Gets or sets the current status of the <see cref="Course"/>.
	/// </summary>
	public CourseStatus Status { get; set; } = CourseStatus.Draft;

	/// <summary>
	/// Gets a collection of <see cref="Lesson">lessons</see> that are included in this course.
	/// </summary>
	public ICollection<Lesson> Lessons { get; } = new HashSet<Lesson>();
}