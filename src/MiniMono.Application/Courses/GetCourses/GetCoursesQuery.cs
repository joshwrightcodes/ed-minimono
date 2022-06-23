// <copyright file="GetCoursesQuery.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Courses.GetCourses;

using MediatR;
using Wright.Demo.MiniMono.Application.Common.Pagination;

public class GetCoursesQuery : PaginatedQuery, IRequest<PaginatedResponse<CourseSummaryDto>>
{
}