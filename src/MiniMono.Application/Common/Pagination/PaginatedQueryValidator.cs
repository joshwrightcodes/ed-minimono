// <copyright file="PaginatedQueryValidator.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common.Pagination;

using FluentValidation;

/// <summary>
/// Validator for implementations of <see cref="PaginatedQuery"/>.
/// </summary>
/// <typeparam name="T">Type the page is of.</typeparam>
public class PaginatedQueryValidator<T> : AbstractValidator<T>
	where T : PaginatedQuery
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PaginatedQueryValidator{T}"/> class.
	/// </summary>
	public PaginatedQueryValidator()
	{
		RuleFor(p => p.PageSize).InclusiveBetween(1, 100);

		RuleFor(p => p.PageNumber).GreaterThanOrEqualTo(1);
	}
}