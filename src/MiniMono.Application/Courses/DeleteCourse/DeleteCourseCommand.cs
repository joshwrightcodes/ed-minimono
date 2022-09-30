// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteCourseCommand.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Wright.Demo.MiniMono.Application.Courses.DeleteCourse;

using MediatR;

public class DeleteCourseCommand : IRequest<Unit>
{
	public Guid Id { get; set; }
}