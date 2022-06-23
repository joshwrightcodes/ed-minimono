using MediatR;

namespace Wright.Demo.MiniMono.Application.Courses.GetCourse;

public class GetCourseQuery : IRequest<CourseDto>
{
	public Guid Id { get; set; }
}