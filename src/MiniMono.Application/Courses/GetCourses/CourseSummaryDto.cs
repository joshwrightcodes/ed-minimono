// <copyright file="CourseSummaryDto.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Courses.GetCourses;

using Common.Mapping;
using Wright.Demo.MiniMono.Domain.Courses;

public class CourseSummaryDto : IMapFrom<Course>
{
	public Guid Id { get; set; }

	public string Name { get; set; }
}