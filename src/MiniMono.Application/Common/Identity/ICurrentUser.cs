// <copyright file="ICurrentUser.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common.Identity;

/// <summary>
/// Interface defining a service for retrieving the current user.
/// </summary>
public interface ICurrentUser
{
	/// <summary>
	/// Gets the current user id.
	/// </summary>
	string UserId { get; }
}