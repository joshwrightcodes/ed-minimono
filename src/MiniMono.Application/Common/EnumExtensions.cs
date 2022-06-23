using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wright.Demo.MiniMono.Application.Common;

/// <summary>
/// Extension methods for <see cref="Enum"/>.
/// </summary>
public static class EnumExtensions
{
	/// <summary>
	/// Gets individual values from an enum with flags.
	/// </summary>
	/// <typeparam name="T">TEnum Type.</typeparam>
	/// <param name="enumeration">Flag Value.</param>
	/// <returns>Individual Flags.</returns>
	public static IEnumerable<T> GetFlagsAsEnumerable<T>(this T enumeration)
		where T : Enum
		=> Enum.GetValues(typeof(T)).Cast<T>().Where(s => enumeration.HasFlag(s));

	/// <summary>
	/// Gets Enum Member Value for a given enum value.
	/// </summary>
	/// <typeparam name="T">Enum Type.</typeparam>
	/// <param name="value">Enum Value.</param>
	/// <returns>Enum Member Value.</returns>
	public static string GetEnumMemberValue<T>(this T value)
		where T : struct, IConvertible
		=> typeof(T)
			.GetTypeInfo().DeclaredMembers
			.SingleOrDefault(x => x.Name == value.ToString())?
			.GetCustomAttribute<EnumMemberAttribute>(false)?
			.Value;

	/// <summary>
	/// Converts Enum value to string, using <see cref="EnumMemberAttribute.Value"/>
	/// where available.
	/// </summary>
	/// <typeparam name="T">The type of the enum to convert.</typeparam>
	/// <param name="value">The enum value to convert.</param>
	/// <param name="options">
	/// Options to control serialization behavior.
	/// </param>
	/// <returns>
	/// Returns value defined in <see cref="EnumMemberAttribute.Value"/> if set, otherwise
	/// returns the name of the enum value.
	/// </returns>
	/// <remarks>
	/// Uses <c>System.Text.Json</c> and <c>Macross.Json.Extensions</c> to handle the serialization.
	/// To reference <see cref="EnumMemberAttribute.Value"/>, the enum must have a
	/// <see cref="JsonConverterAttribute"/> set to use <see cref="JsonStringEnumMemberConverter"/>.
	/// </remarks>
	public static string ToEnumMemberValue<T>(this T value, JsonSerializerOptions options = default)
		where T : Enum
		=> JsonSerializer.Serialize(value, options)
			.Replace("\"", string.Empty, StringComparison.InvariantCultureIgnoreCase);

	/// <summary>
	/// Converts string value to Enum Value, using <see cref="EnumMemberAttribute.Value"/>
	/// where set.
	/// </summary>
	/// <typeparam name="T">The type of the enum to convert.</typeparam>
	/// <param name="value">The string value to convert.</param>
	/// <param name="options">Options to control the behavior during parsing.</param>
	/// <returns>
	/// Returns an enum represented by the value defined in <see cref="EnumMemberAttribute.Value"/> if set,
	/// otherwise returns an enum represented by the name of the enum value.
	/// </returns>
	/// <remarks>
	/// Uses <c>System.Text.Json</c> and <c>Macross.Json.Extensions</c> to handle the serialization.
	/// To reference <see cref="EnumMemberAttribute.Value"/>, the enum must have a
	/// <see cref="JsonConverterAttribute"/> set to use <see cref="JsonStringEnumMemberConverter"/>.
	/// </remarks>
	public static T FromEnumMemberValue<T>(this string value, JsonSerializerOptions options = default)
		where T : Enum
		=> JsonSerializer.Deserialize<T>($"\"{value}\"", options);
}