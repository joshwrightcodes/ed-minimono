// <copyright file="FluentValidationExtensions.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common;

using FluentValidation;

/// <summary>
/// Extensions for collections of <see cref="IValidator"/>.
/// </summary>
public static class FluentValidationExtensions
{
	/// <summary>
	/// Validates entity using all of the specified validators.
	/// </summary>
	/// <typeparam name="T">Entity Type.</typeparam>
	/// <param name="validators">Validators to use.</param>
	/// <param name="entity">Entity model to validate.</param>
	/// <exception cref="Exceptions.ValidationException">
	/// <see cref="Exceptions.ValidationException"/> is thrown when the specified entity contains an invalid value
	/// or violates any business rules. Refer to <see cref="ValidationException.Errors"/> for details
	/// about the specific validation errors.
	/// </exception>
	public static void Validate<T>(this IEnumerable<IValidator<T>> validators, T entity)
	{
		if (!validators.Any())
		{
			return;
		}

		ValidationContext<T> context = new (entity);

		IEnumerable<FluentValidation.Results.ValidationFailure> failures = validators
			.Select(v => v.Validate(context))
			.SelectMany(r => r.Errors)
			.Where(f => f != null);

		if (failures.Any())
		{
			throw new Exceptions.ValidationException(failures);
		}
	}

	/// <summary>
	/// Validates entity using all of the specified validators.
	/// </summary>
	/// <typeparam name="T">Entity Type.</typeparam>
	/// <param name="validators">Validators to use.</param>
	/// <param name="entity">Entity model to validate.</param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None">None</see>.
	/// </param>
	/// <exception cref="Exceptions.ValidationException">
	/// <see cref="Exceptions.ValidationException"/> is thrown when the specified entity contains an invalid value
	/// or violates any business rules. Refer to <see cref="ValidationException.Errors"/> for details
	/// about the specific validation errors.
	/// </exception>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public static async Task ValidateAsync<T>(
		this IEnumerable<IValidator<T>> validators,
		T entity,
		CancellationToken cancellationToken = default)
	{
		if (validators.Any())
		{
			ValidationContext<T> context = new (entity);

			FluentValidation.Results.ValidationResult[] validationResults = await Task
				.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)))
				.ConfigureAwait(false);

			var failures = validationResults
				.SelectMany(r => r.Errors)
				.Where(f => f != null)
				.ToList();

			if (failures.Count != 0)
			{
				throw new Exceptions.ValidationException(failures);
			}
		}
	}
}