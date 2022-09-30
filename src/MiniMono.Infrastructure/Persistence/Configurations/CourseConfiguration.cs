using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wright.Demo.MiniMono.Domain.Courses;

namespace Wright.Demo.MiniMono.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
	public void Configure(EntityTypeBuilder<Course> builder)
	{
		ArgumentNullException.ThrowIfNull(builder);

		builder.Property(p => p.Title)
			.HasMaxLength(200)
			.IsRequired();
	}
}