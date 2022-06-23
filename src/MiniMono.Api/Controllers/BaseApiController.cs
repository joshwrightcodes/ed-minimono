// <copyright file="BaseApiController.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Api.Controllers;

using System.Net.Mime;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Base Api Controller.
/// <para>
/// Handle shared dependencies such as Mediator.
/// </para>
/// </summary>
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("[controller]")]
public abstract class BaseApiController : ControllerBase
{
	private ISender _mediator;
	private IMapper _mapper;

	/// <summary>
	/// Gets the Mediator Service.
	/// </summary>
	protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

	/// <summary>
	/// Gets the Mapper Service.
	/// </summary>
	protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
}