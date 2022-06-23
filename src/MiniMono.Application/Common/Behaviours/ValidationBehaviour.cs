// <copyright file="ValidationBehaviour.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common.Behaviours;

using FluentValidation;
using MediatR;

/// <summary>
/// Automatically validates any requests passing through the MediatR pipeline with
/// any applicable FluentValidation implementations.
/// </summary>
/// <typeparam name="TRequest">Request Type to Validate.</typeparam>
/// <typeparam name="TResponse">Request Response Type.</typeparam>
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	/// <summary>
	/// Initializes a new instance of the <see cref="ValidationBehaviour{TRequest, TResponse}"/> class.
	/// </summary>
	/// <param name="validators">Request Validators for <typeparamref name="TRequest"/>.</param>
	public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
		=> this._validators = validators;

	/// <inheritdoc/>
	public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
	{
		if (_validators.Any())
		{
			ValidationContext<TRequest> context = new (request);

			FluentValidation.Results.ValidationResult[] validationResults = await Task
				.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)))
				.ConfigureAwait(false);

			List<FluentValidation.Results.ValidationFailure> failures = validationResults
				.SelectMany(r => r.Errors)
				.Where(f => f != null)
				.ToList();

			if (failures.Count != 0)
			{
				throw new Wright.Demo.MiniMono.Application.Common.Exceptions.ValidationException(failures);
			}
		}

		return await next().ConfigureAwait(false);
	}
}