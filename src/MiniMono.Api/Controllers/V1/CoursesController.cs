// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CoursesController.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Wright.Demo.MiniMono.Api.Controllers.V1;

using Json.Patch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Wright.Demo.MiniMono.Application.Common.Pagination;
using Wright.Demo.MiniMono.Application.Courses.CreateCourse;
using Wright.Demo.MiniMono.Application.Courses.DeleteCourse;
using Wright.Demo.MiniMono.Application.Courses.GetCourse;
using Wright.Demo.MiniMono.Application.Courses.GetCourses;
using Wright.Demo.MiniMono.Application.Courses.UpdateCourse;

/// <summary>
/// Operations for working with Course entities.
/// </summary>
public partial class CoursesController : BaseApiController
{
	private readonly IOptions<JsonOptions> jsonOptions;

	/// <summary>
	/// Initializes a new instance of the <see cref="CoursesController"/> class with the specified JsonOptions
	/// injected.
	/// </summary>
	/// <param name="jsonOptions">Api Json Options passed to Patch operations.</param>
	/// <exception cref="ArgumentNullException">Thrown when parameters are null.</exception>
	public CoursesController(IOptions<JsonOptions> jsonOptions) =>
		this.jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));

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
	[Authorize(AuthorizationScopes.Read)]
	public async Task<ActionResult<PaginatedResponse<CourseSummaryDto>>> GetCourses(
		[FromQuery] GetCoursesQuery query,
		CancellationToken cancellationToken)
		=> await Mediator.Send(query, cancellationToken);

	/// <summary>
	/// Retrieves a specific course by ID.
	/// </summary>
	/// <param name="id">The id of the course to retrieve.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>Requested course.</returns>
	[HttpGet("{id:guid}", Name = Routes.GetCourse)]
	[ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<CourseDto>> GetCourse(
		Guid id,
		CancellationToken cancellationToken)
		=> await Mediator.Send(new GetCourseQuery { Id = id }, cancellationToken);

	[HttpPost(Name = Routes.CreateCourse)]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> CreateCourse(CreateCourseCommand command)
	{
		Guid id = await Mediator.Send(command);

		return CreatedAtAction(Routes.GetCourse, id);
	}

	[HttpPatch("{id:guid}", Name = Routes.UpdateCourse)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] JsonPatch patch)
	{
		await Mediator.Send(new UpdateCourseCommand
		{
			Id = id,
			Patch = patch,
			JsonOptions = jsonOptions.Value.JsonSerializerOptions,
		});

		return NoContent();
	}

	[HttpDelete("{id:guid}", Name = Routes.DeleteCourse)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteCourse(Guid id)
	{
		await Mediator.Send(new DeleteCourseCommand { Id = id });

		return NoContent();
	}

	public static partial class Routes
	{
		public const string GetCourse = nameof(GetCourse);
		public const string GetCourses = nameof(GetCourses);
		public const string CreateCourse = nameof(CreateCourse);
		public const string UpdateCourse = nameof(UpdateCourse);
		public const string DeleteCourse = nameof(DeleteCourse);
	}

	public static partial class AuthorizationScopes
	{
		public const string Read = "course:read";
		public const string Write = "course:write";
		public const string Edit = "course:edit";
		public const string Administration = "course:admin";
	}
}