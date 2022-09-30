// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCourseCommand.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Wright.Demo.MiniMono.Application.Courses.UpdateCourse;

using System.Text.Json;
using Json.Patch;
using MediatR;
using Wright.Demo.MiniMono.Domain.Courses;

public record UpdateCourseCommand : IRequest<Unit>
{
	/// <summary>
	/// Gets the id of the <see cref="Course"/> to update.
	/// </summary>
	public Guid Id { get; init; }

	/// <summary>
	/// Gets patch operations to be applied to the <see cref="Course"/>.
	/// </summary>
	public JsonPatch Patch { get; init; }

	/// <summary>
	/// Gets serializer options from the MVC formatter.
	/// </summary>
	public JsonSerializerOptions JsonOptions { get; init; }
}