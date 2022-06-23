// <copyright file="IdentityResultExtensions.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using Wright.Demo.MiniMono.Application.Common.Identity;

public static class IdentityResultExtensions
{
	public static Result ToApplicationResult(this IdentityResult result)
		=> result.Succeeded
			? Result.Success()
			: Result.Failure(result.Errors.Select(e => e.Description));
}