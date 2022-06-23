using Wright.Demo.MiniMono.Domain.Common.DomainEvents;

namespace Wright.Demo.MiniMono.Domain.Courses.Events;

public class CourseCreatedEvent : DomainEvent
{
	public CourseCreatedEvent(Course course) => Course = course;

	public Course Course { get; }
}