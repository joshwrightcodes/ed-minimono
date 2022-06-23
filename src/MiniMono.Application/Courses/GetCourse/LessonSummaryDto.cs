// <copyright file="LessonSummaryDto.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Courses.GetCourse;

using Wright.Demo.MiniMono.Application.Common.Mapping;
using Wright.Demo.MiniMono.Domain.Lessons;

public class LessonSummaryDto : IMapFrom<Lesson>
{
	public Guid Id { get; set; }

	public string Name { get; set; }
}