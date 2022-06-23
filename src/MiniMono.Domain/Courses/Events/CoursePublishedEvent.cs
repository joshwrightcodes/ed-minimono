namespace Wright.Demo.MiniMono.Domain.Courses.Events;

public class CoursePublishedEvent
{
	public CoursePublishedEvent(Course course) => Course = course;

	public Course Course { get; }
}