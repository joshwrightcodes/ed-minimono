using AutoMapper;
using MediatR;
using Wright.Demo.MiniMono.Application.Common;
using Wright.Demo.MiniMono.Application.Common.Exceptions;
using Wright.Demo.MiniMono.Domain.Courses;

namespace Wright.Demo.MiniMono.Application.Courses.GetCourse;

public class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, CourseDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetCourseQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	public async Task<CourseDto> Handle(GetCourseQuery request, CancellationToken cancellationToken)
	{
		Course? course = await _context.Courses
			.FindAsync(new object?[] { request.Id }, cancellationToken)
			.ConfigureAwait(false);

		if (course is null)
		{
			throw new NotFoundException(nameof(course), request.Id);
		}

		return _mapper.Map<CourseDto>(course);
	}
}