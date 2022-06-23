// <copyright file="PaginatedQuery.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common.Pagination;

/// <summary>
/// Base class for Queries that are paginated.
/// </summary>
public abstract class PaginatedQuery
{
	/// <summary>
	/// Gets or sets a value indicating how many records to return in an individual
	/// request, when returning a collection.
	/// </summary>
	public virtual int PageSize { get; set; } = 25;

	/// <summary>
	/// Gets or sets a value indicating where in the collection of records the current
	/// page of results in, relative to the <see cref="PageSize">page size</see>.
	/// </summary>
	public virtual int PageNumber { get; set; } = 1;
}