// <copyright file="CoursesController.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Wright.Demo.MiniMono.Application.Courses.CreateCourse;
using Wright.Demo.MiniMono.Application.Courses.GetCourse;

namespace Wright.Demo.MiniMono.Api.Controllers;

using Wright.Demo.MiniMono.Application.Common.Pagination;
using Wright.Demo.MiniMono.Application.Courses.GetCourses;

public class CoursesController : BaseApiController
{
	[HttpGet]
	[ProducesResponseType(typeof(PaginatedResponse<CourseSummaryDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
	public async Task<ActionResult<PaginatedResponse<CourseSummaryDto>>> GetCourses(
		[FromQuery] GetCoursesQuery query,
		CancellationToken cancellationToken)
		=> await Mediator.Send(query, cancellationToken);

	[HttpGet("{courseId:guid}")]
	[ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<CourseDto>> GetCourse(
		Guid courseId,
		CancellationToken cancellationToken)
		=> await Mediator.Send(new Application.Courses.GetCourse.GetCourseQuery { Id = courseId }, cancellationToken);

	[HttpPost]
	public async Task<ActionResult<Guid>> CreateCourse(CreateCourseCommand command)
		=> await Mediator.Send(command);
}