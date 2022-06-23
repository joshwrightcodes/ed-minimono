// <copyright file="PaginationExtensions.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common.Pagination;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Extension methods for working with paginated data.
/// </summary>
public static class PaginationExtensions
{
	/// <summary>
	/// Asynchronously creates a set of paginated results from an resulting query.
	/// </summary>
	/// <typeparam name="T">Type of data in paginated list.</typeparam>
	/// <param name="source">Query Result source.</param>
	/// <param name="pageIndex">Position in the result.</param>
	/// <param name="pageSize">Maximum number of records in the page of results.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Paginated results.</returns>
	public static async Task<PaginatedResponse<T>> PaginatedListAsync<T>(
		this IQueryable<T> source,
		int pageIndex,
		int pageSize,
		CancellationToken cancellationToken = default)
	{
		int count = await source
			.CountAsync(cancellationToken)
			.ConfigureAwait(false);

		List<T> items = await source.Skip((pageIndex - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken)
			.ConfigureAwait(false);

		return new PaginatedResponse<T>(items, count, pageIndex, pageSize);
	}

	/// <summary>
	/// Creates a set of paginated results from an resulting list.
	/// </summary>
	/// <typeparam name="T">Type of data in paginated list.</typeparam>
	/// <param name="source">Query Result source.</param>
	/// <param name="pageIndex">Position in the result.</param>
	/// <param name="pageSize">Maximum number of records in the page of results.</param>
	/// <returns>Paginated results.</returns>
	public static PaginatedResponse<T> Create<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
	{
		IEnumerable<T> enumerable = source.ToList();

		int count = enumerable.Count();

		var items = enumerable
			.Skip((pageIndex - 1) * pageSize)
			.Take(pageSize)
			.ToList();

		return new PaginatedResponse<T>(items, count, pageIndex, pageSize);
	}
}