// <copyright file="CurrentUserService.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

using System.Security.Claims;

namespace Wright.Demo.MiniMono.Api.Services;

using Wright.Demo.MiniMono.Application.Common.Identity;

/// <summary>
/// Defines the <see cref="CurrentUserService" />.
/// </summary>
public class CurrentUserService : ICurrentUser
{
	/// <summary>
	/// Defines the httpContextAccessor.
	/// </summary>
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="CurrentUserService"/> class.
	/// </summary>
	/// <param name="httpContextAccessor">The httpContextAccessor<see cref="IHttpContextAccessor"/>.</param>
	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
		=> this._httpContextAccessor = httpContextAccessor;

	/// <summary>
	/// Gets the UserId.
	/// </summary>
	public string UserId
		=> _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

	/// <summary>
	/// Gets the Username.
	/// </summary>
	public string Username
		=> _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
}