// <copyright file="JsonPatchExtensions.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common.JsonPatch;

using System.Text.Json;
using System.Text.RegularExpressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Json.Patch;
using Wright.Demo.MiniMono.Application.Common.Mapping;

/// <summary>
/// Extension methods for working with JsonPatch using JsonPatch.Net library.
/// </summary>
public static class JsonPatchExtensions
{
	private const string ErrorExpression = @"^(.*)\sOperation:\s(.*)$";

	/// <summary>
	/// Generates an updated model for a given entity and validates the patch
	/// is valid.
	/// </summary>
	/// <typeparam name="TEntity">
	/// The underlying type of the entity that is being updated.
	/// </typeparam>
	/// <typeparam name="TUpdateModel">
	/// The update model associated with <typeparamref name="TEntity"/>. This model must implement <see cref="IMapFrom{T}"/>.
	/// </typeparam>
	/// <param name="patch">
	/// The JsonPatch instructions to apply to <typeparamref name="TUpdateModel"/>.
	/// </param>
	/// <param name="entity">
	/// The entity that the patch is to be applied to.</param>
	/// <param name="mapper">
	/// The AutoMapper service for handling the mapping between <typeparamref name="TEntity"/>
	/// and <typeparamref name="TUpdateModel"/>.
	/// </param>
	/// <param name="validators">
	/// A collection of validators for <typeparamref name="TUpdateModel"/>, where applicable.
	/// </param>
	/// <param name="jsonSerializerOptions">
	/// The settings to apply to the JsonSerializer. This should align to the Json Formatter settings.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None">None</see>.
	/// </param>
	/// <returns>
	/// Object containing the updated values to apply to <typeparamref name="TEntity"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	/// Thrown when <paramref name="entity"/>, <paramref name="mapper"/>, or <paramref name="jsonSerializerOptions"/> are null.
	/// </exception>
	/// <exception cref="Exceptions.ValidationException">
	/// Thrown when either the JsonPatch operation encounters an invalid operation, or when the specified entity contains an invalid value
	/// or violates any business rules. Refer to <see cref="ValidationException.Errors"/> for details
	/// about the specific validation errors.
	/// </exception>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the JsonPatch operation encounters an invalid operation but cannot be handled as a <see cref="Exceptions.ValidationException"/>.
	/// </exception>
	public static async Task<TUpdateModel> PatchEntityAsync<TEntity, TUpdateModel>(
		this JsonPatch patch,
		TEntity entity,
		IMapper mapper,
		IEnumerable<IValidator<TUpdateModel>> validators = null,
		JsonSerializerOptions jsonSerializerOptions = default,
		CancellationToken cancellationToken = default)
		where TEntity : class
		where TUpdateModel : IMapFrom<TEntity>
	{
		ArgumentNullException.ThrowIfNull(entity);
		ArgumentNullException.ThrowIfNull(mapper);
		ArgumentNullException.ThrowIfNull(jsonSerializerOptions);

		TUpdateModel original = mapper.Map<TUpdateModel>(entity);
		TUpdateModel updated;

		// Validate Patch
		try
		{
			updated = patch.Apply<TUpdateModel, TUpdateModel>(original, jsonSerializerOptions);
		}
		catch (InvalidOperationException ex)
		{
			Match match = Regex.Match(ex.Message, ErrorExpression);

			if (!match.Success) throw;

			throw new Exceptions.ValidationException(new[]
			{
				new ValidationFailure("Operation " + match.Groups[2].Value, match.Groups[1].Value),
			});
		}

		// Validate Entity.
		await validators.ValidateAsync(updated, cancellationToken).ConfigureAwait(false);

		return updated;
	}
}