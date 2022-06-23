// <copyright file="ValidationException.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common.Exceptions;

using FluentValidation.Results;

/// <summary>
/// Validation Exception.
/// </summary>
public class ValidationException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ValidationException"/> class.
	/// </summary>
	public ValidationException()
		: base("One or more validation failures have occurred.")
		=> Errors = new Dictionary<string, string[]>();

	/// <summary>
	/// Initializes a new instance of the <see cref="ValidationException"/> class.
	/// </summary>
	/// <param name="failures">Validation errors that caused the exception.</param>
	public ValidationException(IEnumerable<ValidationFailure> failures)
		: this()
		=> Errors = failures
			.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
			.ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

	/// <summary>
	/// Gets a collection of validation errors associated with the Exception.
	/// </summary>
	public IDictionary<string, string[]> Errors { get; }
}