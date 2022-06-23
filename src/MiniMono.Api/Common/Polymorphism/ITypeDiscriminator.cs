// <copyright file="ITypeDiscriminator.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Api.Common.Polymorphism;

/// <summary>
/// Defines contract for entities needing polymorphic serialization with <c>System.Text.Json</c>.
/// </summary>
/// <typeparam name="T">Enum that contains the type discriminator values.</typeparam>
public interface ITypeDiscriminator<out T>
	where T : struct, Enum
{
	/// <summary>
	/// Gets the type discriminator for polymorphic object serialization.
	/// </summary>
	T Type { get; }
}