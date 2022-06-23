// <copyright file="PaginatedResponse.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common.Pagination;

using System.Text.Json.Serialization;

/// <summary>
/// Represents a paginated set of results.
/// </summary>
/// <typeparam name="T">
/// Data type available in <see cref="Items"/>.
/// </typeparam>
public class PaginatedResponse<T>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PaginatedResponse{T}"/> class.
	/// </summary>
	/// <param name="items">Collection of items to add to results page.</param>
	/// <param name="totalCount">Total count of items in results.</param>
	/// <param name="pageIndex">
	/// Position of page in the complete set of results based on
	/// <paramref name="pageSize"/>.
	/// </param>
	/// <param name="pageSize">Maximum number of items in a page of results.</param>
	public PaginatedResponse(List<T> items, int totalCount, int pageIndex, int pageSize)
	{
		PageNumber = pageIndex;
		PageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
		TotalCount = totalCount;
		Items = items;
		PageSize = pageSize;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="PaginatedResponse{T}"/> class.
	/// Constructor for deserialization.
	/// </summary>
	[JsonConstructor]
	private PaginatedResponse()
	{
	}

	[JsonInclude]
	public int PageSize { get; set; }

	[JsonInclude]
	public List<T> Items { get; private set; } = new List<T>();

	[JsonInclude]
	public int PageCount { get; private set; }

	/// <summary>
	/// Gets the index of the current page in the overall set of results.
	/// </summary>
	[JsonInclude]
	public int PageNumber { get; private set; }

	/// <summary>
	/// Gets the total number of records in a set of results.
	/// </summary>
	[JsonInclude]
	public int TotalCount { get; private set; }

	/// <summary>
	/// Gets a value indicating whether there is a previous set of results.
	/// </summary>
	/// <value>
	/// Returns <c>true</c> if there are more than one page of results and the user
	/// is not on the first page, otherwise <c>false</c> is returned.
	/// </value>
	public bool HasPreviousPage => PageNumber > 1;

	/// <summary>
	/// Gets a value indicating whether there is another set of results.
	/// </summary>
	/// <value>
	/// Returns <c>true</c> if there are more than one page of results and the user
	/// is not on the last page, otherwise <c>false</c> is returned.
	/// </value>
	public bool HasNextPage => PageNumber < PageCount;
}