// <copyright file="DateTimeOffsetService.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Infrastructure.Services;

using Wright.Demo.MiniMono.Application.Common;

/// <summary>
/// Default DateTimeOffset Service.
/// </summary>
/// <remarks>
/// Uses <see cref="DateTimeOffset.UtcNow"/> for the value returned in
/// <see cref="UtcNow"/>.
/// </remarks>
public class DateTimeOffsetService : IDateTimeOffset
{
	/// <inheritdoc/>
	public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}