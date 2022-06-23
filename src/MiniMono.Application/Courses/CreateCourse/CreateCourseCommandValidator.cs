using FluentValidation;

namespace Wright.Demo.MiniMono.Application.Courses.CreateCourse;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
	public CreateCourseCommandValidator()
		=> RuleFor(p => p.Name)
			.NotEmpty()
			.NotNull()
			.MaximumLength(200);
}