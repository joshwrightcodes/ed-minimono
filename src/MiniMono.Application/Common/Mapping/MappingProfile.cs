// <copyright file="MappingProfile.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Application.Common.Mapping;

using System.Reflection;
using AutoMapper;

/// <summary>
/// AutoMapper Mapping Profile.
/// </summary>
public class MappingProfile : Profile
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MappingProfile"/> class.
	/// </summary>
	public MappingProfile() => this.ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
}