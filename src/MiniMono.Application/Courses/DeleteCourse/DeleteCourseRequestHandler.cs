// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteCourseRequestHandler.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Wright.Demo.MiniMono.Application.Courses.DeleteCourse;

using MediatR;
using Wright.Demo.MiniMono.Application.Common;
using Wright.Demo.MiniMono.Application.Common.Exceptions;
using Wright.Demo.MiniMono.Domain.Courses;

public class DeleteCourseRequestHandler : IRequestHandler<DeleteCourseCommand, Unit>
{
	private readonly IApplicationDbContext context;

	public DeleteCourseRequestHandler(IApplicationDbContext context)
		=> this.context = context ?? throw new ArgumentNullException(nameof(context));

	public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
	{
		Course? course = await context.Courses
			.FindAsync(new object?[] { request.Id }, cancellationToken)
			.ConfigureAwait(false);

		if (course is null)
		{
			throw new NotFoundException(nameof(course), request.Id);
		}

		context.Courses.Remove(course);
		await context.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}