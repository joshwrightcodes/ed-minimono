// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CoursesController.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Wright.Demo.MiniMono.Api.Controllers.V2;

using Microsoft.AspNetCore.Mvc;
using Wright.Demo.MiniMono.Application.Common.Pagination;
using Wright.Demo.MiniMono.Application.Courses.GetCourses;

public partial class CoursesController : BaseApiController
{
	/// <summary>
	/// Retrieves a collection of courses.
	/// </summary>
	/// <param name="query">Query String parameters.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A paginated collection of courses.</returns>
	[HttpGet(Name = Routes.GetCourses)]
	[ProducesResponseType(typeof(PaginatedResponse<CourseSummaryDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
	public async Task<ActionResult<PaginatedResponse<CourseSummaryDto>>> GetCourses(
		[FromQuery] GetCoursesQuery query,
		CancellationToken cancellationToken)
		=> await Mediator.Send(query, cancellationToken);

	public static partial class Routes
	{
		public const string GetCourse = nameof(GetCourse);
		public const string GetCourses = nameof(GetCourses);
		public const string CreateCourse = nameof(CreateCourse);
		public const string UpdateCourse = nameof(UpdateCourse);
		public const string DeleteCourse = nameof(DeleteCourse);
	}
}