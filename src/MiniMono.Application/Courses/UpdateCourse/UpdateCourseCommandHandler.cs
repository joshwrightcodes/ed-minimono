// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCourseCommandHandler.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Wright.Demo.MiniMono.Application.Courses.UpdateCourse;

using AutoMapper;
using FluentValidation;
using MediatR;
using Wright.Demo.MiniMono.Application.Common;
using Wright.Demo.MiniMono.Application.Common.Exceptions;
using Wright.Demo.MiniMono.Domain.Courses;

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Unit>
{
	private readonly IApplicationDbContext context;
	private readonly IMapper mapper;
	private readonly IEnumerable<IValidator<UpdateCourseDto>> validators;

	/// <summary>
	/// Initializes a new instance of the <see cref="UpdateCourseCommandHandler"/> class.
	/// </summary>
	/// <param name="context">Application Db Context.</param>
	/// <param name="mapper">AutoMapper Service.</param>
	/// <param name="validators">Validators for <see cref="UpdateCourseDto"/>.</param>
	public UpdateCourseCommandHandler(
		IApplicationDbContext context,
		IMapper mapper,
		IEnumerable<IValidator<UpdateCourseDto>> validators)
	{
		this.context = context ?? throw new ArgumentNullException(nameof(context));
		this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
	}

	public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
	{
		Course? course = await context.Courses
			.FindAsync(new object?[] { request.Id }, cancellationToken)
			.ConfigureAwait(false);

		if (course is null)
		{
			throw new NotFoundException(nameof(course), request.Id);
		}

		UpdateCourseDto updated = await request.Patch
			.PatchEntityAsync(
				course,
				mapper,
				validators,
				request.JsonOptions,
				cancellationToken)
			.ConfigureAwait(false);

		course.Description = updated.Description;
		course.Title = updated.Title;
		course.Thumbnail = updated.Thumbnail;

		await context
			.SaveChangesAsync(cancellationToken)
			.ConfigureAwait(false);

		return Unit.Value;
	}
}