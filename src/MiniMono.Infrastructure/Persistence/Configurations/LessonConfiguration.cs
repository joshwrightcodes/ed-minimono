using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wright.Demo.MiniMono.Domain.Courses;
using Wright.Demo.MiniMono.Domain.Lessons;

namespace Wright.Demo.MiniMono.Infrastructure.Persistence.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
	public void Configure(EntityTypeBuilder<Lesson> builder)
	{
		ArgumentNullException.ThrowIfNull(builder);

		builder.Property(p => p.Name)
			.HasMaxLength(200)
			.IsRequired();

		builder.HasMany(p => p.Courses)
			.WithMany(d => d.Lessons);
	}
}