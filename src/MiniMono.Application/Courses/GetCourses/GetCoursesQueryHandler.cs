// <copyright file="GetCoursesQueryHandler.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Courses.GetCourses;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common;
using Common.Pagination;

public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, PaginatedResponse<CourseSummaryDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetCoursesQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	public async Task<PaginatedResponse<CourseSummaryDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
		=> await _context.Courses
			.AsNoTracking()
			.ProjectTo<CourseSummaryDto>(_mapper.ConfigurationProvider)
			.PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken)
			.ConfigureAwait(false);
}