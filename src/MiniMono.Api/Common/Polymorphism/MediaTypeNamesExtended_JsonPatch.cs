// <copyright file="MediaTypeNamesExtended_JsonPatch.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Api.Common.Polymorphism;

/// <summary>
/// Extended Media Types.
/// </summary>
public static partial class MediaTypeNamesExtended
{
	/// <summary>
	/// Application Media Types.
	/// </summary>
	public static partial class Application
	{
		/// <summary>
		/// Media Type for JsonPatch requests.
		/// </summary>
		public const string JsonPatch = "application/json-patch+json";
	}
}