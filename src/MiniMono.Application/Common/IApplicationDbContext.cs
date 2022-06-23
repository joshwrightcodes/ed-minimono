// <copyright file="IApplicationDbContext.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Domain.Courses;
using Domain.Lessons;

public interface IApplicationDbContext
{
	DbSet<Course> Courses { get; }

	DbSet<Lesson> Lessons { get; }

	DatabaseFacade Database { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}