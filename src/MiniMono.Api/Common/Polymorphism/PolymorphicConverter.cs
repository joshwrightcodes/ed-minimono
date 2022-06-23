// <copyright file="PolymorphicConverter.cs" company="Josh Wright">
// Copyright 2022 Josh Wright. Use of this source code is governed by an MIT-style, license that can be found in the
// LICENSE file or at https://opensource.org/licenses/MIT.
// </copyright>

namespace Wright.Demo.MiniMono.Api.Common.Polymorphism;

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wright.Demo.MiniMono.Api.Common.Polymorphism;

/// <summary>
/// Base converter for polymorphic serialization with System.Text.Json.
/// </summary>
/// <typeparam name="TTypeDiscriminator">Type that defines discriminator values.</typeparam>
/// <typeparam name="TOutput">Base class to output.</typeparam>
public abstract class PolymorphicConverter<TTypeDiscriminator, TOutput> : JsonConverter<TOutput>
	where TTypeDiscriminator : struct, Enum
	where TOutput : ITypeDiscriminator<TTypeDiscriminator>
{
	/// <summary>
	/// Gets a collection of types supported by the converter.
	/// </summary>
	public virtual Dictionary<TTypeDiscriminator, Type> Conversions { get; }

	/// <inheritdoc/>
	public override bool CanConvert(Type typeToConvert) =>
		typeof(TOutput).IsAssignableFrom(typeToConvert);

	/// <inheritdoc/>
	public override TOutput Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options)
	{
		Utf8JsonReader readerClone = reader;

		if (readerClone.TokenType != JsonTokenType.StartObject)
		{
			throw new JsonException();
		}

		readerClone.Read();

		if (readerClone.TokenType != JsonTokenType.PropertyName)
		{
			throw new JsonException();
		}

		string propertyName = readerClone.GetString();

		PropertyInfo typeProperty = typeof(TOutput).GetProperty(nameof(ITypeDiscriminator<TTypeDiscriminator>.Type));

		var jsonPropertyName =
			(JsonPropertyNameAttribute)Attribute.GetCustomAttribute(typeProperty, typeof(JsonPropertyNameAttribute));

		string name = nameof(ITypeDiscriminator<TTypeDiscriminator>.Type);

		if (jsonPropertyName is not null)
		{
			name = jsonPropertyName.Name;
		}

		if (!propertyName.Equals(name,
			    options.PropertyNameCaseInsensitive
				    ? StringComparison.InvariantCultureIgnoreCase
				    : StringComparison.InvariantCulture))
		{
			throw new JsonException($"Unable to locate type discriminator token, expecting \"{name}\"");
		}

		readerClone.Read();
		string typeVal = readerClone.GetString();

		TTypeDiscriminator typeDiscriminator;

		try
		{
			typeDiscriminator = JsonSerializer.Deserialize<TTypeDiscriminator>($"\"{typeVal}\"", options);
		}
		catch
		{
			throw new JsonException($"Unable to determine model type for discriminator value of \"{typeVal}\"");
		}

		if (Conversions.TryGetValue(typeDiscriminator, out Type conversion))
		{
			return (TOutput)JsonSerializer.Deserialize(ref reader, conversion, options);
		}

		throw new JsonException($"Unable to determine model type for discriminator value of \"{typeVal}\"");
	}

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, TOutput value, JsonSerializerOptions options)
		=> JsonSerializer.Serialize(writer, value, options);
}