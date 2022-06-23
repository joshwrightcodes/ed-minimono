// <copyright file="PerformanceBehaviour.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Wright.Demo.MiniMono.Application.Common.Identity;

namespace Wright.Demo.MiniMono.Application.Common.Behaviours;

/// <summary>
/// Measures request performance as it moves through the MediatR pipeline.
/// Excessively slow requests will trigger a warning.
/// </summary>
/// <typeparam name="TRequest">Request Type.</typeparam>
/// <typeparam name="TResponse">Request Response Type.</typeparam>
public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger _logger;
	private readonly ICurrentUser _currentUserService;
	private readonly IIdentityService _identityService;

	/// <summary>
	/// Initializes a new instance of the <see cref="PerformanceBehaviour{TRequest, TResponse}"/> class.
	/// </summary>
	/// <param name="logger">Logger Instance.</param>
	/// <param name="currentUserService">Service for determining the current user.</param>
	/// <param name="identityService">Service for interacting with Identity.</param>
	public PerformanceBehaviour(
		ILogger<TRequest> logger,
		ICurrentUser currentUserService,
		IIdentityService identityService)
	{
		ArgumentNullException.ThrowIfNull(logger);
		ArgumentNullException.ThrowIfNull(currentUserService);
		ArgumentNullException.ThrowIfNull(identityService);

		this._logger = logger;
		this._currentUserService = currentUserService;
		this._identityService = identityService;
	}

	/// <inheritdoc/>
	public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
	{
		var timer = Stopwatch.StartNew();

		TResponse response = await next().ConfigureAwait(false);

		timer.Stop();

		long elapsedMilliseconds = timer.ElapsedMilliseconds;

		if (elapsedMilliseconds <= 500)
		{
			return response;
		}

		string requestName = typeof(TRequest).Name;
		string userId = _currentUserService.UserId ?? string.Empty;
		string userName = string.Empty;

		if (!string.IsNullOrEmpty(userId))
		{
			userName = await _identityService.GetUserNameAsync(userId).ConfigureAwait(false);
		}

		_logger.LogWarning(
			message: "Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
			requestName,
			elapsedMilliseconds,
			userId,
			userName,
			request);

		return response;
	}
}