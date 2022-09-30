// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiExplorerGroupPerVersionConvention.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Wright.Demo.MiniMono.Api.Common.Conventions;

using Microsoft.AspNetCore.Mvc.ApplicationModels;

/// <summary>
/// Determines API Version Grouping by Namespace Convention. For example: <c>SomeApi.Controllers.V1</c>.
/// </summary>
public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
{
	/// <inheritdoc />
	public void Apply(ControllerModel controller)
		=> controller.ApiExplorer.GroupName = controller.ControllerType.Namespace!
			.Split('.')
			.Last()
			.ToLowerInvariant();
}