// <copyright file="NotFoundException.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common.Exceptions;

/// <summary>
/// Thrown when Entity cannot be located.
/// </summary>
public class NotFoundException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="NotFoundException"/> class.
	/// </summary>
	public NotFoundException()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NotFoundException"/> class.
	/// </summary>
	/// <param name="message"></param>
	public NotFoundException(string message)
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NotFoundException"/> class.
	/// </summary>
	/// <param name="message"></param>
	/// <param name="innerException"></param>
	public NotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NotFoundException"/> class.
	/// </summary>
	/// <param name="name"></param>
	/// <param name="key"></param>
	public NotFoundException(string name, object key)
		: base($"Entity \"{name}\" ({key}) was not found.")
	{
	}
}