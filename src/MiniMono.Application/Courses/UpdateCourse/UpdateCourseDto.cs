// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCourseDto.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Wright.Demo.MiniMono.Application.Courses.UpdateCourse;

using Wright.Demo.MiniMono.Application.Common.Mapping;
using Wright.Demo.MiniMono.Domain.Courses;

public class UpdateCourseDto : IMapFrom<Course>
{
	public string Title { get; set; }

	public string Description { get; set; }

	public Uri Thumbnail { get; set; }
}