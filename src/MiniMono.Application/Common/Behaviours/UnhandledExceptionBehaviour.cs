// <copyright file="UnhandledExceptionBehaviour.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common.Behaviours;

using MediatR;
using Microsoft.Extensions.Logging;

/// <summary>
/// Intercepts any unhandled exceptions in the MediatR pipeline and ensures that they are
/// appropriately logged.
/// </summary>
/// <typeparam name="TRequest">Request Type.</typeparam>
/// <typeparam name="TResponse">Request Response Type.</typeparam>
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger _logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="UnhandledExceptionBehaviour{TRequest, TResponse}"/> class.
	/// </summary>
	/// <param name="logger">Logging Provider.</param>
	public UnhandledExceptionBehaviour(ILogger<TRequest> logger) => this._logger = logger;

	/// <inheritdoc/>
	public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
	{
		try
		{
			return await next().ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			string requestName = typeof(TRequest).Name;

			_logger.LogError(ex, "Unhandled Exception for Request {Name} {@Request}", requestName, request);

			throw;
		}
	}
}