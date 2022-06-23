using MediatR;

namespace Wright.Demo.MiniMono.Application.Courses.CreateCourse;

public class CreateCourseCommand : IRequest<Guid>
{
	public string Name { get; set; }
}