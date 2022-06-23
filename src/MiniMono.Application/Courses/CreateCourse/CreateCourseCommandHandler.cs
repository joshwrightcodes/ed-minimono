// <copyright file="CreateCourseCommandHandler.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

using Wright.Demo.MiniMono.Domain.Courses;
using Wright.Demo.MiniMono.Domain.Courses.Events;

namespace Wright.Demo.MiniMono.Application.Courses.CreateCourse;

using Common;
using MediatR;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Guid>
{
	private readonly IApplicationDbContext _context;

	public CreateCourseCommandHandler(IApplicationDbContext context)
		=> _context = context ?? throw new ArgumentNullException(nameof(context));

	public async Task<Guid> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
	{
		var entity = new Course(request.Name)
		{
			Status = CourseStatus.Draft,
		};

		entity.AddDomainEvent(new CourseCreatedEvent(entity));

		_context.Courses.Add(entity);

		await _context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}