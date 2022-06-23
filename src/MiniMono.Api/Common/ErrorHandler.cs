// <copyright file="ErrorHandler.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

using Wright.Demo.MiniMono.Application.Common.Exceptions;

namespace Wright.Demo.MiniMono.Api.Common;

using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public static class ErrorHandler
{
	public static void UseCustomErrors(this IApplicationBuilder app, IHostEnvironment environment)
	{
		if (environment.IsDevelopment())
		{
			app.Use(WriteDevelopmentResponse);
		}
		else
		{
			app.Use(WriteProductionResponse);
		}
	}

	private static Task WriteDevelopmentResponse(HttpContext httpContext, Func<Task> next)
		=> WriteResponse(httpContext, includeDetails: true);

	private static Task WriteProductionResponse(HttpContext httpContext, Func<Task> next)
		=> WriteResponse(httpContext, includeDetails: false);

	private static async Task WriteResponse(HttpContext httpContext, bool includeDetails)
	{
		// Try and retrieve the error from the ExceptionHandler middleware
		IExceptionHandlerFeature? exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
		Exception? ex = exceptionDetails?.Error;

		if (ex is not null)
		{
			// ProblemDetails has it's own content type
			httpContext.Response.ContentType = "application/problem+json";

			ProblemDetails problem = ex switch
			{
				ValidationException vex => HandleValidationException(vex, includeDetails),
				NotFoundException nfex => HandleNotFoundException(nfex, includeDetails),
				_ => HandleUnknownException(ex, includeDetails),
			};

			string? traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

			if (traceId is not null)
			{
				problem.Extensions["TraceId"] = traceId;
			}

			if (problem is ValidationProblemDetails details)
			{
				await JsonSerializer.SerializeAsync(httpContext.Response.Body, details);
			}
			else
			{
				await JsonSerializer.SerializeAsync(httpContext.Response.Body, problem);
			}
		}
	}

	private static ProblemDetails HandleUnknownException(Exception exception, bool includeDetails = false)
	{
		ArgumentNullException.ThrowIfNull(exception);

		return new ProblemDetails
		{
			Status = StatusCodes.Status500InternalServerError,
			Title = "An error occurred while processing your request.",
			Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
			Detail = includeDetails ? exception.Message : string.Empty,
		};
	}

	private static ValidationProblemDetails HandleValidationException(ValidationException exception, bool includeDetails = false)
	{
		ArgumentNullException.ThrowIfNull(exception);

		return new ValidationProblemDetails(exception.Errors)
		{
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
			Detail = includeDetails ? exception.Message : string.Empty,
		};
	}

	private static ProblemDetails HandleNotFoundException(NotFoundException exception, bool includeDetails = false)
	{
		ArgumentNullException.ThrowIfNull(exception);

		return new ProblemDetails
		{
			Status = StatusCodes.Status404NotFound,
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
			Title = "The specified resource was not found.",
			Detail = includeDetails ? exception.Message : string.Empty,
		};
	}
}